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
using System.Text;

namespace Algebra.Core.Exprs
{
    public class NodeExprStringBuilderVisitor : NodeExprVisitor<string>
    {
        public static string ToString(NodeExpr e) => new NodeExprStringBuilderVisitor().Visit(e);

        public override string Visit(NodeExprCte e) => (e?.Value.ToString()) ?? "null";

        public override string Visit(NodeBinaryExpr e)
        {
            var l = e.Left.Accept(this);
            var r = e.Right.Accept(this);
            var lp = e.Left.Priority > e.Priority;
            var rp = e.Priority < e.Right.Priority;

            if (lp)
                l = $"({l})";
            if (rp)
                r = $"({r})";

            return (e.TypeBinary == ETypeBinary.Mult && !(e.Left.TypeExpr == ENodeTypeExpr.Constant && e.Right.TypeExpr == ENodeTypeExpr.Constant)) ? l + r : l + MathExpr.TypeBinariesStr[e.TypeBinary] + r;
        }
    }
}
