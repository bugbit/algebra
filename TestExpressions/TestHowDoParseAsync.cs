using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestExpressions
{
    class TestHowDoParseAsync
    {
        static void Main()
        {
            MainAsync().Wait();
        }
        static async Task MainAsync()
        {
            var ok = await Parse("2+3");

            ok = await Parse("*4;");
        }

        private static string mToken;
        private static int mPos = -1;
        private static object mLockGetToken = new object();
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
