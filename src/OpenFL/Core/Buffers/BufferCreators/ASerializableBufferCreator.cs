using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.Buffers.BufferCreators
{
    /// <summary>
    ///     Used when parsing
    ///     Creates a ParsableBuffer object based on the string arguments.
    /// </summary>
    public abstract class ASerializableBufferCreator
    {

        public abstract bool IsCorrectBuffer(string bufferKey);

        public abstract SerializableFLBuffer CreateBuffer(
            string name, string[] args, FLBufferModifiers modifiers,
            int arraySize);

    }
}