using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeFunctionArgument : SerializableFLInstructionArgument
    {

        public SerializeFunctionArgument(string name)
        {
            Value = name;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Function;

        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<IFunction>(() => script.FlFunctions[Value]);
        }

        public override string ToString()
        {
            return Value;
        }

    }
}