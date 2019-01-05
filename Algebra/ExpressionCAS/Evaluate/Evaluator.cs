using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Evaluator(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Precisions.Info argPrecisionInfo, PadContext argContext)
        {
            mContext = new EvaluateContext(argProgress, argCancelToken, argPrecisionInfo, argContext);
        }

        public async Task<Evaluate.EvaluateResult> Evaluate(IList<string> argExpr)
        {
            try
            {
                var pStopWatch = new Stopwatch();

                pStopWatch.Start();
                if (argExpr == null || argExpr.Count == 0 || argExpr.All(s => string.IsNullOrWhiteSpace(s)))
                {
                    mContext.PadProgress.Name = null;
                    mContext.ReportProgress();

                    return null;
                }

                using (var pFilesTmp = new TempFileCollection(Path.GetTempPath()))
                {
                    if (DebugEvaluate)
                        mSourceCode = pFilesTmp.AddExtension(".cs", true);

                    var pGenerate = new Evaluate.GenerateClassUserExpression(mContext);

                    using (var pWriterBase = (DebugEvaluate) ? (TextWriter)new StreamWriter(mSourceCode) : new StringWriter())
                    {
                        mContext.PadProgress.Maximum += 2;
                        mContext.ReportProgress();
                        await pGenerate.Generate(pWriterBase, argExpr);
                        mContext.IncProgress();

                        if (!DebugEvaluate)
                            mSourceCode = pWriterBase.ToString();
                    }
                    var cp = new CompilerParameters(pGenerate.Assembles.ToArray())
                    {
                        GenerateInMemory = !DebugEvaluate
                    };

                    if (DebugEvaluate)
                    {
                        cp.GenerateExecutable = true;
                        cp.IncludeDebugInformation = true;
                        cp.OutputAssembly = Path.ChangeExtension(mSourceCode, ".dll");
                    }

                    var pProvider = Common.HelperCodeCom.CodeProvider.Value;
                    var cr = await Task.Run(() => (DebugEvaluate) ? pProvider.CompileAssemblyFromFile(cp, mSourceCode) : pProvider.CompileAssemblyFromSource(cp, mSourceCode));

                    mContext.IncProgress();

                    if (cr.Errors.Count > 0)
                    {
                        mContext.PadProgress.Name = Properties.Resources.ErrorEvaluate;
                        mContext.ReportProgress();
                        if (DebugEvaluate)
                            System.Diagnostics.Process.Start(mSourceCode);
                    }
                    else
                    {
                        pStopWatch.Stop();
                        mContext.PadProgress.Name = string.Format(Properties.Resources.EvaluateSuccess, pStopWatch.Elapsed.Seconds);
                        mContext.ReportProgress();
                    }
                }

                return null;
            }
            finally
            {
                mContext.PadProgress.Visible = false;
                mContext.ReportProgress();
            }
        }
    }
}
