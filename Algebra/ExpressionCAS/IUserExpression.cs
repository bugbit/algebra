using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public interface IUserExpression
    {
        Expression<Func<object>> Expr { get; }
    }
}
