using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;
using OpenFL.Serialization.Exceptions;

using PluginSystem.Core.Interfaces;
using PluginSystem.Core.Pointer;

using Utility.Serialization;
using Utility.Serialization.Serializers;

namespace OpenFL.Serialization.Serializers.Internal
{
    public class SerializableFLProgramSerializer : ASerializer<SerializableFLProgram>, IPluginHost
    {

        public readonly Byt3Serializer BufferSerializer;
        private readonly FLInstructionSet instructionSet;

        public SerializableFLProgramSerializer(Dictionary<Type, FLBaseSerializer> serializers, FLInstructionSet iset)
        {
            instructionSet = iset;
            BufferSerializer = Byt3Serializer.GetDefaultSerializer();
            int i = 0;
            foreach (KeyValuePair<Type, FLBaseSerializer> keyValuePair in serializers)
            {
                BufferSerializer.AddSerializer(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public bool IsAllowedPlugin(IPlugin plugin)
        {
            return true;
        }

        public void OnPluginLoad(IPlugin plugin, BasePluginPointer ptr)
        {
        }

        public void OnPluginUnload(IPlugin plugin)
        {
        }

        public override SerializableFLProgram DeserializePacket(PrimitiveValueWrapper s)
        {
            int funcCount = s.ReadInt();
            int defCount = s.ReadInt();
            int extCount = s.ReadInt();

            List<SerializableFLBuffer> defs = new List<SerializableFLBuffer>();
            List<SerializableFLFunction> funcs = new List<SerializableFLFunction>();
            List<SerializableExternalFLFunction> exts = new List<SerializableExternalFLFunction>();

            string[] idMap = ReadStringArray(s);
            SetIdMap(idMap);

            for (int i = 0; i < defCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!BufferSerializer.TryReadPacket(temp, out SerializableFLBuffer def))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable Defined buffer: ID: {i}"
                                                        );
                }

                defs.Add(def);
            }

            for (int i = 0; i < funcCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!BufferSerializer.TryReadPacket(temp, out SerializableFLFunction def))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable Defined buffer: ID: {i}"
                                                        );
                }

                funcs.Add(def);
            }

            for (int i = 0; i < extCount; i++)
            {
                MemoryStream temp = new MemoryStream(s.ReadBytes());
                if (!BufferSerializer.TryReadPacket(temp, out SerializableExternalFLFunction def))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable Defined buffer: ID: {i}"
                                                        );
                }

                exts.Add(def);
            }

            return new SerializableFLProgram("DeserializedScript", exts, funcs, defs);
        }

        private void WriteStringArray(PrimitiveValueWrapper s, string[] arr)
        {
            s.Write(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                s.Write(arr[i]);
            }
        }

        private string[] ReadStringArray(PrimitiveValueWrapper s)
        {
            string[] ret = new string[s.ReadInt()];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = s.ReadString();
            }

            return ret;
        }

        private void SetIdMap(string[] idMap)
        {
            for (int i = 0; i < BufferSerializer.ContainedSerializers; i++)
            {
                if (BufferSerializer.GetSerializerAt(i) is FLBaseSerializer flBufferSerializer)
                {
                    flBufferSerializer.SetIdMap(idMap.ToArray());
                }
            }
        }

        public override void SerializePacket(PrimitiveValueWrapper s, SerializableFLProgram obj)
        {
            int funcCount = obj.Functions.Count;
            int defCount = obj.DefinedBuffers.Count;
            int extCount = obj.ExternalFunctions.Count;

            s.Write(funcCount);
            s.Write(defCount);
            s.Write(extCount);

            string[] funcMap = obj.Functions.Select(x => x.Name).ToArray();
            string[] exMap = obj.ExternalFunctions.Select(x => x.Name).ToArray();
            string[] bufMap = obj.DefinedBuffers.Select(x => x.Name).ToArray();

            List<string> idMap = new List<string>();
            idMap.AddRange(funcMap);
            idMap.AddRange(exMap);
            idMap.AddRange(bufMap);
            idMap.AddRange(instructionSet.GetInstructionNames());

            WriteStringArray(s, idMap.ToArray());

            SetIdMap(idMap.ToArray());


            for (int i = 0; i < obj.DefinedBuffers.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!BufferSerializer.TryWritePacket(temp, obj.DefinedBuffers[i]))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable Defined buffer: {obj.DefinedBuffers[i].Name} ID: {i}"
                                                        );
                }

                s.Write(temp.GetBuffer(), (int) temp.Position);
            }

            for (int i = 0; i < obj.Functions.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!BufferSerializer.TryWritePacket(temp, obj.Functions[i]))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable Function: {obj.Functions[i].Name} ID: {i}"
                                                        );
                }

                s.Write(temp.GetBuffer(), (int) temp.Position);
            }

            for (int i = 0; i < obj.ExternalFunctions.Count; i++)
            {
                MemoryStream temp = new MemoryStream();
                if (!BufferSerializer.TryWritePacket(temp, obj.ExternalFunctions[i]))
                {
                    throw new FLDeserializationException(
                                                         $"Can not Deserialize Serializable External Function: {obj.ExternalFunctions[i].Name} ID: {i}"
                                                        );
                }

                s.Write(temp.GetBuffer(), (int) temp.Position);
            }
        }

    }
}