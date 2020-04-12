using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Math.Expr
{
    public sealed class NullExpr : Expr
    {
        private static readonly Lazy<NullExpr> mInstance = new Lazy<NullExpr>(() => new NullExpr());

        private NullExpr() : base(ETypeExpr.Null) { }

        public NullExpr Instance => mInstance.Value;
    }
}
