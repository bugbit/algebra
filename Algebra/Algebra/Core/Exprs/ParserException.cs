using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Algebra.Core.Exprs
{
    public class ParserException : AlgebraException
    {
        public ParserException()
        {
        }

        public ParserException(string message) : base(message)
        {
        }

        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
