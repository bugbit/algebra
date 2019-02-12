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
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    Expression, UnaryOperation, Operation, EOF, Token
                }

                public EType TypeTerm { get; set; }
                public NodeExpr Expr { get; set; }
                public ETypeBinary TypeBinary { get; set; }
                public ETypeUnary TypeUnary { get; set; }
            }

            private IAlgebra<T> mAlg;
            private CancellationToken mTokenCancel;
            private TaskCompletionSource<ParseResult> mSetResultCompleted = null;
            private TaskCompletionSource<string> mSetExprCompleted = null;
            private readonly Task<string> mSetExprTask = null;
            private Tokenizer mTokenizer;

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
                try
                {
                    var pExprs = new List<NodeExprInstruction>();

                    for (; ; )
                    {
                        mTokenCancel.ThrowIfCancellationRequested();

                        var pExpr = await ParseInstruction();

                        pExprs.Add(pExpr);

                        await ReadToken();
                        Back();

                        lock (this)
                        {
                            if (mTokenizer.TypeToken == Tokenizer.EType.EOF)
                                break;
                        }
                    }

                    SetResult(new ParseResult { Finished = true, Exprs = pExprs.ToArray() });
                }
                catch (Exception ex)
                {
                    SetResult(new ParseResult { Finished = true, Ex = ex });
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

            private async Task<NodeExprInstruction> ParseInstruction()
            {
                for (; ; )
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    var pExpr = await ParseExpr();
                    Tokenizer.EType pType;

                    lock (this)
                    {
                        pType = mTokenizer.TypeToken;

                        switch (mTokenizer.TypeToken)
                        {
                            case Tokenizer.EType.TerminateDolar:
                            case Tokenizer.EType.TerminateSemiColon:
                                return NodeExpr.Instruction(pExpr, pType == Tokenizer.EType.TerminateSemiColon);
                        }
                    }
                }
            }

            private Task<NodeExpr> ParseExpr() => ParseExprOperations();

            private async Task<NodeExpr> ParseExprOperations()
            {
                var pStackExpr = new Stack<NodeExpr>();
                var pStackOps = new Stack<ETypeBinary>();
                var pExit = false;

                do
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    var pTerm = await ParseTerm();

                    switch (pTerm.TypeTerm)
                    {
                        case ParseTermResult.EType.Expression:
                            pStackExpr.Push(pTerm.Expr);
                            break;
                        case ParseTermResult.EType.UnaryOperation:
                            var pExpr = await ParseExpr();

                            pStackExpr.Push(NodeExpr.Unary(pTerm.TypeUnary, pExpr));
                            break;
                        case ParseTermResult.EType.Operation:
                            if (pStackOps.Count <= 0)
                            {
                                pStackOps.Push(pTerm.TypeBinary);
                            }
                            else
                            {
                                var t = pStackOps.Peek();

                                if (MathExpr.TypeBinariesPriorities[pTerm.TypeBinary] > MathExpr.TypeBinariesPriorities[t])
                                    EvalOperation(pStackExpr, pStackOps);
                                pStackOps.Push(pTerm.TypeBinary);
                            }
                            break;
                        case ParseTermResult.EType.EOF:
                            await Yield(new ParseResult { Finished = false });
                            break;
                        default:
                            pExit = true;
                            break;

                    }
                } while (!pExit);

                while (pStackOps.Count > 0)
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    EvalOperation(pStackExpr, pStackOps);
                }

                if (pStackExpr.Count != 1)
                    throw new InvalidOperationException();

                return pStackExpr.Pop();
            }

            private void EvalOperation(Stack<NodeExpr> se, Stack<ETypeBinary> so)
            {
                var pType = so.Pop();

                if (se.Count < 2)
                    throw new InvalidOperationException();

                var r = se.Pop();
                var l = se.Pop();

                se.Push(NodeExpr.Binary(pType, l, r));
            }

            private async Task<ParseTermResult> ParseTerm()
            {
                await ReadToken();
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
                    case Tokenizer.EType.Sign:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.UnaryOperation, TypeUnary = MathExpr.StrToUnary[pToken] };
                    case Tokenizer.EType.Operation:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Operation, TypeBinary = MathExpr.StrToBinaries[pToken] };
                    case Tokenizer.EType.EOF:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.EOF };
                    default:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Token };
                }
            }

            private async Task ReadToken() => await mTokenizer.Read(mTokenCancel);

            private void Back()
            {
                mTokenizer.Back();
            }
        }

        private ParserInternal mParser = null;
    }
}
