using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromFileFLBufferCreator : ASerializableBufferCreator
    {

        public override SerializableFLBuffer CreateBuffer(
            string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            return new SerializableFromFileFLBuffer(name, args[0].Replace("\"", ""), modifiers, arraySize);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey.StartsWith("\"") && bufferKey.EndsWith("\"");
        }

    }
}