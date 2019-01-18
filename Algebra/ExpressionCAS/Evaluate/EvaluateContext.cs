#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
