using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public sealed class Parser
    {
        public Task<ParseResult> ParsePrompt(string s, CancellationToken c) => ParseInternal(s, c, t => t.ParsePrompt(), ref mParsePrompt);

        private class ParserInternal
        {
            private CancellationToken mTokenCancel;
            private TaskCompletionSource<ParseResult> mSetResultCompleted = null;
            private TaskCompletionSource<string> mSetExprCompleted = null;
            private Task<string> mSetExprTask = null;

            public ParserInternal(string e, CancellationToken t)
            {
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

            public Task ParsePrompt()
            {
                return Task.Run(() => SetResult(new ParseResult { Finished = true });
            }

            private void SetExpr(string s)
            {

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
                    t = new ParserInternal(s, c);
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
