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
        Session Session { get; }
    }

    public partial interface IAlgebra<T> : IAlgebra
    {
    }

    public abstract partial class Algebra : IAlgebra
    {
        public static readonly IDictionary<EPrecisions, Func<Session, IAlgebra>> Algebras = new Dictionary<EPrecisions, Func<Session, IAlgebra>>()
        {
            [EPrecisions.Integer] = s => new AlgebraInt(s),
            [EPrecisions.Long] = s => new AlgebraLong(s),
            [EPrecisions.BigInteger] = s => new AlgebraBigInteger(s),
            [EPrecisions.Float] = s => new AlgebraFloat(s),
            [EPrecisions.Double] = s => new AlgebraDouble(s),
            [EPrecisions.Decimal] = s => new AlgebraDecimal(s),
            [EPrecisions.BigDecimal] = s => new AlgebraBigDecimal(s)
        };

        public Algebra(Session s)
        {
            Session = s;
            Initialize();
        }

        public Session Session { get; }

        protected virtual void Initialize()
        {
        }
    }

    public abstract partial class Algebra<T> : Algebra, IAlgebra<T>
    {
        public Algebra(Session s) : base(s)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            InitializeParse();
            InitializeBinaryOperations();
        }
    }

    public sealed partial class AlgebraInt : Algebra<int>
    {
        public AlgebraInt(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraLong : Algebra<long>
    {
        public AlgebraLong(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraBigInteger : Algebra<BigInteger>
    {
        public AlgebraBigInteger(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraFloat : Algebra<float>
    {
        public AlgebraFloat(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraDouble : Algebra<double>
    {
        public AlgebraDouble(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraDecimal : Algebra<decimal>
    {
        public AlgebraDecimal(Session s) : base(s)
        {
        }
    }

    public sealed partial class AlgebraBigDecimal : Algebra<BigDecimal>
    {
        public AlgebraBigDecimal(Session s) : base(s)
        {
        }
    }
}
