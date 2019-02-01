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

using Algebra.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public class NodeExprStringBuilderVisitor : NodeExprVisitorASync<string>
    {
        public static Task<string> ToStringAsync(NodeExpr e, CancellationToken t) => new NodeExprStringBuilderVisitor().Visit(e, t);
        public static string ToString(NodeExpr e) => new NodeExprStringBuilderVisitor().Visit(e, new CancellationTokenSource().Token).WaitAndResult();

        public override Task<string> Visit(NodeExprCte e, CancellationToken t) => Task.Run(() => (e?.Value.ToString()) ?? "null");

        public override Task<string> Visit(NodeBinaryExpr e, CancellationToken t)
        {
            return Task.Run
            (
                () =>
                {
                    var lt = Visit(e.Left, t);
                    var rt = Visit(e.Right, t);

                    Task.WaitAll(lt, rt);

                    t.ThrowIfCancellationRequested();

                    var l = lt.Result;
                    var r = rt.Result;
                    var lp = e.Left.Priority > e.Priority;
                    var rp = e.Priority < e.Right.Priority;

                    if (e.IsNecesaryParenthesisLeft)
                        l = $"({l})";
                    if (e.IsNecesaryParenthesisRight)
                        r = $"({r})";

                    return (e.TypeBinary == ETypeBinary.Mult && !(e.Left.TypeExpr == ENodeTypeExpr.Constant && e.Right.TypeExpr == ENodeTypeExpr.Constant)) ? l + r : l + MathExpr.TypeBinariesStr[e.TypeBinary] + r;
                }
            );
        }

        public async override Task<string> Visit(NodeExprInstruction e, CancellationToken t)
        {
            return (await base.Visit(e, t)) + " " + ((e.IsShowResult) ? ";" : "$");
        }
    }
}
