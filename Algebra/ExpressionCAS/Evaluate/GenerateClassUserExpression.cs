using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS.Evaluate
{
    class GenerateClassUserExpression
    {
        private static readonly string[] mUsing = new[] { "CAS = Algebra.ExpressionCAS" };
        private Type mTypePrecision;
        private PadContext mContext;
        private IndentedTextWriter mWriter;
        private int mLineAct;

        public GenerateClassUserExpression(Type argTypePrecision, PadContext argContext)
        {
            mTypePrecision = argTypePrecision;
            mContext = argContext;
        }

        public void Generate()
        {
            using (var pWriterBase = new StringWriter())
            {
                using (mWriter = new IndentedTextWriter(pWriterBase))
                {
                    mLineAct = 1;
                    WriteUsings();
                    WriteLine($"class {new Guid().ToString()} : UserExpresion<{mTypePrecision.FullName}>");
                    WriteLine("{");
                    mWriter.Indent++;
                    mWriter.Indent--;
                    WriteLine("}");
                }
            }
        }

        private void WriteLine(string argStr)
        {
            mWriter.WriteLine(argStr);
            mLineAct++;
        }

        private void WriteLines(IEnumerable<string> argStrs)
        {
            foreach (var s in argStrs)
                WriteLine(s);
        }

        private void WriteUsings()
        {
            var pUsings = mUsing;

            WriteLines(from u in pUsings select $"using {u}");
        }
    }
}
