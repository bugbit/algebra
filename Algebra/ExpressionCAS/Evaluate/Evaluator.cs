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
    public class Evaluator
    {
        private EvaluateContext mContext;
        private string mSourceCode;

        public static bool DebugEvaluate = true;

        public Evaluator(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Type argTypePrecision, PadContext argContext)
        {
            mContext = new EvaluateContext(argProgress, argCancelToken, argTypePrecision, argContext);
        }

        public async Task<Evaluate.EvaluateResult> Evaluate(IList<string> argExpr)
        {
            if (argExpr == null || argExpr.Count == 0 || argExpr.All(s => string.IsNullOrWhiteSpace(s)))
                return null;

            using (var pFilesTmp = new TempFileCollection(Path.GetTempPath()))
            {
                if (DebugEvaluate)
                    mSourceCode = pFilesTmp.AddExtension(".cs", true);

                using (var pWriterBase = (DebugEvaluate) ? (TextWriter)new StreamWriter(mSourceCode) : new StringWriter())
                {
                    var pGenerate = new Evaluate.GenerateClassUserExpression(mContext);

                    await pGenerate.Generate(pWriterBase, argExpr);

                    if (!DebugEvaluate)
                        mSourceCode = pWriterBase.ToString();
                }
            }

            return null;
        }
    }
}
