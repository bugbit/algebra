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
    public partial interface IAlgebra<T>
    {
        T Convert(int n);
    }
    public partial class Algebra<T>
    {
        public abstract T Convert(int n);
    }

    public partial class AlgebraInt
    {
        public override int Convert(int n) => n;
    }

    public partial class AlgebraLong
    {
        public override long Convert(int n) => n;
    }

    public partial class AlgebraBigInteger
    {
        public override BigInteger Convert(int n) => n;
    }

    public partial class AlgebraFloat
    {
        public override float Convert(int n) => n;
    }

    public partial class AlgebraDouble
    {
        public override double Convert(int n) => n;
    }

    public partial class AlgebraDecimal
    {
        public override decimal Convert(int n) => n;
    }

    public partial class AlgebraBigDecimal
    {
        public override BigDecimal Convert(int n) => n;
    }
}
