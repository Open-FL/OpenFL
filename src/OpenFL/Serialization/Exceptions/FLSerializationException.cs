using Utility.Exceptions;

namespace OpenFL.Serialization.Exceptions
{
    public class FLSerializationException : Byt3Exception
    {

        public FLSerializationException(string errorMessage) : base(errorMessage)
        {
        }

    }
}