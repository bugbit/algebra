using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Exprs
{
    public class NodeExprStringBuilderVisitor : NodeExprVisitor<string>
    {
        public static string ToString(NodeExpr e) => new NodeExprStringBuilderVisitor().Visit(e);

        public override string Visit(NodeExprCte e) => (e?.Value.ToString()) ?? "null";
    }
}
