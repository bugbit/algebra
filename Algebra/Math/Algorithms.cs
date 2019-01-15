using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Math
{
    public static class Algorithms
    {
        #region Module
        /* This function calculates (ab)%c */
        public static int Module(int a, int b, int c)
        {
            long x = 1, y = a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % c;
                }
                y = (y * y) % c; // squaring the base
                b /= 2;
            }

            return (int)(x % c);
        }
        public static long Module(long a, long b, long c)
        {
            decimal x = 1, y = a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % c;
                }
                y = (y * y) % c; // squaring the base
                b /= 2;
            }

            return (long)(x % c);
        }
        public static float Module(float a, float b, float c)
        {
            double x = 1, y = a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % c;
                }
                y = (y * y) % c; // squaring the base
                b /= 2;
            }

            return (float)(x % c);
        }
        public static double Module(double a, double b, double c)
        {
            decimal x = 1, y = (decimal)a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % (decimal)c;
                }
                y = (y * y) % (decimal)c; // squaring the base
                b /= 2;
            }

            return (double)(x % (decimal)c);
        }
        public static decimal Module(decimal a, decimal b, decimal c)
        {
            BigInteger x = 1, y = (BigInteger)a; // long long is taken to avoid overflow of intermediate results

            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % (BigInteger)c;
                }
                y = (y * y) % (BigInteger)c; // squaring the base
                b /= 2;
            }

            return (decimal)(x % (BigInteger)c);
        }
        public static BigInteger Module(BigInteger a, BigInteger b, BigInteger c)
        {
            BigInteger x = 1, y = a; // long long is taken to avoid overflow of intermediate results

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
        //public static decimal Module(BigDecimal a, BigDecimal b, BigDecimal c)
        //{
        //    BigDecimal x = 1, y = a; // long long is taken to avoid overflow of intermediate results

        //    while (b > 0)
        //    {
        //        if (b % (BigDecimal)2 == 1)
        //        {
        //            x = (x * y) % c;
        //        }
        //        y = (y * y) % c; // squaring the base
        //        b /= 2;
        //    }

        //    return (decimal)(x % c);
        //}

        public static dynamic Module(dynamic a, dynamic b, dynamic c) => Module(a, b, c);

        #endregion
        public static bool IsPrime(dynamic argNumber)
        {
            if ((argNumber % 2) == 0)
                return false;

            return true;
        }
    }
}
