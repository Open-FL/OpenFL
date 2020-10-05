using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Core
{
    public static class FLDebuggerEvents
    {

        public delegate void AfterFunction(FLProgram program, FunctionRunEventArgs args);

        public delegate void OnFunctionStepInto(FLProgram program, FunctionRunEventArgs args);

        public delegate void OnInstruction(FLProgram program, InstructionRunEventArgs args);

        public delegate void OnInstructionStepInto(FLProgram program, InstructionRunEventArgs args);

        public delegate void OnInternalBufferLoad(FLProgram program, InternalBufferLoadEventArgs args);

        public delegate void OnProgramExit(FLProgram program, ProgramExitEventArgs args);

        public delegate void OnProgramStart(FLProgram program, ProgramStartEventArgs args);

        public class DebuggerEventArgs
        {

            public DebuggerEventArgs(FLProgram root)
            {
                Root = root;
            }

            public bool Cancel { get; set; }

            public FLProgram Root { get; }

        }

        public class ProgramStartEventArgs : DebuggerEventArgs
        {

            public ProgramStartEventArgs(FLProgram root, IFunction entryFunction, bool warmBuffers) : base(root)
            {
                EntryFunction = entryFunction;
                WarmBuffers = warmBuffers;
            }

            public IFunction EntryFunction { get; }

            public bool WarmBuffers { get; }

        }

        public class ProgramExitEventArgs : DebuggerEventArgs
        {

            public ProgramExitEventArgs(FLProgram root) : base(root)
            {
            }

        }

        public class SubProgramStartEventArgs : ProgramStartEventArgs
        {

            public SubProgramStartEventArgs(
                FLProgram root, ExternalFlFunction externalSymbol, IFunction entryFunction, bool warmBuffers) : base(
                 root,
                 entryFunction,
                 warmBuffers
                )
            {
                ExternalSymbol = externalSymbol;
            }

            public ExternalFlFunction ExternalSymbol { get; }

        }

        public class SubProgramExitEventArgs : ProgramExitEventArgs
        {

            public SubProgramExitEventArgs(FLProgram root, ExternalFlFunction externalSymbol) : base(root)
            {
                ExternalSymbol = externalSymbol;
            }

            public ExternalFlFunction ExternalSymbol { get; }

        }

        public class InternalBufferLoadEventArgs : DebuggerEventArgs
        {

            public InternalBufferLoadEventArgs(FLProgram root, FLBuffer loadedBuffer) : base(root)
            {
                LoadedBuffer = loadedBuffer;
            }

            public FLBuffer LoadedBuffer { get; }

        }

        public class WarmEventArgs : DebuggerEventArgs
        {

            public WarmEventArgs(FLProgram root, IWarmable loadedBuffer) : base(root)
            {
                LoadedBuffer = loadedBuffer;
            }

            public IWarmable LoadedBuffer { get; }

        }

        public class FunctionRunEventArgs : DebuggerEventArgs
        {

            public FunctionRunEventArgs(FLProgram root, FLFunction function) : base(root)
            {
                Function = function;
            }

            public FLFunction Function { get; set; }

        }

        public class InstructionRunEventArgs : FunctionRunEventArgs
        {

            public InstructionRunEventArgs(FLProgram root, FLFunction function, FLInstruction instruction) :
                base(root, function)
            {
                Instruction = instruction;
            }

            public FLInstruction Instruction { get; }

        }

    }
}