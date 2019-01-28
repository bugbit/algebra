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
    public enum ETypeExpr
    {
        Constant
    }
    public class Expr : ICloneable
    {
        public ETypeExpr TypeExpr { get; }

        protected Expr(ETypeExpr type)
        {
            TypeExpr = type;
        }

        protected Expr(Expr e) : this(e.TypeExpr) { }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public virtual Expr Clone()
        {
            return new Expr(this);
        }
    }

    public class ExprCte : Expr
    {
        protected ExprCte() : base(ETypeExpr.Constant) { }

        public ExprCte(object value) : this()
        {
            Value = value;
        }

        public object Value { get; }

        public bool IsNumber => Precisions.PrecisionsTypes.ContainsKey(Value.GetType());
    }
}
