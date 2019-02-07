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
using System.Threading.Tasks;

namespace Algebra.Core.Exprs
{
    public sealed class Tokenizer
    {
        public enum EType
        {
            Number, Sign, Operation, OpenParenthesis, CloseParenthesis, TerminateSemiColon, TerminateDolar, EOF
        }

        private readonly static char[] mOperatorsChars = MathExpr.TypeBinariesStr.Values.Select(s => s[0]).ToArray();
        private string mStr;
        private readonly int mStrLen;
        private int mPos = 0;
        private bool mbInit = true;
        private bool mbWait = false;

        public EType TypeToken { get; private set; }
        public string Token { get; private set; }
        public string Value { get; private set; }

        public Tokenizer(string argStr)
        {
            mStr = argStr.Trim();
            mStrLen = mStr.Length;
        }

        public void Back()
        {
            if (mbWait)
                throw new ArgumentException("Error en el estado back");

            mbWait = true;
        }

        public Task Read(CancellationToken t) => GetToken(t);

        private Task<char?> ReadCar(CancellationToken t)
        {
            t.ThrowIfCancellationRequested();
            lock (this)
            {
                if (mPos >= mStrLen)
                    return Task.FromResult((char?)null);

                return Task.FromResult((char?)mStr[mPos++]);
            }
        }

        private void BackCar()
        {
            lock (this)
            {
                mPos--;
            }
        }

        private async Task GetToken(CancellationToken t)
        {
            if (mbWait)
            {
                mbWait = false;

                return;
            }

            Token = Value = "";

            var pCar = await ReadCar(t);

            while (pCar.HasValue && char.IsWhiteSpace(pCar.Value))
                pCar = await ReadCar(t);

            if (!pCar.HasValue)
            {
                TypeToken = EType.EOF;

                return;
            }
            if (pCar == '+' || pCar == '-')
            {
                if (mbInit || (TypeToken == EType.Operation && (Token == "+" || Token == "-")))
                {
                    var pCar2 = await ReadCar(t);

                    if (pCar2.HasValue && char.IsDigit(pCar2.Value))
                    {
                        Token += pCar;
                        pCar = pCar2;
                    }
                    else
                    {
                        BackCar();
                        if (!mbInit)
                        {
                            Token = Value = pCar.ToString();
                            TypeToken = EType.Sign;

                            return;
                        }
                    }
                }

            }
            if (mbInit)
                mbInit = false;
            if (mOperatorsChars.Contains(pCar.Value))
            {
                Token = Value = pCar.ToString();
                TypeToken = EType.Operation;
            }
            else if (char.IsDigit(pCar.Value))
            {
                TypeToken = EType.Number;
                do
                {
                    t.ThrowIfCancellationRequested();
                    Token += pCar;
                    pCar = await ReadCar(t);
                    if (!pCar.HasValue)
                        break;
                } while (char.IsDigit(pCar.Value));
                if (pCar.HasValue)
                    BackCar();
                Value = Token;

                return;
            }
            else
            {
                switch (pCar.Value)
                {
                    case ';':
                        Token = Value = pCar.ToString();
                        TypeToken = EType.TerminateSemiColon;
                        return;
                    case '$':
                        Token = Value = pCar.ToString();
                        TypeToken = EType.TerminateDolar;
                        return;
                    case '(':
                        Token = Value = pCar.ToString();
                        TypeToken = EType.OpenParenthesis;
                        return;
                    case ')':
                        Token = Value = pCar.ToString();
                        TypeToken = EType.CloseParenthesis;
                        return;
                    default:
                        throw new ParserException(string.Format(Algebra_Resources.ParserExceptionUnknownToken, pCar));
                }
            }
        }
    }
}
