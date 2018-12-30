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
        private PadContext mContext;

        public EvaluateContext(IProgress<PadProgress> argProgress, CancellationToken argCancelToken, Type argTypePrecision, PadContext argContext)
        {
            mProgress = argProgress;
            CancelToken = argCancelToken;
            TypePrecision = argTypePrecision;
            mContext = argContext;
            PadProgress = new PadProgress { Name = Properties.Resources.Evaluating };
        }

        public Type TypePrecision { get; }
        public CancellationToken CancelToken { get; }

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
