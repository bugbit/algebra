using Algebra.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS.Evaluate
{
    class GenerateClassUserExpression
    {
        private static readonly string[] mUsing = new[] { "System.Linq.Expressions", "CAS = Algebra.ExpressionCAS", "static System.Math" };

        private EvaluateContext mContext;
        private IndentedTextWriter mWriter;
        private int mLineAct;

        public GenerateClassUserExpression(EvaluateContext argContext)
        {
            mContext = argContext;
        }

        public List<string> Assembles => new List<string>(new[] { "System.dll", "System.Core.dll", Assembly.GetExecutingAssembly().Location });

        public int LineExprStart { get; private set; }
        public int LineExprEnd { get; private set; }

        public async Task Generate(TextWriter argWriter, IList<string> argExpr)
        {
            using (mWriter = new IndentedTextWriter(argWriter))
            {
                var pTypeUserExpression = typeof(UserExpression<>);

                mLineAct = 1;
                await WriteUsings();
                await WriteLine("");
                await WriteLine($"namespace {pTypeUserExpression.Namespace}");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteLine($"class UserExpression{Guid.NewGuid().ToClassName()} : UserExpression<{mContext.PrecisionInfo.TypePrecision.FullName}>");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteExpr(argExpr);
                //await WriteLines(new[{]);
                mWriter.Indent--;
                await WriteLine("}");
                mWriter.Indent--;
                await WriteLine("}");
            }
        }

        private async Task WriteLine(string argStr)
        {
            await mWriter.WriteLineAsync(argStr);
            mLineAct++;
        }

        private async Task WriteLines(ICollection<string> argStrs)
        {
            mContext.PadProgress.Maximum += argStrs.Count;
            foreach (var s in argStrs)
            {
                mContext.CancelToken.ThrowIfCancellationRequested();
                await WriteLine(s);
                mContext.IncProgress();
            }
        }

        private async Task WriteUsings()
        {
            var pUsings = mUsing;

            await WriteLines((from u in pUsings select $"using {u};").ToArray());
        }

        private async Task WriteExpr(IList<string> argExpr)
        {
            await WriteLine(@"public override Expression<Func<object>> Expr => ()=>");
            mWriter.Indent++;
            LineExprStart = mLineAct;
            await WriteLines(argExpr);
            LineExprEnd = mLineAct;
            await WriteLine(";");
            mWriter.Indent--;
        }
    }
}
