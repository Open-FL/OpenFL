using OpenCL.Memory;
using OpenCL.Wrapper;

using OpenFL.Core;
using OpenFL.Core.Buffers;
using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;
using OpenFL.Core.Parsing.StageResults;
using OpenFL.Core.ProgramChecks;
using OpenFL.Parsing;

namespace OpenFL
{
    public class FLRunner
    {

        public FLRunner(
            CLAPI instance, FLInstructionSet instructionSet, BufferCreator bufferCreator,
            FLProgramCheckBuilder checkPipeline)
        {
            InstructionSet = instructionSet;
            BufferCreator = bufferCreator;
            Parser = new FLParser(InstructionSet, BufferCreator);
            checkPipeline.Attach(Parser, true);
            Instance = instance;
        }

        public FLRunner(
            FLInstructionSet instructionSet, BufferCreator bufferCreator,
            FLProgramCheckBuilder checkPipeline) : this(CLAPI.MainThread, instructionSet, bufferCreator, checkPipeline)
        {
        }

        public FLRunner(CLAPI instance, FLInstructionSet instructionSet, BufferCreator bufferCreator) : this(
                                                                                                             instance,
                                                                                                             instructionSet,
                                                                                                             bufferCreator,
                                                                                                             FLProgramCheckBuilder
                                                                                                                 .CreateDefaultCheckBuilder(
                                                                                                                                            instructionSet,
                                                                                                                                            bufferCreator
                                                                                                                                           )
                                                                                                            )
        {
        }

        public FLRunner(FLInstructionSet instructionSet, BufferCreator bufferCreator) : this(
                                                                                             CLAPI.MainThread,
                                                                                             instructionSet,
                                                                                             bufferCreator
                                                                                            )
        {
        }

        public FLRunner(CLAPI instance, KernelDatabase database) : this(
                                                                        instance,
                                                                        FLInstructionSet.CreateWithBuiltInTypes(
                                                                                                                database
                                                                                                               ),
                                                                        BufferCreator.CreateWithBuiltInTypes()
                                                                       )
        {
        }

        public FLRunner(KernelDatabase database) : this(CLAPI.MainThread, database)
        {
        }

        public FLRunner(CLAPI instance, string kernelPath) : this(
                                                                  FLInstructionSet.CreateWithBuiltInTypes(
                                                                                                          instance,
                                                                                                          kernelPath
                                                                                                         ),
                                                                  BufferCreator.CreateWithBuiltInTypes()
                                                                 )
        {
        }

        public FLRunner(string kernelPath) : this(CLAPI.MainThread, kernelPath)
        {
        }

        public FLInstructionSet InstructionSet { get; }

        public BufferCreator BufferCreator { get; }

        public FLParser Parser { get; }

        public CLAPI Instance { get; }


        public FLProgram Run(string file, int width, int height, int depth)
        {
            return Run(Parser.Process(new FLParserInput(file)), width, height, depth);
        }

        public FLProgram Run(SerializableFLProgram file, int width, int height, int depth)
        {
            return Run(file.Initialize(Instance, InstructionSet), width, height, depth);
        }

        public FLProgram Run(FLProgram file, int width, int height, int depth)
        {
            FLBuffer buffer =
                new FLBuffer(
                             CLAPI.CreateEmpty<byte>(
                                                     Instance,
                                                     height * width * depth * 4,
                                                     MemoryFlag.ReadWrite,
                                                     "FLRunnerExecutionCreatedBuffer"
                                                    ),
                             width,
                             height,
                             depth
                            );
            return Run(file, buffer, true);
        }

        public FLProgram Run(FLProgram file, FLBuffer input, bool makeInternal)
        {
            file.Run(input, makeInternal);
            return file;
        }

        public FLProgram Initialize(SerializableFLProgram file)
        {
            return file.Initialize(Instance, InstructionSet);
        }

    }
}