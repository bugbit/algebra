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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public abstract class UserExpression<T> : IUserExpression
    {
        protected PadContext mContext;

        public UserExpression(PadContext argContext)
        {
            mContext = argContext;
        }

        public abstract Expression<Func<object>> Expr { get; }
    }

    public class PruebaUserExpression : UserExpression<int>
    {
        public PruebaUserExpression(PadContext argContext) : base(argContext) { }

        public override Expression<Func<object>> Expr => throw new NotImplementedException();
    }
}
