using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Core.Math.AlgExprs
{
    public class CalcResult
    {
        public Expr Result { get; internal set; }
        public Explain.CalcExplain Explain { get; internal set; }
    }
}
