using OpenFL.Core.Buffers.BufferCreators.BuiltIn.Empty;
using OpenFL.Core.ElementModifiers;

using Utility.Serialization;

namespace OpenFL.Serialization.Serializers.Internal.BufferSerializer
{
    public class EmptyFLBufferSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            string name = ResolveId(s.ReadInt());
            string[] mods = s.ReadArray<string>();
            FLBufferModifiers bmod = new FLBufferModifiers(name, mods);
            if (bmod.IsArray)
            {
                return new SerializableEmptyFLBuffer(name, s.ReadInt(), bmod);
            }

            return new SerializableEmptyFLBuffer(name, bmod);
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            SerializableEmptyFLBuffer input = (SerializableEmptyFLBuffer) obj;

            s.Write(ResolveName(input.Name));
            s.WriteArray(input.Modifiers.GetModifiers().ToArray());
            if (input.IsArray)
            {
                s.Write(input.Size);
            }
        }

    }
}