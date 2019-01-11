using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.ExpressionCAS
{
    public class PadContext
    {
        public HashSet<Assembly> GeneratesAssemblies { get; } = new HashSet<Assembly>();
        public List<IUserExpression> GeneratesUserExprs { get; } = new List<IUserExpression>();
    }
}
