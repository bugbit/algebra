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
        public EOperators TypeOperator { get; }
        public Expr[] Exprs { get; }

        /// <summary>
        /// case 1 = (op e1 e2)
        /// 2 * 3 = (* 2 3) case 1
        /// (2 + 3) * 5 = (* (+ 2 3) 5) case 1
        /// (2 * 3) * 5 = (* 2 3 5) case 2
        /// 2 * (3 + 4) = (* 2 (+ 3 4)) case 1
        /// 2 * (3 * 4) = (* 2 3 4) case 2
        /// (2 + 3) * (4 + 5) (* (+ 2 3) (+ 4 5) case 1
        /// (2 * 3) * (4 * 5) (* 2 3 4 5)
        /// </summary>
        /// <param name="argOperator"></param>
        /// <param name="argExprs"></param>
        public OperatorExpr(EOperators argOperator, IEnumerable<Expr> argExprs) : base(ETypeExpr.Operator)
        {
            TypeOperator = argOperator;
            Exprs =
                (argExprs == null)
                    ? new Expr[0]
                    : (from e in argExprs where e != null let op = e as OperatorExpr select (op == null) ? new[] { e } : (IEnumerable<Expr>)op.Exprs).SelectMany(e => e).ToArray();
        }

        public OperatorExpr(EOperators argOperator, params Expr[] argExprs) : this(argOperator, argExprs.AsEnumerable()) { }

        public override bool IsConstant => Exprs.Length > 0 && Exprs.All(e => e.IsConstant);

        public static int GetPriority(EOperators op)
        {
            switch (op)
            {
                case EOperators.Equal:
                    return 5;
                case EOperators.Pow:
                    return 4;
                case EOperators.Mul:
                case EOperators.Div:
                    return 3;
                case EOperators.Add:
                case EOperators.Minus:
                    return 1;
                default:
                    return 0;
            }
        }

        public int GetPriority() => GetPriority(TypeOperator);
    }
}
