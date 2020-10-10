using OpenFL.Core.Arguments;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableArrayLengthSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeArrayLengthArgument(s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeArrayLengthArgument).Value);
        }

    }

    //SerializeArrayElementArgumentEnclosedIndex
}