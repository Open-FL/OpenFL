﻿using System.Collections.Generic;

using OpenFL.Serialization.Exceptions;
using OpenFL.Serialization.FileFormat;
using OpenFL.Serialization.FileFormat.ExtraStages;

using Utility.Serialization;
using Utility.Serialization.Serializers;

namespace OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLFileFormatSerializer : ASerializer<FLFileFormat>
    {

        private static readonly Dictionary<string, ExtraStage> ExtraSteps = new Dictionary<string, ExtraStage>
                                                                            {
                                                                                {
                                                                                    "zip", new ExtraStage(
                                                                                         new
                                                                                             ZipExtraStage(),
                                                                                         new
                                                                                             UnZipExtraStage()
                                                                                        )
                                                                                }
                                                                            };

        private readonly FLHeaderSerializer flHs = new FLHeaderSerializer();

        private readonly FLProgramHeaderSerializer phs = new FLProgramHeaderSerializer();

        public override FLFileFormat DeserializePacket(PrimitiveValueWrapper s)
        {
            FLHeader header = flHs.DeserializePacket(s);
            if (!header.IsCompatible())
            {
                throw new FLDeserializationException("The Loaded Script is not compatible.");
            }

            FLProgramHeader programHeader = phs.DeserializePacket(s);

            byte[] program = s.ReadBytes();

            List<ExtraStage> extraStages = CreateExtraStages(header.ExtraSerializationSteps);

            for (int i = 0; i < extraStages.Count; i++)
            {
                program = extraStages[i].FromFile.Process(program);
            }

            return new FLFileFormat(header, programHeader, program);
        }

        private List<ExtraStage> CreateExtraStages(string[] extraStages)
        {
            List<ExtraStage> ret = new List<ExtraStage>();
            for (int i = 0; i < extraStages.Length; i++)
            {
                if (string.IsNullOrEmpty(extraStages[i]))
                {
                    continue;
                }

                if (!ExtraSteps.ContainsKey(extraStages[i]))
                {
                    throw new FLDeserializationException(
                                                         "Can not Deserialize Script because it used an Extra Stage that is not available."
                                                        );
                }

                ret.Add(ExtraSteps[extraStages[i]]);
            }

            return ret;
        }

        public override void SerializePacket(PrimitiveValueWrapper s, FLFileFormat obj)
        {
            flHs.SerializePacket(s, obj.CompilerHeader);
            phs.SerializePacket(s, obj.ProgramHeader);

            byte[] program = obj.Program;

            List<ExtraStage> extraStages = CreateExtraStages(obj.CompilerHeader.ExtraSerializationSteps);
            for (int i = 0; i < extraStages.Count; i++)
            {
                program = extraStages[i].ToFile.Process(program);
            }

            s.Write(program);
        }

    }
}