using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private void GetToken()
        {
            if (bWait)
            {
                bWait = false;

                return;
            }

            Token = Value = "";
            while (mPos < mStrLen && char.IsWhiteSpace(mStr[mPos]))
                mPos++;

            if (mPos >= mStrLen)
            {
                TypeToken = EType.EOF;

                return;
            }

            var pCar = mStr[mPos++];

            if (mOperatorsChars.Contains(pCar))
            {
                Token = Value = pCar.ToString();
                TypeToken = EType.Operation;
            }
            else if (char.IsDigit(pCar))
            {
                TypeToken = EType.Number;
                while (char.IsDigit(pCar))
                {
                    Token += pCar;
                    pCar = mStr[mPos++];
                    if (mPos >= mStrLen)
                        return;
                }
                mPos--;

                return;
            }
            else
            {
                switch (pCar)
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
