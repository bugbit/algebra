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

        public EvaluateVisitor(IAlgebra<N> a)
        {
            mAlg = a;
        }

        public override Task<NodeExpr> Visit(NodeExprBinary e, CancellationToken t)
        {
            return Task.Run
           (
               () =>
               {
                   var lt = Visit(e.Left, t);
                   var rt = Visit(e.Right, t);

                   Task.WaitAll(lt, rt);

                   var l = lt.Result;
                   var r = rt.Result;

                   return
                    ((l is NodeExprNumber<N> ln) && (r is NodeExprNumber<N> rn))
                    ? (NodeExpr)NodeExpr.Number(mAlg.EvalBinaryOperator(e.TypeBinary, ln.Value, rn.Value))
                    : NodeExpr.Binary(e.TypeBinary, l, r);
               }
           );
        }
    }
}
