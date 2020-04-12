using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Math.Expr
{
    public class FunctionExpr : Expr
    {
        public EFunctions TypeFunction { get; }
        public Expr Argument { get; }

        public FunctionExpr(EFunctions argFunc, Expr argArg) : base(ETypeExpr.Function)
        {
            TypeFunction = argFunc;
            Argument = argArg ?? Expr.Null;
        }

        public override bool IsConstant => Argument.IsConstant;
    }
}
