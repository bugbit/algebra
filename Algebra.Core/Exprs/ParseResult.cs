using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Exprs
{
    public class ParseResult
    {
        public NodeExpr[] Exprs { get; set; }
        public bool Finished { get; set; }
    }
}
