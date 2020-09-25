using OpenFL.Core;

namespace OpenFL.Debugging
{
    public interface IProgramDebugger
    {

        bool FollowScripts { get; }

        void OnProgramStart(FLDebuggerEvents.ProgramStartEventArgs args);

        void OnProgramExit(FLDebuggerEvents.ProgramExitEventArgs args);

        void OnSubProgramStart(FLDebuggerEvents.SubProgramStartEventArgs args);

        void OnSubProgramExit(FLDebuggerEvents.SubProgramExitEventArgs args);

        void OnBufferWarm(FLDebuggerEvents.WarmEventArgs args);

        void OnInternalBufferLoad(FLDebuggerEvents.InternalBufferLoadEventArgs args);

        void OnFunctionStepInto(FLDebuggerEvents.FunctionRunEventArgs args);

        void OnInstructionStepInto(FLDebuggerEvents.InstructionRunEventArgs args);

        void AfterFunction(FLDebuggerEvents.FunctionRunEventArgs args);

        void AfterInstruction(FLDebuggerEvents.InstructionRunEventArgs args);

    }
}