using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Core
{
    public class LaTex
    {
        private StringBuilder mStr;

        public LaTex()
        {
            mStr = new StringBuilder();
        }

        public void Append(object o) => mStr.Append(Convert.ToString(o));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">ex: {r|r}</param>
        /// <param name="objs"></param>
        public void Array(string c, IEnumerable<object> objs)
        {
            var cols = c.Split('|').Length;
            var n = 0;

            mStr.Append(@"\begin{array} ");
            mStr.Append(c);
            foreach (var o in objs)
            {
                if (n == 0)
                    n = 1;
                else
                    mStr.Append((n == cols) ? @"\\" : " &");
                Append(o);
                n++;
            }
            mStr.Append(@"\\");
        }
    }
}
