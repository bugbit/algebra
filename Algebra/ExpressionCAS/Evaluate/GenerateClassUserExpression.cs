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

using Algebra.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS.Evaluate
{
    class GenerateClassUserExpression
    {
        private static readonly string[] mUsing = new[] { "System", "System.Numerics", "System.Linq.Expressions", $"static {typeof(ExpressionCAS).FullName}", $"static {typeof(Math.MathEx).FullName}" };

        private EvaluateContext mContext;
        private IndentedTextWriter mWriter;
        private int mLineAct;

        public GenerateClassUserExpression(EvaluateContext argContext)
        {
            mContext = argContext;
        }

        public string NameSpace { get; private set; }
        public string ClassName { get; private set; }
        public string FullClassName => NameSpace + "." + ClassName;
        public List<string> Assembles => new List<string>(new[] { "System.dll", "System.Core.dll", "System.Numerics.dll", Assembly.GetExecutingAssembly().Location });

        public int LineExprStart { get; private set; }
        public int LineExprEnd { get; private set; }

        public async Task Generate(TextWriter argWriter, IList<string> argExpr)
        {
            using (mWriter = new IndentedTextWriter(argWriter))
            {
                var pTypeUserExpression = typeof(UserExpression<>);

                NameSpace = pTypeUserExpression.Namespace;
                ClassName = $"UserExpression{Guid.NewGuid().ToClassName()}";
                mLineAct = 1;
                await WriteUsings();
                await WriteLine("");
                await WriteLine($"namespace {NameSpace}");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteLine($"class {ClassName} : UserExpression<{mContext.PrecisionInfo.TypePrecision.FullName}>");
                await WriteLine("{");
                mWriter.Indent++;
                await WriteLine($"public {ClassName}(PadContext argContext) : base(argContext) {{ }}");
                await WriteLine("");
                await WriteExpr(argExpr);
                //await WriteLines(new[{]);
                mWriter.Indent--;
                await WriteLine("}");
                mWriter.Indent--;
                await WriteLine("}");
            }
        }

        private async Task WriteLine(string argStr)
        {
            mContext.CancelToken.ThrowIfCancellationRequested();
            await mWriter.WriteLineAsync(argStr);
            mLineAct++;
        }

        private async Task WriteLines(ICollection<string> argStrs)
        {
            mContext.PadProgress.Maximum += argStrs.Count;
            foreach (var s in argStrs)
            {
                mContext.CancelToken.ThrowIfCancellationRequested();
                await WriteLine(s);
                mContext.IncProgress();
            }
        }

        private async Task WriteUsings()
        {
            var pUsings = mUsing;

            await WriteLines((from u in pUsings select $"using {u};").ToArray());
        }

        private async Task WriteExpr(IList<string> argExpr)
        {
            await WriteLine(@"public override Expression<Func<object>> Expr => ()=>");
            mWriter.Indent++;
            LineExprStart = mLineAct;
            await WriteLines(argExpr);
            LineExprEnd = mLineAct;
            await WriteLine(";");
            mWriter.Indent--;
        }
    }
}
