using Algebra.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private IProgress<PadProgress> mProgress;
        private CancellationToken mCancelToken;
        private Type mTypePrecision;
        private PadContext mContext;
        private PadProgress mPadProgress;
        private IndentedTextWriter mWriter;
        private int mLineAct;

        public GenerateClassUserExpression(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Type argTypePrecision, PadContext argContext)
        {
            mProgress = argProgress;
            mCancelToken = argCancelToken;
            mTypePrecision = argTypePrecision;
            mContext = argContext;
            mPadProgress = new PadProgress { Name = Properties.Resources.Evaluating, Maximum = 2 };
        }

        public async Task Generate(TextWriter argWriter, IList<string> argExpr)
        {
            ReportProgress();
            using (mWriter = new IndentedTextWriter(argWriter))
            {
                mLineAct = 1;
                IncProgress();
                await WriteUsings();
                await WriteLine($"class {Guid.NewGuid().ToClassName()} : UserExpresion<{mTypePrecision.FullName}>");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteExpr(argExpr);
                //await WriteLines(new[{]);
                mWriter.Indent--;
                await WriteLine("}");
            }
        }

        private void ReportProgress()
        {
            mProgress.Report(mPadProgress);
        }

        private void IncProgress()
        {
            mPadProgress.Progress++;
            ReportProgress();
        }

        private async Task WriteLine(string argStr)
        {
            await mWriter.WriteLineAsync(argStr);
            mLineAct++;
        }

        private async Task WriteLines(ICollection<string> argStrs)
        {
            mPadProgress.Maximum += argStrs.Count;
            foreach (var s in argStrs)
            {
                mCancelToken.ThrowIfCancellationRequested();
                await WriteLine(s);
                IncProgress();
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
