using OpenFL.Core.Arguments;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableArrayElementArgumentVariableIndexSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayElementArgumentVariableIndex(s.ReadString(), s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeArrayElementArgumentVariableIndex).Value);
            s.Write((obj as SerializeArrayElementArgumentVariableIndex).Index);
        }

    }
}