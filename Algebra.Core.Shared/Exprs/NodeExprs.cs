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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public enum ENodeTypeExpr
    {
        Constant, Unary, BinaryExpr, Instruction
    }

    public enum ETypeConstant
    {
        Number, Null
    }

    public enum ETypeBinary
    {
        Add, Sub, Mult, Div, Pow
    }

    public enum ETypeUnary
    {
        SignPos, SigNeg
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

        public override string ToString() => NodeExprStringBuilderVisitor.ToString(this);

        public virtual T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);
        public virtual Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => visitor.Visit(this, t);

        public virtual int Priority => MathExpr.TypeNodesPriorities[TypeExpr];

        public abstract NodeExpr Clone();

        public static NodeExprCte Null => new NodeExprCte(null, ETypeConstant.Null);
        public static NodeExprNumber<N> Number<N>(N n) => new NodeExprNumber<N>(n);
        public static NodeExprUnary Unary(ETypeUnary t, NodeExpr e) => new NodeExprUnary(t, e);
        public static NodeExprBinary Binary(ETypeBinary t, NodeExpr l, NodeExpr r) => new NodeExprBinary(t, l, r);
        public static NodeExprInstruction Instruction(NodeExpr e, bool isshowresult) => new NodeExprInstruction(e, isshowresult);
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
        public override Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => visitor.Visit(this, t);

        public override NodeExpr Clone() => new NodeExprCte(this);
    }

    public class NodeExprNumber<N> : NodeExprCte
    {
        protected NodeExprNumber() : base() { }

        public NodeExprNumber(N n) : base(n, ETypeConstant.Number) { }

        public NodeExprNumber(NodeExprNumber<N> e) : this(e.Value) { }

        new public N Value => (N)base.Value;

        public override T Accept<T>(INodeExprVisitor<T> visitor) => (visitor is INodeExprVisitor<N, T>) ? ((INodeExprVisitor<N, T>)visitor).Visit(this) : visitor.Visit(this);
        public override async Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => (visitor is INodeExprVisitorAsync<N, T>) ? await ((INodeExprVisitorAsync<N, T>)visitor).Visit(this, t) : await visitor.Visit(this, t);

        public override NodeExpr Clone() => new NodeExprNumber<N>(this);
    }

    public class NodeExprUnary : NodeExpr
    {
        protected NodeExprUnary() : base(ENodeTypeExpr.Unary) { }

        public NodeExprUnary(ETypeUnary t, NodeExpr e) : this()
        {
            TypeUnary = t;
            Expr = e;
        }

        public NodeExprUnary(NodeExprUnary e) : this(e.TypeUnary, e.Expr) { }

        public ETypeUnary TypeUnary { get; }
        public NodeExpr Expr { get; }


        public override T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);
        public override Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => visitor.Visit(this, t);
        public override NodeExpr Clone() => new NodeExprUnary(this);
    }

    public class NodeExprBinary : NodeExpr
    {
        public ETypeBinary TypeBinary { get; }
        public NodeExpr Left { get; }
        public NodeExpr Right { get; }

        protected NodeExprBinary() : base(ENodeTypeExpr.BinaryExpr) { }

        public NodeExprBinary(ETypeBinary type, NodeExpr left, NodeExpr right) : this()
        {
            TypeBinary = type;
            Left = left;
            Right = right;
        }

        public NodeExprBinary(NodeExprBinary e) : this(e.TypeBinary, e.Left, e.Right) { }

        public override T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);
        public override Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => visitor.Visit(this, t);

        public override int Priority => MathExpr.TypeBinariesPriorities[TypeBinary];

        public bool IsNecesaryParenthesisLeft => Left.Priority > Priority;
        public bool IsNecesaryParenthesisRight => Priority < Right.Priority;

        public override NodeExpr Clone() => new NodeExprBinary(this);
    }

    public class NodeExprInstruction : NodeExpr
    {
        public NodeExpr Expr { get; }
        public bool IsShowResult { get; }

        protected NodeExprInstruction() : base(ENodeTypeExpr.Instruction) { }

        public NodeExprInstruction(NodeExpr e, bool isshowresult) : this()
        {
            Expr = e;
            IsShowResult = isshowresult;
        }

        public NodeExprInstruction(NodeExprInstruction e) : this(e.Expr, e.IsShowResult) { }

        public override T Accept<T>(INodeExprVisitor<T> visitor) => visitor.Visit(this);
        public override Task<T> Accept<T>(INodeExprVisitorAsync<T> visitor, CancellationToken t) => visitor.Visit(this, t);

        public override NodeExpr Clone() => new NodeExprInstruction(this);
    }
}
