using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Exprs;

namespace Algebra.Core
{
    public partial interface IAlgebra
    {
        Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null);
    }

    public partial class Algebra
    {
        public abstract Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null);
    }

    public partial class Algebra<T>
    {
        public override Task<NodeExpr> Eval(NodeExpr e, CancellationToken t, IVars v = null) => EvaluateVisitor<T>.Eval(e, this, t, v);
    }
}
