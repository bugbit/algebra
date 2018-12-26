using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestExpressions
{
    class ExpressionCAS<T>
    {
        public static Expression IsNumberPrime(Expression<Func<T>> e)
        {
            return Expression.Constant(false);
        }
    }
    class UserExpression : ExpressionCAS<int>
    {
        public Expression Expr1 { get; private set; }

        public void Evaluate()
        {
            Expr1 = IsNumberPrime(() => 10);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var e = new UserExpression();

            e.Evaluate();

            Console.WriteLine(e.Expr1);
        }
    }
}
