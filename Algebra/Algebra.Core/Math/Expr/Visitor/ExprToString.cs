using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Math.Expr.Visitor
{
    public class ExprToString : ExprVisitorRetExpr<string>
    {
        public override string Visit(NullExpr e) => "null";

        public override string Visit(NumberExpr e) => e.Value.ToString();
        public override string Visit(LiteralExpr e) => e.Name;
        public override string Visit(UnaryExpr e)
        {
            var pParens = e.NeedsParentheses(e.Operand);
            var pStr = string.Empty;

            switch (e.TypeExpr)
            {
                case ETypeExpr.Negate:
                    pStr += $"-";
                    break;
            }
            pStr += ParenthesizedVisit(e, e.Operand);

            return pStr;
        }

        private string ParenthesizedVisit(Expr parent, Expr nodeToVisit)
        {
            var pStr = Visit(nodeToVisit);

            return (parent.NeedsParentheses(nodeToVisit)) ? $"({pStr})" : pStr;
        }
    }
}
