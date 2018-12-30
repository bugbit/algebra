using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra
{
    public class PadProgress
    {
        public string Name { get; set; }
        public bool Visible { get; set; } = true;
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Progress { get; set; }
    }
}
