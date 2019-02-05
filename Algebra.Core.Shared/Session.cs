using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Algebra.Core
{
    public class Session
    {
        private int mVarCounter = 0;

        public Session()
        {
            ChangeAlgebra(Algebra.Default);
        }

        public IAlgebra Alg { get; private set; }

        public Vars Vars => new Vars();

        public void ChangeAlgebra(EPrecisions p)
        {
            ChangeAlgebra(Algebra.Algebras[p].Invoke());
        }

        public void ChangeAlgebra(IAlgebra a)
        {
            Alg = a;
        }

        public string[] CreateVarsCounters(IEnumerable<string> argVarsNames)
        {
            var pCounter = Interlocked.Increment(ref mVarCounter);

            return (from n in argVarsNames select $"_{n}{pCounter}").ToArray();
        }

        public string[] PeekVarsCounters(IEnumerable<string> argVarsNames)
        {
            var pRet = CreateVarsCounters(argVarsNames);

            Interlocked.Decrement(ref mVarCounter);

            return pRet;
        }
    }
}
