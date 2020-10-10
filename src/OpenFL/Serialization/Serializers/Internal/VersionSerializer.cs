using System;

namespace OpenFL.Serialization.Serializers.Internal
{
    public class VersionSerializer : ASerializer<Version>
    {

        public override Version DeserializePacket(PrimitiveValueWrapper s)
        {
            return Version.Parse(s.ReadString());
        }

        public override void SerializePacket(PrimitiveValueWrapper s, Version obj)
        {
            s.Write(obj.ToString());
        }

    }
}