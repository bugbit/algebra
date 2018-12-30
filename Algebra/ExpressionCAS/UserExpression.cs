using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public abstract class UserExpression<T> : ExpressionCAS<T>, IUserExpression
    {
        protected PadContext mContext;

        public UserExpression(PadContext argContext)
        {
            mContext = argContext;
        }

        public abstract Expression<Func<object>> Expr { get; }
    }
}
