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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Exprs;
using Algebra.Core.Math;

namespace Algebra.Core
{
    public partial interface IAlgebra
    {
        Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null);
        Task<bool> PrimeP(NodeExpr e, EAlgorithmPrimeP a, CancellationToken t);
    }

    public partial class Algebra
    {
        public abstract Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null);
        public abstract Task<bool> PrimeP(NodeExpr e, EAlgorithmPrimeP a, CancellationToken t);
    }

    public partial interface IAlgebra<T> where T : struct
    {
        Task<T?> EvalToNumber(NodeExpr e, CancellationToken t, IVars v = null);
    }

    public partial class Algebra<T> where T : struct
    {
        public override Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null) => EvaluateVisitor<T>.Eval(e, this, t, v);

        public async Task<T?> EvalToNumber(NodeExpr e, CancellationToken t, IVars v = null)
        {
            var r = await Eval(e, t, v);

            t.ThrowIfCancellationRequested();

            if (r is NodeExprNumber<T> en)
                return en.Value;

            return null;
        }

        public override async Task<bool> PrimeP(NodeExpr e, EAlgorithmPrimeP a, CancellationToken t)
        {
            var n = await EvalToNumber(e, t);

            return n.HasValue && IsNumberInteger(n.Value) && await PrimeP(n.Value, a, t);
        }
    }
}
