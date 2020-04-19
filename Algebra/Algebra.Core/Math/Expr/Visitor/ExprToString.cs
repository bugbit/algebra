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
        //public override string Visit(UnaryExpr e)
        //{
        //    switch(e.TypeExpr)
        //    {
        //        case ETypeExpr.Negate:
        //            return $"-";
        //    }
        //}
    }
}
