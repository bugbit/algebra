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

            //pPad.ExpressionString = "2+2";
            //pPad.ExpressionString = "IsPrime(2+2)";
            //pPad.ExpressionString = "IsPrime((BigInteger) 3)";
            pPad.ExpressionString = "IsPrime(3L)";
            //pPad.ExpressionString = "Factorial(100)";

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            pFrm.Evaluate();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Application.Run(pFrm);
        }
    }
}
