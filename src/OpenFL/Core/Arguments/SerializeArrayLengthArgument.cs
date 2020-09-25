using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeArrayLengthArgument : SerializableFLInstructionArgument
    {

        public SerializeArrayLengthArgument(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;

        public override string Identifier => "~" + Value;

        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<decimal>(() => script.DefinedBuffers[Value].Size);
        }

        public override string ToString()
        {
            return Identifier;
        }

    }
}