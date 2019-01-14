using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Math
{
    public static class Algorithms
    {
        public static bool IsPrime(dynamic argNumber)
        {
            if ((argNumber % 2) == 0)
                return false;

            return true;
        }
    }
}
