using OpenFL.Core.Arguments;

using Utility.Serialization;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableArrayElementArgumentValueIndexSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayElementArgumentValueIndex(ResolveId(s.ReadInt()), s.ReadInt());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName((obj as SerializeArrayElementArgumentValueIndex).Value));
            s.Write((obj as SerializeArrayElementArgumentValueIndex).Index);
        }

    }
}