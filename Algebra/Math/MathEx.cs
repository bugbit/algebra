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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Math
{
    public static partial class MathEx
    {
        public static bool IsNumber(object argObj)
        {
            switch (Type.GetTypeCode(argObj.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                    return false;
                case TypeCode.Object:
                    return (argObj is BigInteger) || (argObj is BigDecimal);
                default:
                    return true;
            }
        }

        public static bool IsInteger(object argObj)
        {
            switch (Type.GetTypeCode(argObj.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                    return false;
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return (Convert.ToDecimal(argObj) % 1) == 0m;
                case TypeCode.Object:
                    if (argObj is BigInteger)
                        return true;
                    if (argObj is BigDecimal)
                        return ((BigDecimal)argObj).Scale <= 0;

                    return false;
                default:
                    return true;
            }
        }

        public static BigInteger Factorial(BigInteger n)
        {
            BigInteger r = n;

            while (--n > 1)
                r *= n;

            return r;
        }
    }
}
