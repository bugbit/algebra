using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Extensions
{
    public static class RandomExtensions
    {
        public static int RandInt(this Random r) => r.Next();

        public static long RandLong(this Random r)
        {
            var b = new byte[8];

            r.NextBytes(b);

            return BitConverter.ToInt64(b, 0);
        }

        /*
The decimal type has the same components as any other floating point number: a mantissa, an exponent and a sign. As usual, the sign is just a single bit, but there are 96 bits of mantissa and 5 bits of exponent. 
However, not all exponent combinations are valid. Only values 0-28 work, and they are effectively all negative: the numeric value is sign * mantissa / 10exponent. 
This means the maximum and minimum values of the type are +/- (296-1), and the smallest non-zero number in terms of absolute magnitude is 10-28.         
         */
        public static float RandFloat(this Random r) => r.Next() % 0x7FFFFF;

        public static double RandDouble(this Random r) => r.RandLong() % (1L << 52);

        public static decimal RandDecimal(this Random r)
        {
            var b = new byte[12];

            r.NextBytes(b);
            b[3] = 0;

            return new decimal(BitConverter.ToInt32(b, 0), BitConverter.ToInt32(b, 4), BitConverter.ToInt32(b, 8), false, 0);
        }

        public static long Next(this Random r, long max) => r.RandLong() % max;

        public static decimal Next(this Random r, decimal max) => r.RandDecimal() % max;
    }
}
