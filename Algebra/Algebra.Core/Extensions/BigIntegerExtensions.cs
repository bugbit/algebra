using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deveel.Math;

namespace Algebra.Core.Extensions
{
    public static class BigIntegerExtensions
    {
        // Source: http://mjs5.com/2016/01/20/c-biginteger-square-root-function/  Michael Steiner, Jan 2016
        // Slightly modified to correct error below 6. (thank you M Ktsis D) 
        public static BigInteger Sqrt(this BigInteger number)
        {
            if (number < 9)
            {
                if (number == 0)
                    return 0;
                if (number < 4)
                    return 1;
                else
                    return 2;
            }

            BigInteger n = 0, p = 0;
            var high = number >> 1;
            var low = BigInteger.Zero;

            while (high > low + 1)
            {
                n = (high + low) >> 1;
                p = n * n;
                if (number < p)
                {
                    high = n;
                }
                else if (number > p)
                {
                    low = n;
                }
                else
                {
                    break;
                }
            }
            return number == p ? n : low;
        }
    }
}
