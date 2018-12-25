using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestExpressions
{
    class Algoritmos<T>
    {
        public static bool IsNumberPrime(Expression<Func<T>> e)
        {
            return false;
        }
    }
    class UserExpression : Algoritmos<int>
    {
        public Expression RunExpression()
        {
            var ispn = IsNumberPrime(() => 10);

            return Expression.Constant(ispn);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var e = new UserExpression().RunExpression();

            Console.WriteLine(e);
        }
    }
}
