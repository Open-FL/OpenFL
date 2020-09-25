using OpenFL.Core;
using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Debugging
{
    public interface IDebugger
    {

        void Register(FLProgram program);

        void OnProgramStart(FLProgram program, FLDebuggerEvents.ProgramStartEventArgs args);

        void OnProgramExit(FLProgram program, FLDebuggerEvents.ProgramExitEventArgs args);

        void OnSubProgramStart(FLProgram program, FLDebuggerEvents.SubProgramStartEventArgs args);

        void OnSubProgramExit(FLProgram program, FLDebuggerEvents.SubProgramExitEventArgs args);

        void OnBufferWarm(FLProgram program, FLDebuggerEvents.WarmEventArgs args);

        void OnInternalBufferLoad(FLProgram program, FLDebuggerEvents.InternalBufferLoadEventArgs args);

        void OnFunctionStepInto(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args);

        void OnInstructionStepInto(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args);

        void AfterFunction(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args);

        void AfterInstruction(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args);

    }
}