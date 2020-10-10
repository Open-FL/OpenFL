﻿using Utility.Serialization;
using Utility.Serialization.Serializers;

namespace OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLProgramHeaderSerializer : ASerializer<FLProgramHeader>
    {

        private readonly VersionSerializer vs = new VersionSerializer();

        public override FLProgramHeader DeserializePacket(PrimitiveValueWrapper s)
        {
            return new FLProgramHeader(s.ReadString(), s.ReadString(), vs.DeserializePacket(s));
        }

        public override void SerializePacket(PrimitiveValueWrapper s, FLProgramHeader obj)
        {
            s.Write(obj.ProgramName);
            s.Write(obj.Author);
            vs.SerializePacket(s, obj.Version);
        }

    }
}