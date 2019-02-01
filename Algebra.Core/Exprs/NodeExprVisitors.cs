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
using System.Threading;
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public interface INodeExprVisitor<T>
    {
        T Visit(NodeExpr e);
        T Visit(NodeExprCte e);
        T Visit(NodeBinaryExpr e);
        T Visit(NodeExprInstruction e);
    }

    public interface INodeExprVisitor<N, T> : INodeExprVisitor<T>
    {
        T Visit(NodeExprNumber<N> e);
    }

    public interface INodeExprVisitorAsync<T>
    {
        Task<T> Visit(NodeExpr e, CancellationToken t);
        Task<T> Visit(NodeExprCte e, CancellationToken t);
        Task<T> Visit(NodeBinaryExpr e, CancellationToken t);
        Task<T> Visit(NodeExprInstruction e, CancellationToken t);
    }

    public interface INodeExprVisitorAsync<N, T> : INodeExprVisitorAsync<T>
    {
        Task<T> Visit(NodeExprNumber<N> e, CancellationToken t);
    }

    public class NodeExprVisitor<T> : INodeExprVisitor<T>
    {
        public virtual T Visit(NodeExpr e) => e.Accept(this);

        public virtual T Visit(NodeExprCte e) => default(T);

        public virtual T Visit(NodeBinaryExpr e) => default(T);

        public virtual T Visit(NodeExprInstruction e) => Visit(e.Expr);
    }

    public class NodeExprVisitor<N, T> : NodeExprVisitor<T>, INodeExprVisitor<N, T>
    {
        public virtual T Visit(NodeExprNumber<N> e) => default(T);
    }

    public class NodeExprVisitorASync<T> : INodeExprVisitorAsync<T>
    {
        public virtual Task<T> Visit(NodeExpr e, CancellationToken t) => e.Accept(this, t);

        public virtual Task<T> Visit(NodeExprCte e, CancellationToken t) => Task.FromResult(default(T));

        public virtual Task<T> Visit(NodeBinaryExpr e, CancellationToken t) => Task.FromResult(default(T));
        public virtual Task<T> Visit(NodeExprInstruction e, CancellationToken t) => Visit(e.Expr, t);
    }

    public class NodeExprVisitorASync<N, T> : NodeExprVisitorASync<T>, INodeExprVisitorAsync<N, T>
    {
        public virtual Task<T> Visit(NodeExprNumber<N> e, CancellationToken t) => Task.FromResult(default(T));
    }
}

