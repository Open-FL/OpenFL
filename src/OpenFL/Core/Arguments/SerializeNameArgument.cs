using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeNameArgument : SerializableFLInstructionArgument
    {

        public SerializeNameArgument(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Name;

        public override string Identifier => Value; //Not used anyway


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<string>(() => Value);
        }

        public override string ToString()
        {
            return Value;
        }

    }
}