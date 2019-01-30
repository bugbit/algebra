using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Core.Exprs.Parser
{
    public sealed class Tokenizer
    {
        public enum EType
        {
            Number, Operation, OpenParenthesis, CloseParenthesis, TerminateSemiColon, TerminateDolar, EOF
        }

        private string mStr;
        private bool bWait = false;

        public EType TypeToken { get; private set; }
        public string Token { get; private set; }
        public string Value { get; private set; }

        public Tokenizer(string argStr)
        {
            mStr = argStr;
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
        }
    }
}
