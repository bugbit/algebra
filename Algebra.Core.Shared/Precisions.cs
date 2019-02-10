#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Algebra.Core
{
    public enum EPrecisions
    {
        Integer, Long, BigInteger, Float, Double, Decimal, BigDecimal, Default = Decimal
    }

    public static class Precisions
    {
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
            //[EPrecisions.BigDecimal] = new Info(typeof(BigDecimal))
        };

        public static readonly IDictionary<Type, Info> PrecisionsTypes = PrecicionsInfo.ToDictionary(d => d.Value.TypePrecision, d => d.Value);
    }
}
