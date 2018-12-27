using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
            var exprs = new[]
            {
                "()=>10",
                "x=>20",
                "a=()=>20",
                "a=()=>20; b=()=>IsNumberPrimer(()=>10);"
            };

            foreach (var e in exprs)
                evaluate(e);

            //var node1 = SyntaxFactory.ParseExpression("a=10");
            //var node2 = SyntaxFactory.ParseExpression("10");
            //var node3 = SyntaxFactory.ParseSyntaxTree("a=10;b=20;").GetRoot();
            //var node4 = SyntaxFactory.ParseSyntaxTree("a=10");
            //var node5 = SyntaxFactory.ParseSyntaxTree("10");
            //var e = new UserExpression();

            //e.Evaluate();

            //Console.WriteLine(e.Expr1);
        }
        static void evaluate(string e)
        {
            var n = SyntaxFactory.ParseExpression(e);

            if (!n.ContainsDiagnostics && n.FullSpan.Length == e.Length)
            {
                evaluate(n);

                return;
            }

            evaluate(SyntaxFactory.ParseSyntaxTree(e).GetRoot());
        }

        static void evaluate(SyntaxNode n)
        {
            if (n is AssignmentExpressionSyntax)
            {

            }
            else if (n is CompilationUnitSyntax)
            {
                evaluate(((CompilationUnitSyntax)n).EndOfFileToken.SyntaxTree.GetRoot());
            }
            else if (n is ExpressionSyntax)
            {
            }
        }
    }
}
