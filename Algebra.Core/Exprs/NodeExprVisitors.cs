using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Exprs
{
    public interface INodeExprVisitor<T>
    {
        T Visit(NodeExpr e);
        T Visit(NodeExprCte e);
        T Visit(NodeBinaryExpr e);
    }

    public class NodeExprVisitor<T> : INodeExprVisitor<T>
    {
        public virtual T Visit(NodeExpr e) => e.Accept(this);

        public virtual T Visit(NodeExprCte e) => default(T);

        public virtual T Visit(NodeBinaryExpr e) => default(T);
    }
}

