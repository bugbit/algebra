using System;
using System.Collections.Generic;
using System.Text;
using ST = Algebra.Core.Syntax;

namespace Algebra.Core.Math.Expr.Visitor
{
    public class ExprToString : ExprVisitorRetExpr<string>
    {
        private static readonly Lazy<ExprToString> mInstance = new Lazy<ExprToString>(() => new ExprToString());

        private ExprToString() { }

        public static ExprToString Instance => mInstance.Value;

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
            var l = e.Left;
            var r = e.Right;
            var pStr = VisitBinary(e, l, r);

            return pStr;
        }

        public override string Visit(FunctionExpr e)
        {
            var pStr = ST.Symbols.DictFuncsStr[e.TypeExpr] + ParenthesizedVisit(true, e.Argument);

            return pStr;
        }

        private string OpToCar(Expr e) => (ST.Symbols.DictOperatorsStr.TryGetValue(e.TypeExpr, out char car)) ? car.ToString() : string.Empty;

        private string VisitBinary(Expr e, Expr l, Expr r)
        {
            var pFlag = e.NeedsParentheses(l);
            var pFlag2 = e.NeedsParentheses(r);
            var pStr = string.Empty;

            pStr += ParenthesizedVisit(pFlag, l);
            if (e.TypeExpr != ETypeExpr.Multiply || ((l is NumberExpr) && (r is NumberExpr)))
                pStr += OpToCar(e);
            pStr += ParenthesizedVisit(pFlag2, r);

            return pStr;
        }

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
