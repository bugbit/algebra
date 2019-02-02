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
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core
{
    public partial interface IAlgebra<T>
    {
        T ParseNumber(string str);
        Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t);
    }

    public partial class Algebra<T>
    {
        private Exprs.Parser<T> mParse;

        public Algebra()
        {
            mParse = new Exprs.Parser<T>(this);
        }

        public virtual T ParseNumber(string str) => default(T);
        //public override object Parse(string str)
        //{
        //    return base.Parse(str);
        //}
        //new public virtual T Parse(string str) => default(T);
        public Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t) => mParse.Parse(str, t);
    }

    public partial class AlgebraInt
    {
        public override int ParseNumber(string str) => (int)decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }
}
