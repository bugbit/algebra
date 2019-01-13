using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Math
{
    public class Algorithms<T>
    {
        public bool IsPrime(T argNumber)
        {
            return IsPrimeD(argNumber);
        }

        private bool IsPrimeD(dynamic argNumber)
        {
            if ((argNumber % 2) == 0)
                return false;

            return true;
        }
    }
}
