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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Exprs;

namespace Algebra.Core.Exprs
{
    public sealed class Parser<T>
    {
        private IAlgebra<T> mAlg;

        public Parser(IAlgebra<T> a)
        {
            mAlg = a;
        }

        public Task<ParseResult> ParsePrompt(string s, CancellationToken c) => ParseInternal(s, c, t => t.ParsePrompt(), ref mParsePrompt);

        private class ParserInternal
        {
            private class ParseTermResult
            {
                public enum EType
                {
                    Expression, Operation, EOF, Terminate
                }

                public EType TypeTerm { get; set; }

                public NodeExpr Expr { get; set; }
                public string Token { get; set; }
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

            public async Task ParsePrompt()
            {
                var r = await ParseInstrs();

                SetResult(new ParseResult { Finished = true, Exprs = r });
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

            private async Task<NodeExprInstruction[]> ParseInstrs()
            {
                var pes = new List<NodeExprInstruction>();
                bool pExit = false;

                do
                {
                    var ei = await ParseInstr();

                    pes.Add(ei);
                    await mTokenizer.Read(mTokenCancel);
                    mTokenizer.Back();
                    lock (mTokenizer)
                    {
                        pExit = mTokenizer.TypeToken == Tokenizer.EType.EOF;
                    }
                } while (!pExit);

                return pes.ToArray();
            }

            private async Task<NodeExprInstruction> ParseInstr()
            {
                NodeExpr e0 = null;
                NodeExpr e = null;
                Tokenizer.EType pType;

                for (; ; )
                {
                    e = await ParseExpr(e0);
                    lock (this)
                    {
                        pType = mTokenizer.TypeToken;
                    }

                    if (e == null || (pType != Tokenizer.EType.TerminateDolar && pType != Tokenizer.EType.TerminateSemiColon))
                    {
                        await Yield(new ParseResult { Finished = false });

                        e0 = e;
                    }
                    else
                        break;
                }

                var ei = new NodeExprInstruction(e, pType == Tokenizer.EType.TerminateSemiColon);

                return ei;
            }

            private async Task<NodeExpr> ParseExpr(NodeExpr e0)
            {
                NodeExpr e = e0;
                NodeExpr r;

                for (; ; )
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    r = await ParseFactor(e);

                    if (r == null)
                        return e;
                    e = r;
                }
            }

            private async Task<NodeExpr> ParseFactor(NodeExpr e)
            {
                NodeExpr el = null;
                NodeExpr er = null;
                ETypeBinary? b = null;

                while (el == null || er == null)
                {
                    mTokenCancel.ThrowIfCancellationRequested();

                    var pTerm = await ParseTerm();

                    switch (pTerm.TypeTerm)
                    {
                        case ParseTermResult.EType.Expression:
                            if (el == null)
                            {
                                el = pTerm.Expr;
                            }
                            else if (er == null)
                            {
                                er = pTerm.Expr;

                                var bb = b ?? ETypeBinary.Mult;

                                return NodeExpr.Binary(bb, el, er);
                            }
                            break;
                        case ParseTermResult.EType.Operation:
                            b = MathExpr.StrToBinaries[pTerm.Token];

                            if (el == null)
                            {
                                if (b == ETypeBinary.Add)
                                {
                                    el = await ParseExpr(null);

                                    return (el != null) ? NodeExpr.Binary(ETypeBinary.Mult, NodeExpr.Number<T>(mAlg.Convert(1)), el) : null;
                                }
                                if (b == ETypeBinary.Sub)
                                {
                                    el = await ParseExpr(null);

                                    return (el != null) ? NodeExpr.Binary(ETypeBinary.Mult, NodeExpr.Number<T>(mAlg.Convert(-1)), el) : null;
                                }
                            }
                            break;
                        case ParseTermResult.EType.EOF:
                            if (el != null)
                                return el;

                            await Yield(new ParseResult { Finished = false });

                            break;
                        case ParseTermResult.EType.Terminate:
                            if (el == null)
                                return null;

                            throw new InvalidOperationException();
                    }
                }

                throw new InvalidOperationException();
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
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Operation, Token = pToken };
                    case Tokenizer.EType.EOF:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.EOF };
                    case Tokenizer.EType.TerminateDolar:
                    case Tokenizer.EType.TerminateSemiColon:
                        return new ParseTermResult { TypeTerm = ParseTermResult.EType.Terminate, Token = pToken };
                }

                throw new InvalidOperationException();
            }
        }

        private ParserInternal mParsePrompt = null;

        private Task<ParseResult> ParseInternal(string s, CancellationToken c, Func<ParserInternal, Task> CreateParse, ref ParserInternal t)
        {
            var pStarted = false;
            ParserInternal tt;

            lock (this)
            {
                if (t == null || t.Finished)
                {
                    pStarted = true;
                    t = new ParserInternal(mAlg, s, c);
                }
                tt = t;
            }

            if (pStarted)
            {
                Task.Run(() => CreateParse.Invoke(tt));

                return tt.SetResultTask;
            }

            return tt.Continue(s);
        }
    }
}
