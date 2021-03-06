﻿#region LICENSE
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
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public static class ExpressionCAS
    {
        public static object Eval(Expression argExpr)
        {
            var pTask = EvalAsync(argExpr);

            pTask.Wait();

            return pTask.Result;
        }
        public async static Task<object> EvalAsync(Expression argExpr)
        {
            if (argExpr.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)argExpr).Value;

            var pExpr = (argExpr as LambdaExpression) ?? Expression.Lambda(argExpr);

            return await Task.Run(() => pExpr.Compile().DynamicInvoke(null));
        }
    }
}
