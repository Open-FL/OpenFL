using OpenFL.Core.Arguments;

using Utility.Serialization;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableNameArgumentSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeNameArgument(s.ReadString());
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write((obj as SerializeNameArgument).Value);
        }

    }
}