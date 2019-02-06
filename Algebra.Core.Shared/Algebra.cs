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
using System.Numerics;
using System.Text;

namespace Algebra.Core
{
    public partial interface IAlgebra
    {
    }

    public partial interface IAlgebra<T> : IAlgebra
    {
    }

    public abstract partial class Algebra : IAlgebra
    {
        public static readonly IDictionary<EPrecisions, Func<IAlgebra>> Algebras = new Dictionary<EPrecisions, Func<IAlgebra>>()
        {
            [EPrecisions.Integer] = () => new AlgebraInt(),
            [EPrecisions.Long] = () => new AlgebraLong(),
            [EPrecisions.BigInteger] = () => new AlgebraBigInteger(),
            [EPrecisions.Float] = () => new AlgebraFloat(),
            [EPrecisions.Double] = () => new AlgebraDouble(),
            [EPrecisions.Decimal] = () => new AlgebraDecimal(),
            [EPrecisions.BigDecimal] = () => new AlgebraBigDecimal()
        };
        private static readonly Lazy<IAlgebra> mDefault = new Lazy<IAlgebra>(Algebras[EPrecisions.Decimal]);

        public static IAlgebra Default => mDefault.Value;
    }

    public partial class Algebra<T> : Algebra, IAlgebra<T>
    {
    }

    public sealed partial class AlgebraInt : Algebra<int>
    {
    }

    public sealed partial class AlgebraLong : Algebra<long>
    {

    }

    public sealed partial class AlgebraBigInteger : Algebra<BigInteger>
    {

    }

    public sealed partial class AlgebraFloat : Algebra<float>
    {

    }

    public sealed partial class AlgebraDouble : Algebra<double>
    {

    }

    public partial class AlgebraDecimal : Algebra<decimal>
    {

    }

    public sealed partial class AlgebraBigDecimal : Algebra<BigDecimal>
    {

    }
}
