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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestExpressions
{
    #region old parser with state
    //public class ParserResult
    //{
    //    public bool Finished { get; set; }
    //    public string Result { get; set; }
    //}

    //class ParserInternal
    //{
    //    private CancellationToken mTokenCancel;
    //    private TaskCompletionSource<ParserResult> mSetResultCompleted = null;
    //    private TaskCompletionSource<string> mSetExprCompleted = null;
    //    private readonly Task<string> mSetExprTask = null;
    //    private readonly object mLockGetToken = new object();
    //    private string mExpr;
    //    private string mResult;
    //    private int mPos = 0;

    //    public ParserInternal(string e, CancellationToken t)
    //    {
    //        SetExpr(e);
    //        mTokenCancel = t;
    //        Finished = false;
    //        mSetResultCompleted = new TaskCompletionSource<ParserResult>();
    //        SetResultTask = mSetResultCompleted.Task;
    //        mSetExprCompleted = new TaskCompletionSource<string>();
    //        mSetExprTask = mSetExprCompleted.Task;
    //    }

    //    public async Task<ParserResult> Continue(string e)
    //    {
    //        mSetExprCompleted.SetResult(e);

    //        mSetResultCompleted = new TaskCompletionSource<ParserResult>();
    //        SetResultTask = mSetResultCompleted.Task;

    //        return await SetResultTask;
    //    }

    //    public bool Finished { get; private set; }

    //    public Task<ParserResult> SetResultTask { get; private set; }

    //    private char? GetToken()
    //    {
    //        lock (mLockGetToken)
    //        {
    //            return (mExpr == null || mPos >= mExpr.Length) ? (char?)null : mExpr[mPos++];
    //        }
    //    }

    //    private void SetExpr(string s)
    //    {
    //        mExpr = s;
    //        mPos = 0;
    //    }

    //    private ParserResult SetResult(ParserResult r)
    //    {
    //        TaskCompletionSource<ParserResult> tc = null;

    //        lock (this)
    //        {
    //            Finished = r.Finished;
    //            tc = mSetResultCompleted;
    //        }

    //        mTokenCancel.ThrowIfCancellationRequested();

    //        if (tc == null)
    //            throw new InvalidOperationException();

    //        tc.SetResult(r);

    //        return r;
    //    }

    //    private async Task Yield(ParserResult r)
    //    {
    //        SetResult(r);
    //        Task<string> t = null;

    //        lock (this)
    //        {
    //            t = mSetExprTask;
    //        }

    //        mTokenCancel.ThrowIfCancellationRequested();

    //        if (t == null)
    //            throw new InvalidOperationException();

    //        SetExpr(await t);
    //    }

    //    public async Task Parse()
    //    {
    //        char? pCar;

    //        mResult = "";
    //        do
    //        {
    //            mTokenCancel.ThrowIfCancellationRequested();

    //            pCar = GetToken();

    //            if (pCar == null)
    //                await Yield(new ParserResult { Finished = false });
    //            else
    //                mResult += pCar;
    //        } while (pCar != ';');

    //        SetResult(new ParserResult { Finished = true, Result = mResult });
    //    }
    //}

    //public class Parser
    //{
    //    private ParserInternal mParseExpr = null;

    //    public Task<ParserResult> Parse(string s, CancellationToken c) => ParseInternal(s, c, t => t.Parse(), ref mParseExpr);

    //    private Task<ParserResult> ParseInternal(string s, CancellationToken c, Func<ParserInternal, Task> CreateParse, ref ParserInternal t)
    //    {
    //        var pStarted = false;
    //        ParserInternal tt;

    //        lock (this)
    //        {
    //            if (t == null || t.Finished)
    //            {
    //                pStarted = true;
    //                t = new ParserInternal(s, c);
    //            }
    //            tt = t;
    //        }

    //        if (pStarted)
    //        {
    //            Task.Run(() => CreateParse.Invoke(tt));

    //            return tt.SetResultTask;
    //        }

    //        return tt.Continue(s);
    //    }
    //}
    #endregion

    class TestHowDoParseAsync
    {
        static void Main()
        {
            MainAsync().Wait();

            Console.ReadLine();
        }
        static async Task MainAsync()
        {
            var t = new CancellationTokenSource();
            var s = new Algebra.Core.Session();
            var a = s.Alg;
            var r1 = await a.Parse("2+3*4", t.Token);

            Console.WriteLine(r1);
            //var r1 = await a.ParsePrompt("2+3", t.Token);

            //Print(a, r1, t.Token);

            //if (!r1.Finished)
            //{
            //    var r2 = await a.ParsePrompt("*4;", t.Token);

            //    Print(a, r2, t.Token);
            //}

            /*
            var pParse = new Parser();
            var r1 = await pParse.Parse("2+3", t.Token);

            if (r1.Finished)
                Console.WriteLine(r1.Result);

            var r2 = await pParse.Parse("*4;", t.Token);

            if (r2.Finished)
                Console.WriteLine(r2.Result);
                */
        }

        static async void Print(Algebra.Core.IAlgebra a, Algebra.Core.Exprs.ParseResult r, CancellationToken t)
        {
            if (r.Finished)
                if (r.Exprs != null)
                {
                    Print(r.Exprs);
                    foreach (var e in r.Exprs)
                        Console.WriteLine(await a.Eval(e, t));
                }
                else if (r.Ex != null)
                    Console.WriteLine($"Exception: {r.Ex.Message}");
        }

        static void Print(Algebra.Core.Exprs.NodeExpr[] es)
        {
            foreach (var e in es)
                Console.WriteLine(e);
        }

        private static string mToken;
        private static int mPos = -1;
        private readonly static object mLockGetToken = new object();
        private static Task<string> mGetTokenTask = null;
        private static TaskCompletionSource<string> mGetToken = null;

        private async static Task<char?> GetToken()
        {
            Task<string> t = null;

            lock (mLockGetToken)
            {
                t = mGetTokenTask;
                if (t != null)
                    mGetTokenTask = null;
            }
            if (t != null)
            {
                if (!t.IsCompleted)
                    await t;

                mToken = t.Result;
                mPos = -1;
            }

            if (mToken == null || (mPos + 1) >= mToken.Length)
                return null;

            return mToken[Interlocked.Increment(ref mPos)];
        }

        private async static Task<bool> Parse(string argStr)
        {
            lock (mLockGetToken)
            {
                if (mGetToken == null || mGetToken.Task.IsCompleted)
                {
                    mGetToken = new TaskCompletionSource<string>();
                    mGetTokenTask = mGetToken.Task;
                }
            }
            mGetToken.SetResult(argStr);

            return await ParseInternal();
        }

        private async static Task<bool> ParseInternal()
        {
            char? pCar;
            do
            {
                pCar = await GetToken();

                if (pCar == null)
                    return false;
            } while (pCar != ';');

            return true;
        }
    }
}
