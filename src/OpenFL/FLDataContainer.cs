using System;

using OpenCL.Wrapper;

using OpenFL.Core;
using OpenFL.Core.Buffers;
using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;
using OpenFL.Core.ProgramChecks;
using OpenFL.Parsing;
using OpenFL.ResourceManagement;

namespace OpenFL
{
    public class FLDataContainer : IDisposable
    {

        public readonly BufferCreator BufferCreator;
        public FLProgramCheckBuilder CheckBuilder { get; private set; }
        public readonly CLAPI Instance;
        public readonly FLInstructionSet InstructionSet;
        public readonly FLParser Parser;
        public SerializableFLProgram SerializedProgram;


        private readonly FL2FLCUnpacker fl2flc;
        private readonly FL2TexUnpacker fl2tex;
        private readonly FLC2TexUnpacker flc2tex;
        private readonly FLRESUnpacker fl2res;

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

        public void SetCheckBuilder(FLProgramCheckBuilder builder)
        {
            CheckBuilder = builder;
        }

        public FLBuffer CreateBuffer(int width, int height, int depth, string name)
        {

            return new FLBuffer(Instance, width, height, depth, name);


        }

        public void Dispose()
        {
            InstructionSet.Dispose();
            Instance.Dispose();
        }

    }
}