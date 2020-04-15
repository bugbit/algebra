using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Core.Math.Expr
{
    public class OperatorExprWriterInfo
    {
        public class ExprsInfo
        {
            public bool PutParens { get; set; }
            public char? Symbol { get; set; }
            public Expr Expr { get; set; }
        }

        public EOperators Operator { get; set; }
        public ExprsInfo[] Exprs { get; set; }

        public static OperatorExprWriterInfo WriterInfo(OperatorExpr e)
        {
            var pExprs = new ExprsInfo[e.Exprs.Length];

            for (var i = 0; i < pExprs.Length; i++)
            {
                var e1 = e.Exprs[i];

                pExprs[i] = new ExprsInfo
                {
                    PutParens = Expr.PutParens(e, e1),
                    Symbol = e.Symbol(i),
                    Expr = e1,
                };
            }

            var pInfo = new OperatorExprWriterInfo
            {
                Operator = e.TypeOperator,
                Exprs = pExprs.ToArray()
            };

            return pInfo;
        }
    }
}
