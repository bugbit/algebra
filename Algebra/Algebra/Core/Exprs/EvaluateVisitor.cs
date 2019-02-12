using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public class EvaluateVisitor<N> : NodeExprVisitorRetExprASync<N>
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
