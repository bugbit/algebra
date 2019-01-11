using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS.Evaluate
{
    public class EvaluateResult
    {
        public bool Success { get; set; }
        public string MsgError { get; set; }
        public Expression<Func<object>> Expr { get; set; }
        public object Value { get; set; }
        public string[] NewVars { get; set; }
    }
}
