using OpenFL.Core.ProgramChecks;

namespace OpenFL.Core.Exceptions
{
    public class FLProgramCheckException : Byt3Exception
    {

        public FLProgramCheckException(
            string errorMessage,
            FLProgramCheck stage) : base(stage.GetType().Name + ": " + errorMessage)
        {
        }

    }
}