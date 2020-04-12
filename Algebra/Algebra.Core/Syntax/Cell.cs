using System;
using System.Collections.Generic;
using System.Text;
using Algebra.Core.Math.Expr;

namespace Algebra.Core.Syntax
{
    class Cell
    {
        public Expr Expr { get; set; }
        public EOperators TypeOp { get; set; }
    }
}
