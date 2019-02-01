﻿#region LICENSE
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
            Number, Operation, OpenParenthesis, CloseParenthesis, TerminateSemiColon, TerminateDolar, EOF
        }

        private readonly static char[] mOperatorsChars = MathExpr.TypeBinariesStr.Values.Select(s => s[0]).ToArray();
        private string mStr;
        private readonly int mStrLen;
        private int mPos = 0;
        private bool bWait = false;

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
            if (bWait)
                throw new ArgumentException("Error en el estado back");

            bWait = true;
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

        private async Task GetToken(CancellationToken t)
        {
            if (bWait)
            {
                bWait = false;

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

            if (mOperatorsChars.Contains(pCar.Value))
            {
                Token = Value = pCar.ToString();
                TypeToken = EType.Operation;
            }
            else if (char.IsDigit(pCar.Value))
            {
                TypeToken = EType.Number;
                while (char.IsDigit(pCar.Value))
                {
                    t.ThrowIfCancellationRequested();
                    Token += pCar;
                    pCar = await ReadCar(t);
                    if (!pCar.HasValue)
                        return;
                }
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
                }
            }
        }
    }
}
