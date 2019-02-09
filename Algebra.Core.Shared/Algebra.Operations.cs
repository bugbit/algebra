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
using Algebra.Core.Exprs;

using static System.Math;
using static DecimalMath.DecimalEx;

namespace Algebra.Core
{

    public partial interface IAlgebra<T>
    {
        T Add(T n1, T n2);
        T Sub(T n1, T n2);
        T Mult(T n1, T n2);
        T Div(T n1, T n2);
        T Pow(T n1, T n2);
        T EvalBinaryOperator(ETypeBinary t, T n1, T n2);
    }

    public partial class Algebra<T>
    {
        private Dictionary<ETypeBinary, Func<T, T, T>> mBinaryOperators;

        public abstract T Add(T n1, T n2);
        public abstract T Sub(T n1, T n2);
        public abstract T Mult(T n1, T n2);
        public abstract T Div(T n1, T n2);
        public abstract T Pow(T n1, T n2);
        public T EvalBinaryOperator(ETypeBinary t, T n1, T n2) => mBinaryOperators[t].Invoke(n1, n2);

        private void InitializeBinaryOperations()
        {
            mBinaryOperators = new Dictionary<ETypeBinary, Func<T, T, T>>
            {
                [ETypeBinary.Add] = Add,
                [ETypeBinary.Sub] = Sub,
                [ETypeBinary.Mult] = Mult,
                [ETypeBinary.Div] = Div,
                [ETypeBinary.Pow] = Pow,
            };
        }
    }

    public partial class AlgebraInt
    {
        public override int Add(int n1, int n2) => n1 + n2;
        public override int Sub(int n1, int n2) => n1 - n2;
        public override int Mult(int n1, int n2) => n1 * n2;
        public override int Div(int n1, int n2) => n1 / n2;
        public override int Pow(int n1, int n2) => (int)BigInteger.Pow(n1, n2);
    }

    public partial class AlgebraLong
    {
        public override long Add(long n1, long n2) => n1 + n2;
        public override long Sub(long n1, long n2) => n1 - n2;
        public override long Mult(long n1, long n2) => n1 * n2;
        public override long Div(long n1, long n2) => n1 / n2;
        public override long Pow(long n1, long n2) => (long)BigInteger.Pow(n1, (int)n2);
    }

    public partial class AlgebraBigInteger
    {
        public override BigInteger Add(BigInteger n1, BigInteger n2) => n1 + n2;
        public override BigInteger Sub(BigInteger n1, BigInteger n2) => n1 - n2;
        public override BigInteger Mult(BigInteger n1, BigInteger n2) => n1 * n2;
        public override BigInteger Div(BigInteger n1, BigInteger n2) => n1 / n2;
        public override BigInteger Pow(BigInteger n1, BigInteger n2) => BigInteger.Pow(n1, (int)n2);
    }

    public partial class AlgebraFloat
    {
        public override float Add(float n1, float n2) => n1 + n2;
        public override float Sub(float n1, float n2) => n1 - n2;
        public override float Mult(float n1, float n2) => n1 * n2;
        public override float Div(float n1, float n2) => n1 / n2;
        public override float Pow(float n1, float n2) => Pow(n1, n2);
    }

    public partial class AlgebraDouble
    {
        public override double Add(double n1, double n2) => n1 + n2;
        public override double Sub(double n1, double n2) => n1 - n2;
        public override double Mult(double n1, double n2) => n1 * n2;
        public override double Div(double n1, double n2) => n1 / n2;
        public override double Pow(double n1, double n2) => Pow(n1, n2);
    }

    public partial class AlgebraDecimal
    {
        public override decimal Add(decimal n1, decimal n2) => n1 + n2;
        public override decimal Sub(decimal n1, decimal n2) => n1 - n2;
        public override decimal Mult(decimal n1, decimal n2) => n1 * n2;
        public override decimal Div(decimal n1, decimal n2) => n1 / n2;
        public override decimal Pow(decimal n1, decimal n2) => Pow(n1, n2);
    }

    public partial class AlgebraBigDecimal
    {
        public override BigDecimal Add(BigDecimal n1, BigDecimal n2) => (decimal)n1 + (decimal)n2;
        public override BigDecimal Sub(BigDecimal n1, BigDecimal n2) => (decimal)n1 - (decimal)n2;
        public override BigDecimal Mult(BigDecimal n1, BigDecimal n2) => (decimal)n1 * (decimal)n2;
        public override BigDecimal Div(BigDecimal n1, BigDecimal n2) => (decimal)n1 / (decimal)n2;
        public override BigDecimal Pow(BigDecimal n1, BigDecimal n2) => Pow((decimal)n1, (decimal)n2);
    }
}
