using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public static class Precisions
    {
        public enum EPrecisions
        {
            Integer, BigInteger, Float, Double, Decimal, BigDecimal
        }

        public static readonly IDictionary<EPrecisions, Type> PrecicionsTypes = new Dictionary<EPrecisions, Type>
        {
            [EPrecisions.Integer] = typeof(int),
            [EPrecisions.BigInteger] = typeof(BigInteger),
            [EPrecisions.Float] = typeof(float),
            [EPrecisions.Double] = typeof(double),
            [EPrecisions.Decimal] = typeof(decimal),
            [EPrecisions.BigDecimal] = typeof(BigDecimal)
        };

        public static readonly IDictionary<EPrecisions, string> PrecisionsNames = PrecicionsTypes.ToDictionary(d => d.Key, d => d.Value.Name);
    }
}
