using Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExpressions
{
    class TestEvaluate
    {
        [STAThread]
        static void Main()
        {
            var pFrm = new FrmAlgebraPad();
            var pPad = pFrm.NewPad();

            pPad.ExpressionString = "()=>2+2";

            pFrm.Evaluate();
            Application.Run(pFrm);
        }
    }
}
