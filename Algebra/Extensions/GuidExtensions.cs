using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Extensions
{
   public static class GuidExtensions
    {
        public static string ToClassName(this Guid argGuid) => argGuid.ToString().Replace("-", string.Empty);
    }
}
