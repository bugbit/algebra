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
using System.Globalization;
using System.Text;

namespace Algebra.Core
{
    public partial interface IAlgebra
    {
        object Parse(string str);
    }
    public partial class Algebra
    {
        public virtual object Parse(string str) => null;
    }
    public partial interface IAlgebra<T>
    {
        T ParseT(string str);
    }

    public partial class Algebra<T>
    {
        public override object Parse(string str) => ParseT(str);
        public virtual T ParseT(string str) => default(T);
        //public override object Parse(string str)
        //{
        //    return base.Parse(str);
        //}
        //new public virtual T Parse(string str) => default(T);
    }

    public partial class AlgebraInt
    {
        public override int ParseT(string str) => (int)decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }
}
