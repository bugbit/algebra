using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public class UserExpression
    {
        public static IUserExpression Evaluate(Type argTypePrecision, PadContext argContext, string argExpr)
        {
            if (string.IsNullOrWhiteSpace(argExpr))
                return null;

            var pGenerate = new Evaluate.GenerateClassUserExpression(argTypePrecision, argContext);

            return null;
        }
    }

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
