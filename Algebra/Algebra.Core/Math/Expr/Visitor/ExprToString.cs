using System;
using System.Collections.Generic;
using System.Text;
using ST = Algebra.Core.Syntax;

namespace Algebra.Core.Math.Expr.Visitor
{
    public class ExprToString : ExprVisitorRetExpr<string>
    {
        public override string Visit(NullExpr e) => "null";

        public override string Visit(NumberExpr e) => e.Value.ToString();
        public override string Visit(LiteralExpr e) => e.Name;
        public override string Visit(UnaryExpr e)
        {
            var pStr = OpToCar(e) + ParenthesizedVisit(e, e.Operand);

            return pStr;
        }

        public override string Visit(BinaryExpr e)
        {
            var pFlag = e.NeedsParentheses(e.Left);
            var pFlag2 = e.NeedsParentheses(e.Right);
            var pStr = string.Empty;

            pStr += ParenthesizedVisit(pFlag, e.Left);
            if (e.TypeExpr != ETypeExpr.Multiply || ((e.Left is NumberExpr) && (e.Right is NumberExpr)))
                pStr += OpToCar(e);
            pStr += ParenthesizedVisit(pFlag2, e.Right);

            return pStr;
        }

        public override string Visit(FunctionExpr e)
        {
            var pStr = ST.Symbols.DictFuncsStr[e.TypeExpr] + ParenthesizedVisit(true, e.Argument);

            return pStr;
        }

        private string OpToCar(Expr e) => (ST.Symbols.DictOperatorsStr.TryGetValue(e.TypeExpr, out char car)) ? car.ToString() : string.Empty;

        private string ParenthesizedVisit(Expr parent, Expr nodeToVisit) => ParenthesizedVisit(parent.NeedsParentheses(nodeToVisit), nodeToVisit);

        private string ParenthesizedVisit(bool argHasParen, Expr e)
        {
            var pStr = string.Empty;

            if (argHasParen)
                pStr += ST.Symbols.OpenParensChar;
            pStr += Visit(e);
            if (argHasParen)
                pStr += ST.Symbols.CloseParensChars;

            return pStr;
        }
    }
}
