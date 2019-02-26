using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core
{
    class CultureManager
    {
        public static IRString CreateAlgebraIRString(string argKey) => new IRString(() => Algebra_Resources.Culture, () => Algebra_Resources.ResourceManager, argKey);
    }
}
