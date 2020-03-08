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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Algebra.Core.Exprs;

namespace Algebra.Core
{
    public interface IVars
    {
        void Set(string n, NodeExpr e);
        NodeExpr Get(string n);
        NodeExpr this[string n] { get; set; }
        IVars CreateAmbito(IVars argVars);
    }
    public class Vars : IVars
    {
        private class Ambito : IVars
        {
            private IVars mVars;
            private IVars mAmbito;

            public Ambito(IVars argVars, IVars argAmbito)
            {
                mVars = argVars;
                mAmbito = argAmbito;
            }

            public NodeExpr this[string n] { get => mVars[n]; set => mVars[n] = value; }

            public NodeExpr Get(string n)
            {
                var e = mAmbito.Get(n);

                return (e.IsNull) ? mVars.Get(n) : e;
            }

            public void Set(string n, NodeExpr e)
            {
                mVars.Set(n, e);
            }

            public IVars CreateAmbito(IVars argVars) => new Ambito(this, argVars);
        }

        private ConcurrentDictionary<string, NodeExpr> mVars = new ConcurrentDictionary<string, NodeExpr>();

        public void Add(Vars v)
        {
            foreach (var vv in v.mVars)
                Set(vv.Key, vv.Value);
        }

        public void Set(string n, NodeExpr e)
        {
            mVars.AddOrUpdate(n, e, (nn, ee) => ee);
        }

        public NodeExpr Get(string n)
        {
            if (mVars.TryGetValue(n, out NodeExpr e))
                return e;

            return Exprs.NodeExpr.Null;
        }

        public NodeExpr this[string n]
        {
            get => Get(n);
            set => Set(n, value);
        }

        public IVars CreateAmbito(IVars argVars) => new Ambito(this, argVars);
    }
}
