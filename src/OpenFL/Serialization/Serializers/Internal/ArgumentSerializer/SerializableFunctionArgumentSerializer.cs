using OpenFL.Core.Arguments;

using Utility.Serialization;

namespace OpenFL.Serialization.Serializers.Internal.ArgumentSerializer
{
    public class SerializableFunctionArgumentSerializer : FLBaseSerializer
    {

        public override object Deserialize(PrimitiveValueWrapper s)
        {
            return new SerializeFunctionArgument(ResolveId(s.ReadInt()));
        }

        public override void Serialize(PrimitiveValueWrapper s, object obj)
        {
            s.Write(ResolveName((obj as SerializeFunctionArgument).Value));
        }

    }
}