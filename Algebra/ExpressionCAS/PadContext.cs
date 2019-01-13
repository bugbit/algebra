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
