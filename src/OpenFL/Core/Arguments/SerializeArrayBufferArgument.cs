using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeArrayBufferArgument : SerializeBufferArgument
    {

        public SerializeArrayBufferArgument(string index) : base(index)
        {
        }

        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.BufferArray;

    }
}