using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Math.Expr
{
    public class OperatorExpr : Expr
    {
        new public EOperators Operator { get; }
        public Expr[] Exprs { get; }

        public OperatorExpr(EOperators argOperator, IEnumerable<Expr> argExprs) : base(ETypeExpr.Operator)
        {
            Operator = argOperator;
            Exprs = argExprs?.ToArray() ?? new Expr[0];
        }

        public override bool IsConstant => Exprs.Length > 0 && Exprs.All(e => e.IsConstant);

        public Expr[] FixExprs(EOperators op, Expr e, CancellationToken token)
        {
            if (e is OperatorExpr pOp && pOp.Operator == op)
            {
                var pExprs = new List<Expr[]>(pOp.Exprs.Length);

                Parallel.For(0, pExprs.Count, new ParallelOptions { CancellationToken = token }, i => pExprs[i] = FixExprs(op, pOp.Exprs[i], token));

                return pExprs.SelectMany(e => e).ToArray();
            }

            return new[] { e };
        }

        public OperatorExpr FixExprs(CancellationToken token)
        {
            var pExprs = new List<Expr[]>(Exprs.Length);

            Parallel.For(0, pExprs.Count, new ParallelOptions { CancellationToken = token }, i => pExprs[i] = FixExprs(Operator, Exprs[i], token));

            return new OperatorExpr(Operator, pExprs.SelectMany(e => e));
        }
    }
}
