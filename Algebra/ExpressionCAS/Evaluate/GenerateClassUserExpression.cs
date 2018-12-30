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
        private static readonly string[] mUsing = new[] { "System.Linq.Expressions", "CAS = Algebra.ExpressionCAS" };
        private static readonly string[] mExprPart1 = new[]
        {
            "public Expression<Func<object>> Expr",
            "{"
        };
        private static readonly string[] mExprPart2 = new[]
        {
            "get",
            "{"
        };
        private static readonly string[] mExprPart3 = new[]
        {
            "return"
        };

        private EvaluateContext mContext;
        private IndentedTextWriter mWriter;
        private int mLineAct;

        public GenerateClassUserExpression(EvaluateContext argContext)
        {
            mContext = argContext;
        }

        public List<string> Assembles => new List<string>(new[] { "System.dll", "System.Core.dll", Assembly.GetExecutingAssembly().Location });

        public async Task Generate(TextWriter argWriter, IList<string> argExpr)
        {
            mContext.PadProgress.Maximum += 2;
            mContext.ReportProgress();
            using (mWriter = new IndentedTextWriter(argWriter))
            {
                mLineAct = 1;
                mContext.IncProgress();
                await WriteUsings();
                await WriteLine($"class {Guid.NewGuid().ToClassName()} : UserExpresion<{mContext.TypePrecision.FullName}>");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteExpr(argExpr);
                //await WriteLines(new[{]);
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
            await WriteLines(mExprPart1);
            mWriter.Indent++;
            await WriteLines(mExprPart2);
            mWriter.Indent++;
            await WriteLines(mExprPart3);
            mWriter.Indent++;
            await WriteLines(argExpr);
            await WriteLine(";");
            mWriter.Indent--;
            mWriter.Indent--;
            await WriteLine("}");
            mWriter.Indent--;
            await WriteLine("}");
        }
    }
}
