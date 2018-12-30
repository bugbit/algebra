//#define ROSLYN
// <package id="Microsoft.CodeAnalysis.CSharp" version="1.3.2" targetFramework="net45" />

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
#if ROSLYN
        public Expression Expr1 { get; private set; }

        public void Evaluate()
        {
            Expr1 = IsNumberPrime(() => 10);
        }
#endif

        public Expression<Func<object>> Expr
        {
            get
            {
                return () => IsNumberPrime(() => 10);
            }
        }

        public Expression<Func<object>> Expr1
        {
            get
            {
                return
                    () => new { a = 20, b = IsNumberPrime(() => 10) }
                    ;
            }
        }

        public object Evaluate1()
        {
            return IsNumberPrime(() => 10);
        }

        public object Evaluate2()
        {
            return new { a = 20, b = IsNumberPrime(() => 10) };
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
#if ROSLYN
            var exprs = new[]
            {
                "()=>10",
                "x=>20",
                "a=()=>20",
                "{ a=()=>20; b=()=>IsNumberPrimer(()=>10); }",
                "a=()=>20; b=()=>IsNumberPrimer(()=>10);"
            };

            foreach (var e in exprs)
                evaluate(e);

            //var node1 = SyntaxFactory.ParseExpression("a=10");
            //var node2 = SyntaxFactory.ParseExpression("10");
            //var node3 = SyntaxFactory.ParseSyntaxTree("a=10;b=20;").GetRoot();
            //var node4 = SyntaxFactory.ParseSyntaxTree("a=10");
            //var node5 = SyntaxFactory.ParseSyntaxTree("10");
#endif

            var expr = new UserExpression();

#if ROSLYN
            expr.Evaluate();

            Console.WriteLine(expr.Expr1);
#endif
            Console.WriteLine(expr.Evaluate1());
            Console.WriteLine(expr.Evaluate2());
            Console.WriteLine(expr.Expr);
            Console.WriteLine(expr.Expr.Compile().Invoke());
            Console.WriteLine(expr.Expr1);
            Console.WriteLine(expr.Expr1.Compile().Invoke());
        }

#if ROSLYN
        static void evaluate(string e)
        {
            var n = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression(e);

            if (!n.ContainsDiagnostics && n.FullSpan.Length == e.Length)
            {
                evaluate(n);

                return;
            }

            evaluate(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseSyntaxTree(e).GetRoot());
        }

        static void evaluate(Microsoft.CodeAnalysis.SyntaxNode n)
        {
            if (n is Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax)
            {

            }
            else if (n is Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax)
            {
                evaluate(((Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax)n).SyntaxTree.GetRoot());
            }
            else if (n is Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)
            {
            }
        }
#endif
    }
}
