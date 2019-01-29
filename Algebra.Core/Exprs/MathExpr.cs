using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Exprs
{
    public class MathExpr
    {
        public static readonly IDictionary<ETypeBinary, string> TypeBinariesStr = new Dictionary<ETypeBinary, string>
        {
            [ETypeBinary.Add] = "+",
            [ETypeBinary.Sub] = "-",
            [ETypeBinary.Mult] = "*",
            [ETypeBinary.Div] = "/",
            [ETypeBinary.Pow] = "^"
        };

        public const int PriorityHight = -int.MaxValue;
        public const int PriorityLow = +int.MaxValue;
        public const int PriorityFunction = -100;
        //public const int Priority

        // 0 => Hight, .... 32767 => Low
        public static readonly IDictionary<ETypeBinary, int> TypeBinariesPriorities = new Dictionary<ETypeBinary, int>
        {
            [ETypeBinary.Pow] = 0,
            [ETypeBinary.Mult] = 1,
            [ETypeBinary.Div] = 1,
            [ETypeBinary.Add] = 2,
            [ETypeBinary.Sub] = 2,
        };

        // 0 => Hight, .... 32767 => Low
        public static readonly IDictionary<ENodeTypeExpr, int> TypeNodesPriorities = new Dictionary<ENodeTypeExpr, int>
        {
            [ENodeTypeExpr.Constant] = 100,
            [ENodeTypeExpr.BinaryExpr] = 0
        };
    }
}
