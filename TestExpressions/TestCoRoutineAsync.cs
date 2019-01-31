using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExpressions
{
    public sealed class Coordinator : IEnumerable
    {
        private readonly Queue<Action> actions = new Queue<Action>();
        // Used by collection initializer to specify the coroutines to run 
        public void Add(Action<Coordinator> coroutine)
        {
            actions.Enqueue(() => coroutine(this));
        }

        // Required for collection initializers, but we don’t really want 
        // to expose anything. 
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException("IEnumerable only supported to enable collection initializers");
        }
    }

    class TestCoRoutineAsync
    {
        static string mStr;

        private static TaskFactory coroutineFactory = new TaskFactory(new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler);

        /// <summary>
        /// Executes a co-routine using an exclusive scheduler.
        /// </summary>
        private static async Task RunCoroutineAsync(Func<Task> coroutine)
        {
            await await coroutineFactory.StartNew(coroutine);
        }

        static void Main()
        {
            MainAsync().Wait();
        }

        async static Task Parse()
        {
            Console.WriteLine(mStr);
            await Task.Yield();
            Console.WriteLine(mStr);
            await Task.Yield();
        }

        async static Task Parse2()
        {
            Console.WriteLine(mStr);
            await Task.Yield();
            Console.WriteLine(mStr);
            await Task.Yield();
        }
        // https://stackoverflow.com/questions/22852251/async-await-as-a-replacement-of-coroutines

        static async Task MainAsync()
        {
            //mStr = "10";
            //await Task.WhenAll(RunCoroutineAsync(Parse), RunCoroutineAsync(Parse2));
        }

        static async Task StartCoRoutine(Task<IEnumerator> t)
        {
            var e = await t;

            while (e.MoveNext())
            {
                var r = e.Current;

                if (r is Task)
                    await (Task)r;
            }
        }

        static IEnumerator Parse11()
        {
            Console.WriteLine("Entry 11");
            yield return Parse12();
            Console.WriteLine("Entry Yield 11");
            yield return Task.FromResult<object>(null);
            Console.WriteLine("Finish 11");
            yield return Task.FromResult<object>(null);
        }

        static IEnumerator Parse12()
        {
            Console.WriteLine("Entry 12");
            yield return Task.FromResult<object>(null);
            Console.WriteLine("Entry Yield 12");
            Console.WriteLine("Finish 12");
            yield return Task.FromResult<object>(null);
        }
    }
}
