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

        private char[] mOperatorsChars = MathExpr.TypeBinariesStr.Values.Select(s => s[0]).ToArray();
        private string mStr;
        private int mStrLen;
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

            Token = "";
            Value = "";

            while (mPos < mStrLen && char.IsWhiteSpace(mStr[mPos]))
                mPos++;

            if (mPos >= mStrLen)
            {
                TypeToken = EType.EOF;

                return;
            }
        }
    }
}
