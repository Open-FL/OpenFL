using System;

using OpenFL.Core;
using OpenFL.Core.Buffers;
using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;
using OpenFL.Core.ProgramChecks;
using OpenFL.Parsing;

namespace OpenFL
{
    public class FLDataContainer : IDisposable
    {

        public readonly BufferCreator BufferCreator;


        public readonly CLAPI Instance;
        public readonly FLInstructionSet InstructionSet;
        public readonly FLParser Parser;
        public SerializableFLProgram SerializedProgram;

        public FLDataContainer()
        {
            Instance = CLAPI.GetInstance();
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(Instance, "assets/kernel");
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            Parser = new FLParser(InstructionSet, BufferCreator, WorkItemRunnerSettings.Default);
        }

        public FLDataContainer(
            CLAPI instance, FLInstructionSet iset, BufferCreator creator, FLParser parser)
        {
            Instance = instance;

            //KernelDB = db;
            InstructionSet = iset;
            BufferCreator = creator;
            Parser = parser;
        }

        public FLProgramCheckBuilder CheckBuilder { get; private set; }

        public void Dispose()
        {
            InstructionSet.Dispose();
            Instance.Dispose();
        }

        public void SetCheckBuilder(FLProgramCheckBuilder builder)
        {
            CheckBuilder = builder;
        }

        public FLBuffer CreateBuffer(int width, int height, int depth, string name, bool optimize = false)
        {
            return new FLBuffer(Instance, width, height, depth, name, MemoryFlag.ReadWrite, optimize);
        }

    }
}