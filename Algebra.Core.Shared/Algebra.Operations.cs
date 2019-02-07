using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra.Core
{

    public partial interface IAlgebra<T>
    {
        T Add(T n1, T n2);
        T Sub(T n1, T n2);
        T Mult(T n1, T n2);
        T Div(T n1, T n2);
    }

    public partial class Algebra<T>
    {
        public abstract T Add(T n1, T n2);
        public abstract T Sub(T n1, T n2);
        public abstract T Mult(T n1, T n2);
        public abstract T Div(T n1, T n2);
    }

    public partial class AlgebraInt
    {
        public override int Add(int n1, int n2) => n1 + n2;
        public override int Sub(int n1, int n2) => n1 - n2;
        public override int Mult(int n1, int n2) => n1 * n2;
        public override int Div(int n1, int n2) => n1 / n2;
    }

    public partial class AlgebraLong
    {
        public override long Add(long n1, long n2) => n1 + n2;
        public override long Sub(long n1, long n2) => n1 - n2;
        public override long Mult(long n1, long n2) => n1 * n2;
        public override long Div(long n1, long n2) => n1 / n2;
    }

    public partial class AlgebraBigInteger
    {
        public override BigInteger Add(BigInteger n1, BigInteger n2) => n1 + n2;
        public override BigInteger Sub(BigInteger n1, BigInteger n2) => n1 - n2;
        public override BigInteger Mult(BigInteger n1, BigInteger n2) => n1 * n2;
        public override BigInteger Div(BigInteger n1, BigInteger n2) => n1 / n2;
    }

    public partial class AlgebraFloat
    {
        public override float Add(float n1, float n2) => n1 + n2;
        public override float Sub(float n1, float n2) => n1 - n2;
        public override float Mult(float n1, float n2) => n1 * n2;
        public override float Div(float n1, float n2) => n1 / n2;
    }

    public partial class AlgebraDouble
    {
        public override double Add(double n1, double n2) => n1 + n2;
        public override double Sub(double n1, double n2) => n1 - n2;
        public override double Mult(double n1, double n2) => n1 * n2;
        public override double Div(double n1, double n2) => n1 / n2;
    }

    public partial class AlgebraDecimal
    {
        public override decimal Add(decimal n1, decimal n2) => n1 + n2;
        public override decimal Sub(decimal n1, decimal n2) => n1 - n2;
        public override decimal Mult(decimal n1, decimal n2) => n1 * n2;
        public override decimal Div(decimal n1, decimal n2) => n1 / n2;
    }

    public partial class AlgebraBigDecimal
    {
        public override BigDecimal Add(BigDecimal n1, BigDecimal n2) => (decimal)n1 + (decimal)n2;
        public override BigDecimal Sub(BigDecimal n1, BigDecimal n2) => (decimal)n1 - (decimal)n2;
        public override BigDecimal Mult(BigDecimal n1, BigDecimal n2) => (decimal)n1 * (decimal)n2;
        public override BigDecimal Div(BigDecimal n1, BigDecimal n2) => (decimal)n1 / (decimal)n2;
    }
}
