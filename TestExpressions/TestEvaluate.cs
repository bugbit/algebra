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

            pPad.ExpressionString = "2+2";

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            pFrm.Evaluate();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Application.Run(pFrm);
        }
    }
}
