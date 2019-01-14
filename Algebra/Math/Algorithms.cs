using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Math
{
    public static class Algorithms
    {
        /* This function calculates (ab)%c */
        public static int Module(dynamic a, dynamic b, dynamic c)
        {
            dynamic x = a / a, y = a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % c;
                }
                y = (y * y) % c; // squaring the base
                b /= 2;
            }

            return x % c;
        }
        public static bool IsPrime(dynamic argNumber)
        {
            if ((argNumber % 2) == 0)
                return false;

            return true;
        }
    }
}
