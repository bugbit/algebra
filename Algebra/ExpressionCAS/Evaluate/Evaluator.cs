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

        public async Task<EvaluateResult> Evaluate(IList<string> argExpr)
        {
            try
            {
                var pStopWatch = new Stopwatch();
                var pResult = new EvaluateResult();

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

                    var pGenerate = new GenerateClassUserExpression(mContext);

                    using (var pWriterBase = (DebugEvaluate) ? (TextWriter)new StreamWriter(mSourceCode) : new StringWriter())
                    {
                        mContext.PadProgress.Maximum += 3;
                        mContext.ReportProgress();
                        await pGenerate.Generate(pWriterBase, argExpr);
                        mContext.CancelToken.ThrowIfCancellationRequested();
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
                        cp.IncludeDebugInformation = true;
                        cp.OutputAssembly = Path.ChangeExtension(mSourceCode, ".dll");
                    }
                    else
                        cp.CompilerOptions = "/optimize";

                    var pProvider = Common.HelperCodeCom.CodeProvider.Value;
                    var cr = await Task.Run(() => (DebugEvaluate) ? pProvider.CompileAssemblyFromFile(cp, mSourceCode) : pProvider.CompileAssemblyFromSource(cp, mSourceCode));

                    mContext.CancelToken.ThrowIfCancellationRequested();
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
                        await Evaluate(pGenerate, cr, pResult);
                        //Evaluate(cr,pr)
                        pStopWatch.Stop();
                        mContext.PadProgress.Name = string.Format(Properties.Resources.EvaluateSuccess, pStopWatch.Elapsed.Seconds);
                        mContext.PadProgress.Progress++;
                        mContext.ReportProgress();
                    }
                }

                return pResult;
            }
            finally
            {
                mContext.PadProgress.Visible = false;
                mContext.ReportProgress();
            }
        }

        private async Task Evaluate(GenerateClassUserExpression argGenerate, CompilerResults argResults, EvaluateResult argResult)
        {
            // Get expression y execute it
            //cr.CompiledAssembly
            var pContext = mContext.Context;
            var pType = argResults.CompiledAssembly.GetType(argGenerate.ClassName);
            var pConstr = pType.GetConstructor(new[] { typeof(PadContext) });
            var pObj = pConstr.Invoke(new object[] { pContext });
            var pUserExpr = (IUserExpression)pObj;
            var pExpr = pUserExpr.Expr;
            var pExprC = pExpr.Compile();
            var pValue = await Task.Factory.FromAsync(pExprC.BeginInvoke, pExprC.EndInvoke, null);

            pContext.Assemblies.Add(argResults.CompiledAssembly);
        }
    }
}
