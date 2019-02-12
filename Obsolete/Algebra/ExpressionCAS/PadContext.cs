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
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Algebra.ExpressionCAS
{
    public class PadContext
    {
        private int mVarCounter = 0;
        private ConcurrentDictionary<string, object> mVars = new ConcurrentDictionary<string, object>();

        public List<Assembly> GeneratesAssemblies { get; } = new List<Assembly>();

        public string[] CreateVarsCounters(IEnumerable<string> argVarsNames)
        {
            var pCounter = Interlocked.Increment(ref mVarCounter);

            return (from n in argVarsNames select $"_{n}{pCounter}").ToArray();
        }

        public void SetVar(string argVarName, object argValue)
        {
            mVars[argVarName] = argValue;
        }

        public void AddVars(IDictionary<string, object> argVars)
        {
            foreach (var kv in argVars)
                SetVar(kv.Key, kv.Value);
        }

        public object GetVar(string argVarName)
        {
            object pValue;

            mVars.TryGetValue(argVarName, out pValue);

            return pValue;
        }

        public Expression GetVarAsExpressiom(string argVarName)
        {
            var pValue = GetVar(argVarName);

            return (pValue == null) ? Expression.Empty() : (pValue as Expression) ?? Expression.Constant(pValue);
        }
    }
}
