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
    public class MathExpr
    {
        public static readonly IDictionary<ETypeBinary, string> TypeBinariesStr = new Dictionary<ETypeBinary, string>
        {
            [ETypeBinary.Add] = "+",
            [ETypeBinary.Sub] = "-",
            [ETypeBinary.Mult] = "*",
            [ETypeBinary.Div] = "/",
            [ETypeBinary.Pow] = "^"
        };

        public const int PriorityHight = -int.MaxValue;
        public const int PriorityLow = +int.MaxValue;
        public const int PriorityFunction = -100;
        //public const int Priority

        // 0 => Hight, .... 32767 => Low
        public static readonly IDictionary<ETypeBinary, int> TypeBinariesPriorities = new Dictionary<ETypeBinary, int>
        {
            [ETypeBinary.Pow] = 0,
            [ETypeBinary.Mult] = 1,
            [ETypeBinary.Div] = 1,
            [ETypeBinary.Add] = 2,
            [ETypeBinary.Sub] = 2,
        };

        // 0 => Hight, .... 32767 => Low
        public static readonly IDictionary<ENodeTypeExpr, int> TypeNodesPriorities = new Dictionary<ENodeTypeExpr, int>
        {
            [ENodeTypeExpr.Constant] = 0,
            [ENodeTypeExpr.BinaryExpr] = 0
        };
    }
}
