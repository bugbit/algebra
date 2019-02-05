using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core
{
    public class Vars
    {
        private ConcurrentDictionary<string, Exprs.NodeExpr> mVars = new ConcurrentDictionary<string, Exprs.NodeExpr>();

        public void Add(Vars v)
        {
            foreach (var vv in v.mVars)
                Set(vv.Key, vv.Value);
        }

        public void Set(string n, Exprs.NodeExpr e)
        {
            mVars.AddOrUpdate(n, e, (nn, ee) => ee);
        }

        public Exprs.NodeExpr Get(string n)
        {
            Exprs.NodeExpr e;

            if (mVars.TryGetValue(n, out e))
                return e;

            return Exprs.NodeExpr.Null;
        }

        public Exprs.NodeExpr this[string n]
        {
            get => Get(n);
            set => Set(n, value);
        }
    }
}
