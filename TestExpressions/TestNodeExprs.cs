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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algebra.Core.Exprs;

namespace TestExpressions
{
    class TestNodeExprs
    {
        static void Main(string[] args)
        {
            //var n1 = (int)decimal.Parse("1.1", NumberStyles.Float, NumberFormatInfo.InvariantInfo);
            //var n2 = double.Parse("1.1", NumberFormatInfo.InvariantInfo);
            var a1 = Algebra.Core.Algebra.Default;
            var n1 = a1.ParseNumber("1.1");

            var e1 = new NodeExprCte(100, ETypeConstant.Number);
            var e2 = new NodeExprCte(200, ETypeConstant.Number);
            var e3 = new NodeBinaryExpr(ETypeBinary.Add, e1, e2);

            var e11 = new NodeExprCte(10, ETypeConstant.Number);
            var e21 = new NodeExprCte(20, ETypeConstant.Number);
            var e31 = new NodeBinaryExpr(ETypeBinary.Add, e11, e21);

            // (100+200)*(10+20)
            var e4 = new NodeBinaryExpr(ETypeBinary.Mult, e3, e31);
            // (100+200)*(10+20)+(100+200)*(10+20)
            var e5 = new NodeBinaryExpr(ETypeBinary.Add, e4, e4);

            Console.WriteLine(e5);
            Console.ReadLine();
        }
    }
}
