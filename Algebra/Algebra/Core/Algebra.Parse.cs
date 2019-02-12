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
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Exprs;

namespace Algebra.Core
{
    public partial interface IAlgebra
    {
        Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t);
        Task<Exprs.NodeExpr> Parse(string str, CancellationToken t);
    }

    public partial interface IAlgebra<T>
    {
        T ParseNumber(string str);
        //Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t);
    }

    public partial class Algebra : IAlgebra
    {
        public abstract Task<Exprs.NodeExpr> Parse(string str, CancellationToken t);
        public virtual Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t) => Task.FromResult(new Exprs.ParseResult { Finished = true, Exprs = new[] { Exprs.NodeExpr.Null } });
    }

    public partial class Algebra<T>
    {
        private Exprs.Parser<T> mParse;

        private void InitializeParse()
        {
            mParse = new Exprs.Parser<T>(this);
        }

        public virtual T ParseNumber(string str) => default(T);

        public override Task<NodeExpr> Parse(string str, CancellationToken t) => mParse.Parse(str, t);

        //public override object Parse(string str)
        //{
        //    return base.Parse(str);
        //}
        //new public virtual T Parse(string str) => default(T);
        public override Task<Exprs.ParseResult> ParsePrompt(string str, CancellationToken t) => mParse.ParsePrompt(str, t);
    }

    public partial class AlgebraInt
    {
        public override int ParseNumber(string str) => (int)decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraLong
    {
        public override long ParseNumber(string str) => (long)decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraBigInteger
    {
        public override BigInteger ParseNumber(string str) => (BigInteger)decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraFloat
    {
        public override float ParseNumber(string str) => float.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraDouble
    {
        public override double ParseNumber(string str) => double.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraDecimal
    {
        public override decimal ParseNumber(string str) => decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }

    public partial class AlgebraBigDecimal
    {
        public override BigDecimal ParseNumber(string str) => decimal.Parse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
    }
}
