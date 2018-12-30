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
    public class UserExpression
    {
        public static bool DebugEvaluate = true;

        public async static Task<Evaluate.EvaluateResult> Evaluate(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Type argTypePrecision, PadContext argContext, IList<string> argExpr)
        {
            if (argExpr == null || argExpr.Count == 0 || argExpr.All(s => string.IsNullOrWhiteSpace(s)))
                return null;

            using (var pFilesTmp = new TempFileCollection(Path.GetTempPath()))
            {
                string pSourceCode = null;

                if (DebugEvaluate)
                    pSourceCode = pFilesTmp.AddExtension(".cs", true);

                using (var pWriterBase = (DebugEvaluate) ? (TextWriter)new StreamWriter(pSourceCode) : new StringWriter())
                {
                    var pGenerate = new Evaluate.GenerateClassUserExpression(argProgress, argCancelToken, argTypePrecision, argContext);

                    await pGenerate.Generate(pWriterBase, argExpr);

                    if (!DebugEvaluate)
                        pSourceCode = pWriterBase.ToString();
                }
            }

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
