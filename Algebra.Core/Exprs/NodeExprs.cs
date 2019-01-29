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
    public enum ENodeTypeExpr
    {
        Constant, BinaryExpr
    }

    public enum ETypeConstant
    {
        Number
    }

    public enum ETypeBinary
    {
        Add, Sub, Mult, Div, Pow
    }

    public abstract class NodeExpr : ICloneable
    {
        public ENodeTypeExpr TypeExpr { get; }

        protected NodeExpr(ENodeTypeExpr type)
        {
            TypeExpr = type;
        }

        protected NodeExpr(NodeExpr e) : this(e.TypeExpr) { }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public virtual T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);

        public virtual int Priority => MathExpr.TypeNodesPriorities[TypeExpr];

        public abstract NodeExpr Clone();
    }

    public class NodeExprCte : NodeExpr
    {
        protected NodeExprCte() : base(ENodeTypeExpr.Constant) { }

        public NodeExprCte(object value, ETypeConstant type) : this()
        {
            Value = value;
            TypeConstant = type;
        }

        public NodeExprCte(NodeExprCte e) : this(e.Value, e.TypeConstant) { }

        public object Value { get; }

        public ETypeConstant TypeConstant { get; }

        public override T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);

        public override NodeExpr Clone() => new NodeExprCte(this);
    }

    public class NodeBinaryExpr : NodeExpr
    {
        public ETypeBinary TypeBinary { get; }
        public NodeExpr Left { get; }
        public NodeExpr Right { get; }

        protected NodeBinaryExpr() : base(ENodeTypeExpr.BinaryExpr) { }

        public NodeBinaryExpr(ETypeBinary type, NodeExpr left, NodeExpr right) : this()
        {
            TypeBinary = type;
            Left = left;
            Right = right;
        }

        public NodeBinaryExpr(NodeBinaryExpr e) : this(e.TypeBinary, e.Left, e.Right) { }

        public override T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);

        public override int Priority => MathExpr.TypeBinariesPriorities[TypeBinary];

        public override NodeExpr Clone() => new NodeBinaryExpr(this);
    }
}
