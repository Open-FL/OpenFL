using System;

using Utility.Exceptions;

namespace OpenFL.Core.Exceptions
{
    public class FLInvalidDefineStatementException : Byt3Exception
    {

        public FLInvalidDefineStatementException(string errorMessage) : base(errorMessage)
        {
        }

        public FLInvalidDefineStatementException(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
        }

    }
}