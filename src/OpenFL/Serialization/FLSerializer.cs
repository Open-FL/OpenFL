using System;
using System.Collections.Generic;
using System.IO;

using OpenFL.Core.Arguments;
using OpenFL.Core.Buffers.BufferCreators.BuiltIn.Empty;
using OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;
using OpenFL.Serialization.Exceptions;
using OpenFL.Serialization.FileFormat;
using OpenFL.Serialization.Serializers.Internal;
using OpenFL.Serialization.Serializers.Internal.ArgumentSerializer;
using OpenFL.Serialization.Serializers.Internal.BufferSerializer;
using OpenFL.Serialization.Serializers.Internal.FileFormatSerializer;

using PluginSystem.Core;

using Utility.Serialization;

namespace OpenFL.Serialization
{
    public static class FLSerializer
    {

        private static Byt3Serializer CreateLoader(FLInstructionSet iset)
        {
            SerializableBufferArgumentSerializer bbuf = new SerializableBufferArgumentSerializer();
            SerializableArrayArgumentSerializer abuf = new SerializableArrayArgumentSerializer();
            SerializableDecimalArgumentSerializer debuf = new SerializableDecimalArgumentSerializer();
            SerializableFunctionArgumentSerializer fabuf = new SerializableFunctionArgumentSerializer();
            SerializableExternalFunctionArgumentSerializer exbuf = new SerializableExternalFunctionArgumentSerializer();
            SerializableArrayLengthSerializer arLen = new SerializableArrayLengthSerializer();
            SerializableNameArgumentSerializer nArg = new SerializableNameArgumentSerializer();
            SerializableArrayElementArgumentVariableIndexSerializer arEVI =
                new SerializableArrayElementArgumentVariableIndexSerializer();
            SerializableArrayElementArgumentValueIndexSerializer arEVaI =
                new SerializableArrayElementArgumentValueIndexSerializer();
            SerializableArrayElementArgumentEnclosedIndexSerializer arEI =
                new SerializableArrayElementArgumentEnclosedIndexSerializer();
            Dictionary<Type, FLBaseSerializer> argumentParser = new Dictionary<Type, FLBaseSerializer>
                                                                {
                                                                    { typeof(SerializeBufferArgument), bbuf },
                                                                    { typeof(SerializeArrayBufferArgument), abuf },
                                                                    { typeof(SerializeDecimalArgument), debuf },
                                                                    { typeof(SerializeFunctionArgument), fabuf },
                                                                    {
                                                                        typeof(SerializeExternalFunctionArgument), exbuf
                                                                    },
                                                                    { typeof(SerializeArrayLengthArgument), arLen },
                                                                    { typeof(SerializeNameArgument), nArg },
                                                                    {
                                                                        typeof(
                                                                            SerializeArrayElementArgumentVariableIndex),
                                                                        arEVI
                                                                    },
                                                                    {
                                                                        typeof(SerializeArrayElementArgumentValueIndex),
                                                                        arEVaI
                                                                    },
                                                                    {
                                                                        typeof(
                                                                            SerializeArrayElementArgumentEnclosedIndex),
                                                                        arEI
                                                                    }
                                                                };

            SerializableFLFunctionSerializer efunc = new SerializableFLFunctionSerializer(argumentParser);
            SerializableExternalFLFunctionSerializer exfunc = new SerializableExternalFLFunctionSerializer(iset);
            EmptyFLBufferSerializer ebuf = new EmptyFLBufferSerializer();
            FromImageFLBufferSerializer fibuf = new FromImageFLBufferSerializer(true);
            Dictionary<Type, FLBaseSerializer> bufferParser =
                new Dictionary<Type, FLBaseSerializer>
                {
                    { typeof(SerializableExternalFLFunction), exfunc },
                    { typeof(SerializableFLFunction), efunc },
                    { typeof(SerializableEmptyFLBuffer), ebuf },
                    { typeof(SerializableFromFileFLBuffer), fibuf }
                };

            SerializableFLProgramSerializer prog = new SerializableFLProgramSerializer(bufferParser, iset);
            PluginManager.LoadPlugins(prog);
            Byt3Serializer main = Byt3Serializer.GetDefaultSerializer();
            main.AddSerializer(typeof(SerializableFLProgram), prog);
            main.AddSerializer(typeof(FLFileFormat), new FLFileFormatSerializer());
            return main;
        }


        public static SerializableFLProgram LoadProgram(Stream s, FLInstructionSet iset)
        {
            Byt3Serializer main = CreateLoader(iset);

            if (!main.TryReadPacket(s, out FLFileFormat file))
            {
                throw new FLDeserializationException("Can not parse FL File Format");
            }

            MemoryStream programStream = new MemoryStream(file.Program);

            if (!main.TryReadPacket(programStream, out SerializableFLProgram program))
            {
                throw new FLDeserializationException("Program Data is Corrupt");
            }

            return program;
        }

        public static void SaveProgram(
            Stream s, SerializableFLProgram program, FLInstructionSet iset,
            string[] extraSteps,
            FLProgramHeader programHeader = null)
        {
            Byt3Serializer main = CreateLoader(iset);

            MemoryStream ms = new MemoryStream();

            if (!main.TryWritePacket(ms, program))
            {
                throw new FLDeserializationException("Can not parse stream");
            }

            if (programHeader == null)
            {
                programHeader = new FLProgramHeader("Program", "NONE", Version.Parse("0.0.0.1"));
            }

            FLHeader header = new FLHeader(
                                           FLVersions.HeaderVersion,
                                           FLVersions.SerializationVersion,
                                           FLVersions.CommonVersion,
                                           extraSteps
                                          );

            byte[] p = ms.ToArray();

            ms.Close();

            FLFileFormat file = new FLFileFormat(header, programHeader, p);

            if (!main.TryWritePacket(s, file))
            {
                throw new FLDeserializationException("Can not parse stream");
            }
        }

    }
}