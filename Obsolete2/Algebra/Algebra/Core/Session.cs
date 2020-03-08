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
using System.Linq;
using System.Text;
using System.Threading;

namespace Algebra.Core
{
    public class Session
    {
        private int mVarCounter = 0;

        public Session()
        {
            ChangeAlgebra(EPrecisions.Default);
        }

        public IAlgebra Alg { get; private set; }

        public Vars Vars => new Vars();

        public void ChangeAlgebra(EPrecisions p)
        {
            Alg = Algebra.Algebras[p].Invoke(this);
        }

        public string[] CreateVarsCounters(IEnumerable<string> argVarsNames)
        {
            var pCounter = Interlocked.Increment(ref mVarCounter);

            return (from n in argVarsNames select $"_{n}{pCounter}").ToArray();
        }

        public string[] PeekVarsCounters(IEnumerable<string> argVarsNames)
        {
            var pRet = CreateVarsCounters(argVarsNames);

            Interlocked.Decrement(ref mVarCounter);

            return pRet;
        }
    }
}
