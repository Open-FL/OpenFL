using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeExternalFunctionArgument : SerializableFLInstructionArgument
    {

        public SerializeExternalFunctionArgument(string index)
        {
            Value = index;
        }

        public string Value { get; }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Script;

        public override string Identifier => Value;


        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            return new ImplicitCastBox<IFunction>(() => script.DefinedScripts[Value]);
        }

        public override string ToString()
        {
            return Value;
        }

    }
}