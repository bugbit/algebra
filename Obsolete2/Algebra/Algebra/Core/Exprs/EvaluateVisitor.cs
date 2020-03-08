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

namespace Algebra.Core.Exprs
{
    public class EvaluateVisitor<N> : NodeExprVisitorRetExprASync<N> where N : struct
    {
        private readonly IAlgebra<N> mAlg;
        private readonly IVars mVars;

        public EvaluateVisitor(IAlgebra<N> a, IVars v)
        {
            mAlg = a;
            mVars = v;
        }

        public override Task<NodeExpr> Visit(NodeExprBinary e, CancellationToken t)
        {
            return Task.Run
           (
               () =>
               {
                   var lt = Visit(e.Left, t);
                   var rt = Visit(e.Right, t);

                   Task.WaitAll(new[] { lt, rt }, t);

                   t.ThrowIfCancellationRequested();

                   var l = lt.Result;
                   var r = rt.Result;

                   return
                    ((l is NodeExprNumber<N> ln) && (r is NodeExprNumber<N> rn))
                    ? (NodeExpr)NodeExpr.Number(mAlg.EvalBinaryOperator(e.TypeBinary, ln.Value, rn.Value))
                    : NodeExpr.Binary(e.TypeBinary, l, r);
               }
           );
        }

        public static async Task<NodeExpr> Eval(NodeExpr e, IAlgebra<N> a, CancellationToken t, IVars v = null)
        {
            var vv = (v != null) ? a.Session.Vars.CreateAmbito(v) : a.Session.Vars;
            var visit = new EvaluateVisitor<N>(a, vv);
            var r = await visit.Visit(e, t);

            t.ThrowIfCancellationRequested();

            return r;
        }
    }
}
