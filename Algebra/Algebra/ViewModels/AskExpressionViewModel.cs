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

namespace Algebra.ViewModels
{
    public class AskExpressionViewModel<T> : AskViewModel<T>
    {
        private string mExprStr;
        private Core.Exprs.NodeExpr mExpr;

        public string ExprStr { get => mExprStr; set => SetProperty(ref mExprStr, value); }
        public Core.Exprs.NodeExpr Expr { get => mExpr; set => SetProperty(ref mExpr, value); }
    }
}
