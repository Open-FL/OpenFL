using System.Collections.Generic;

using Utility.FastString;

namespace OpenFL.Core.DataObjects.SerializableDataObjects
{
    public class SerializableFLInstruction
    {

        public SerializableFLInstruction(string instructionKey, List<SerializableFLInstructionArgument> arguments)
        {
            InstructionKey = instructionKey;
            Arguments = arguments;
        }

        public string InstructionKey { get; }

        public List<SerializableFLInstructionArgument> Arguments { get; }

        public override string ToString()
        {
            return InstructionKey + " " + Arguments.Unpack(" ");
        }

    }
}