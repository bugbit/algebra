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
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Extensions;
using Algebra.Core.Math;

namespace Algebra.Core
{
    public partial interface IAlgebra<T>
    {
        bool IsNumberInteger(T n);
        bool Miller(T n, int iteration);
        Task<bool> PrimeP(T n, EAlgorithmPrimeP a, CancellationToken t);
    }

    public partial class Algebra<T>
    {
        private Dictionary<EAlgorithmPrimeP, Func<T, CancellationToken, Task<bool>>> mAlgorithmsPrimeP;

        public abstract bool IsNumberInteger(T n);
        public abstract bool Miller(T n, int iteration);

        public Task<bool> PrimeP(T n, EAlgorithmPrimeP a, CancellationToken t) => mAlgorithmsPrimeP[a].Invoke(n, t);

        private void InitializeFunctions()
        {
            mAlgorithmsPrimeP = new Dictionary<EAlgorithmPrimeP, Func<T, CancellationToken, Task<bool>>>
            {
                [EAlgorithmPrimeP.Miller] = (n, t) => Task.Run(() => Miller(n, 20), t) // 18-20 iterations are enough for most of the applications.
            };
        }
    }

    public partial class AlgebraFloat
    {
        public override bool IsNumberInteger(float n) => (System.Convert.ToDecimal(n) % 1m) == 0m;
        public override bool Miller(float n, int iteration) => MathEx.Miller((int)n, iteration);
    }

    public partial class AlgebraDouble
    {
        public override bool IsNumberInteger(double n) => (System.Convert.ToDecimal(n) % 1m) == 0m;
        public override bool Miller(double n, int iteration) => MathEx.Miller((long)n, iteration);
    }

    public partial class AlgebraDecimal
    {
        public override bool IsNumberInteger(decimal n) => (n % 1m) == 0m;
        public override bool Miller(decimal n, int iteration) => MathEx.Miller((BigInteger)n, iteration);
    }

    public partial class AlgebraBigDecimal
    {
        public override bool IsNumberInteger(BigDecimal n) => n.Scale <= 0;
        public override bool Miller(BigDecimal n, int iteration) => MathEx.Miller((BigInteger)n, iteration);
    }
}
