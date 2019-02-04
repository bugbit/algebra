#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Exprs;

namespace Algebra.Core.Exprs
{
    public sealed class Parser<T>
    {
        private readonly IAlgebra<T> mAlg;

        public Parser(IAlgebra<T> a)
        {
            mAlg = a;
        }

        public Task<ParseResult> Parse(string s, CancellationToken c)
        {
            var pStarted = false;
            ParserInternal tt;

            lock (this)
            {
                if (mParser == null || mParser.Finished)
                {
                    pStarted = true;
                    mParser = new ParserInternal(mAlg, s, c);
                }
                tt = mParser;
            }

            if (pStarted)
            {
                Task.Run(tt.Parse);

                return tt.SetResultTask;
            }

            return tt.Continue(s);
        }

        private class ParserInternal
        {
            [DebuggerDisplay("{TypeTerm} {TypeBinary} {Token} {Expr}")]
            private class ParseTermResult
            {
                public enum EType
                {
                    Expression, UnaryOperation, Operation, EOF, Terminate
                }

                public ETypeUnary TypeUnary { get; set; }
                public ETypeBinary TypeBinary { get; set; }
                public EType TypeTerm { get; set; }
                public NodeExpr Expr { get; set; }
                public string Token { get; set; }
                public Tokenizer.EType TypeToken { get; set; }
            }

            private IAlgebra<T> mAlg;
            private CancellationToken mTokenCancel;
            private TaskCompletionSource<ParseResult> mSetResultCompleted = null;
            private TaskCompletionSource<string> mSetExprCompleted = null;
            private readonly Task<string> mSetExprTask = null;
            private Tokenizer mTokenizer;
            private ConcurrentStack<ConcurrentStack<NodeExpr>> mStackExprs = new ConcurrentStack<ConcurrentStack<NodeExpr>>();
            private ConcurrentStack<ParseTermResult> mStackOperations = new ConcurrentStack<ParseTermResult>();

            public ParserInternal(IAlgebra<T> a, string e, CancellationToken t)
            {
                mAlg = a;
                SetExpr(e);
                mTokenCancel = t;
                Finished = false;
                mSetResultCompleted = new TaskCompletionSource<ParseResult>();
                SetResultTask = mSetResultCompleted.Task;
                mSetExprCompleted = new TaskCompletionSource<string>();
                mSetExprTask = mSetExprCompleted.Task;
            }

            public bool Finished { get; private set; }

            public Task<ParseResult> SetResultTask { get; private set; }

            public async Task<ParseResult> Continue(string e)
            {
                mSetExprCompleted.SetResult(e);

                mSetResultCompleted = new TaskCompletionSource<ParseResult>();
                SetResultTask = mSetResultCompleted.Task;

                return await SetResultTask;
            }

            public async Task Parse()
            {
                mStackExprs = new ConcurrentStack<ConcurrentStack<NodeExpr>>();
                mStackOperations = new ConcurrentStack<ParseTermResult>();
                for (; ; )
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    var pTerm = await ParseTerm();

                    switch (pTerm.TypeTerm)
                    {
                        case ParseTermResult.EType.Expression:
                            AddExprToStack(pTerm.Expr);
                            break;
                        case ParseTermResult.EType.Operation:
                            lock (mStackOperations)
                            {
                                var t = PeekOperation();

                                if (t != null && MathExpr.TypeBinariesPriorities[pTerm.TypeBinary] > MathExpr.TypeBinariesPriorities[t.TypeBinary])
                                    EvalOperation();
                                AddOperationToExpr(pTerm);
                            }
                            break;
                        case ParseTermResult.EType.EOF:
                            var es = await ExprInstruccion();

                            if (es != null)
                            {
                                SetResult(new ParseResult { Finished = true, Exprs = es });

                                return;
                            }
                            await Yield(new ParseResult { Finished = false });
                            break;
                        case ParseTermResult.EType.Terminate:
                            await Eval(t => false);

                            if (!TryPopExpr(out NodeExpr e))
                                throw new NullReferenceException(nameof(e));

                            AddExprToStack(NodeExpr.Instruction(e, pTerm.TypeToken == Tokenizer.EType.TerminateSemiColon));
                            break;
                    }
                }
            }

            private void SetExpr(string s)
            {
                mTokenizer = new Tokenizer(s);
            }

            private void SetResult(ParseResult r)
            {
                TaskCompletionSource<ParseResult> tc = null;

                lock (this)
                {
                    Finished = r.Finished;
                    tc = mSetResultCompleted;
                }

                mTokenCancel.ThrowIfCancellationRequested();

                if (tc == null)
                    throw new InvalidOperationException();

                tc.SetResult(r);
            }

            private async Task Yield(ParseResult r)
            {
                SetResult(r);
                Task<string> t = null;

                lock (this)
                {
                    t = mSetExprTask;
                }

                mTokenCancel.ThrowIfCancellationRequested();

                if (t == null)
                    throw new InvalidOperationException();

                SetExpr(await t);
            }

            private ParseTermResult PeekOperation()
            {
                lock (mStackOperations)
                {
                    if (mStackOperations.IsEmpty)
                        return null;

                    if (!mStackOperations.TryPeek(out ParseTermResult t))
                        return null;

                    return t;
                }
            }

            private ConcurrentStack<NodeExpr> AddStackExpr()
            {
                var s = new ConcurrentStack<NodeExpr>();

                mStackExprs.Push(s);

                return s;
            }

            private void AddExprToStack(NodeExpr e)
            {
                ConcurrentStack<NodeExpr> s;

                if (mStackExprs.IsEmpty)
                {
                    s = AddStackExpr();
                }
                else
                {
                    mStackExprs.TryPeek(out s);
                }
                s.Push(e);
            }

            private void AddOperationToExpr(ParseTermResult t)
            {
                mStackOperations.Push(t);
            }

            private bool TryPeekStackExpr(out ConcurrentStack<NodeExpr> se)
            {
                return mStackExprs.TryPeek(out se);
            }

            private bool TryPeekExpr(out NodeExpr e)
            {
                if (TryPeekStackExpr(out ConcurrentStack<NodeExpr> se))
                    return se.TryPeek(out e);

                e = null;

                return false;
            }

            private Task<NodeExprInstruction[]> ExprInstruccion()
            {
                return Task.Run
                (
                    () =>
                    {
                        if (mStackExprs.IsEmpty)
                            return null;

                        var es = new List<NodeExprInstruction>();

                        foreach (var se in mStackExprs)
                        {
                            if (se.Count != 1)
                                return null;

                            if (!(se.First() is NodeExprInstruction ei))
                                return null;

                            es.Add(ei);
                        }

                        if (es.Count <= 0)
                            return null;

                        return es.ToArray();
                    }
                );
            }

            private bool TryPopExpr(out NodeExpr e)
            {
                if (!TryPeekStackExpr(out ConcurrentStack<NodeExpr> se))
                {
                    e = null;

                    return false;
                }

                if (!se.TryPop(out e))
                    return false;

                if (se.IsEmpty)
                    return TryPopStackExpr(out se);

                return true;
            }

            private bool TryPopStackExpr(out ConcurrentStack<NodeExpr> se) => mStackExprs.TryPop(out se);

            private bool TryPopOperation(out ParseTermResult t) => mStackOperations.TryPop(out t);

            private NodeExpr EvalOperation()
            {
                return (!TryPopOperation(out ParseTermResult t)) ? null : EvalOperation(t);
            }

            private NodeExpr EvalOperation(ParseTermResult t)
            {
                NodeExpr e;

                switch (t.TypeTerm)
                {
                    case ParseTermResult.EType.UnaryOperation:
                        if (!TryPopExpr(out NodeExpr ee))
                            return null;

                        e = NodeExpr.Unary(t.TypeUnary, ee);

                        AddExprToStack(e);

                        return e;
                    case ParseTermResult.EType.Operation:
                        if (!TryPopExpr(out NodeExpr r))
                            return null;
                        if (!TryPopExpr(out NodeExpr l))
                            return null;

                        e = NodeExpr.Binary(t.TypeBinary, l, r);

                        AddExprToStack(e);

                        return e;
                    default:
                        return null;
                }
            }

            private Task<NodeExpr> Eval(Func<ParseTermResult, bool> exit)
            {
                return Task.Run
                (
                    () =>
                    {
                        NodeExpr e = null;

                        while (TryPopOperation(out ParseTermResult t))
                        {
                            mTokenCancel.ThrowIfCancellationRequested();

                            if (exit.Invoke(t))
                                break;

                            e = EvalOperation(t);
                        }

                        return e;
                    }
                );
            }

            private async Task<ParseTermResult> ParseTerm()
            {
                await mTokenizer.Read(mTokenCancel);
                Tokenizer.EType pType;
                string pValue;
                string pToken;

                lock (mTokenizer)
                {
                    pType = mTokenizer.TypeToken;
                    pValue = mTokenizer.Value;
                    pToken = mTokenizer.Token;
                }

                switch (pType)
                {
                    case Tokenizer.EType.Number:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Expression, Expr = NodeExpr.Number(mAlg.ParseNumber(pValue)) };
                    case Tokenizer.EType.Operation:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Operation, TypeBinary = MathExpr.StrToBinaries[pToken], Token = pToken };
                    case Tokenizer.EType.EOF:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.EOF };
                    case Tokenizer.EType.TerminateDolar:
                    case Tokenizer.EType.TerminateSemiColon:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Terminate, TypeToken = pType };
                }

                throw new InvalidOperationException();
            }
        }

        private ParserInternal mParser = null;
    }
}
