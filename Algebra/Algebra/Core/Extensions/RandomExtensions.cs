using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Extensions
{
    public static class RandomExtensions
    {
        public static ulong Next(this Random r, ulong min, ulong max)
        {
            var hight = r.Next((int)(min >> 32), (int)(max >> 32));
            var minLow = System.Math.Min((int)min, (int)max);
            var maxLow = System.Math.Max((int)min, (int)max);
            var low = (uint)r.Next(minLow, maxLow);
            ulong result = (ulong)hight;
            result <<= 32;
            result |= (ulong)low;
            return result;
        }

        public static long Next(this Random r, long max) => (long)r.Next(0, (ulong)System.Math.Abs(max));

        public static long Next(this Random r, long min, long max) => min + (long)r.Next(0, (ulong)System.Math.Abs(max - min));
    }
}
