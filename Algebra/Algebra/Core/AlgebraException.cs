using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Algebra.Core
{
    public class AlgebraException : ApplicationException
    {
        public AlgebraException()
        {
        }

        public AlgebraException(string message) : base(message)
        {
        }

        public AlgebraException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlgebraException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
