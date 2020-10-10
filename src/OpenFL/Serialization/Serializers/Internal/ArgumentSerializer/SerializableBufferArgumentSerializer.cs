using OpenFL.Core.Arguments;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableBufferArgumentSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeBufferArgument(ResolveId(s.ReadInt()));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName((obj as SerializeBufferArgument).Value));
        }

    }
}