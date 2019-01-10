using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS.Evaluate
{
    public class EvaluateContext
    {
        private IProgress<PadProgress> mProgress;

        public EvaluateContext(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Precisions.Info argPrecisionInfo, PadContext argContext)
        {
            mProgress = argProgress;
            CancelToken = argCancelToken;
            PrecisionInfo = argPrecisionInfo;
            Context = argContext;
            PadProgress = new PadProgress { Name = Properties.Resources.Evaluating, Visible = true };
        }

        public Precisions.Info PrecisionInfo { get; }
        public CancellationToken CancelToken { get; }
        public PadContext Context { get; }

        public PadProgress PadProgress { get; }

        public void ReportProgress()
        {
            mProgress.Report(PadProgress);
        }

        public void IncProgress()
        {
            PadProgress.Progress++;
            ReportProgress();
        }
    }
}
