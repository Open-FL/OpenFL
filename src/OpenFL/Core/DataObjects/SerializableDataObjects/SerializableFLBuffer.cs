using OpenFL.Core.Buffers;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.DataObjects.SerializableDataObjects
{
    public abstract class SerializableFLBuffer : SerializableNamedObject
    {

        protected SerializableFLBuffer(string name, FLBufferModifiers modifiers) : base(name)
        {
            Modifiers = modifiers;
        }

        public bool IsArray => Modifiers.IsArray;

        public FLBufferModifiers Modifiers { get; }

        public abstract FLBuffer GetBuffer();

        public override string ToString()
        {
            return $"{FLKeywords.DefineKey} {Modifiers} {Name}: ";
        }

    }
}