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
            Integer, Long, BigInteger, Float, Double, Decimal, BigDecimal
        }

        public class Info
        {
            public Info() { }
            public Info(Type argType)
            {
                TypePrecision = argType;
                Name = argType.Name;
            }

            public string Name { get; set; }
            public Type TypePrecision { get; set; }
        }

        public static readonly IDictionary<EPrecisions, Info> PrecicionsInfo = new Dictionary<EPrecisions, Info>
        {
            [EPrecisions.Integer] = new Info(typeof(int)),
            [EPrecisions.Long] = new Info(typeof(long)),
            [EPrecisions.BigInteger] = new Info(typeof(BigInteger)),
            [EPrecisions.Float] = new Info(typeof(float)),
            [EPrecisions.Double] = new Info(typeof(double)),
            [EPrecisions.Decimal] = new Info(typeof(decimal)),
            [EPrecisions.BigDecimal] = new Info(typeof(BigDecimal))
        };
    }
}
