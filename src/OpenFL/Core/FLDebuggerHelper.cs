using System;
using System.Collections.Generic;

using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Debugging;

namespace OpenFL.Core
{
    public static class FLDebuggerHelper
    {

        private static readonly List<IDebugger> Debugger = new List<IDebugger>();

        public static void AttachDebugger(IDebugger debugger)
        {
            if (!Debugger.Contains(debugger))
            {
                Debugger.Add(debugger);
            }
        }

        public static void DetachDebugger(IDebugger debugger)
        {
            if (Debugger.Contains(debugger))
            {
                Debugger.Remove(debugger);
            }
        }

        private static void HandleEventReturn(FLDebuggerEvents.DebuggerEventArgs args)
        {
            if (args.Cancel)
            {
                throw new DebuggerAbortedException("Aborted by User");
            }
        }

        public static void Register(FLProgram program)
        {
            Debugger.ForEach(x => x.Register(program));
        }

        public static void OnProgramStart(FLProgram program, FLDebuggerEvents.ProgramStartEventArgs args)
        {
            Debugger.ForEach(x => x.OnProgramStart(program, args));
            HandleEventReturn(args);
        }

        public static void OnProgramExit(FLProgram program, FLDebuggerEvents.ProgramExitEventArgs args)
        {
            Debugger.ForEach(x => x.OnProgramExit(program, args));
            HandleEventReturn(args);
        }

        public static void OnSubProgramStart(FLProgram program, FLDebuggerEvents.SubProgramStartEventArgs args)
        {
            Debugger.ForEach(x => x.OnSubProgramStart(program, args));
            HandleEventReturn(args);
        }

        public static void OnSubProgramExit(FLProgram program, FLDebuggerEvents.SubProgramExitEventArgs args)
        {
            Debugger.ForEach(x => x.OnSubProgramExit(program, args));
            HandleEventReturn(args);
        }

        public static void OnInstructionStepInto(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args)
        {
            Debugger.ForEach(x => x.OnInstructionStepInto(program, args));
            HandleEventReturn(args);
        }

        public static void OnFunctionStepInto(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args)
        {
            Debugger.ForEach(x => x.OnFunctionStepInto(program, args));
            HandleEventReturn(args);
        }

        public static void AfterFunction(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args)
        {
            Debugger.ForEach(x => x.AfterFunction(program, args));
            HandleEventReturn(args);
        }

        public static void AfterInstruction(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args)
        {
            Debugger.ForEach(x => x.AfterInstruction(program, args));
            HandleEventReturn(args);
        }

        public static void OnInternalBufferLoad(FLProgram program, FLDebuggerEvents.InternalBufferLoadEventArgs args)
        {
            Debugger.ForEach(x => x.OnInternalBufferLoad(program, args));
            HandleEventReturn(args);
        }

        public static void OnBufferWarm(FLProgram program, FLDebuggerEvents.WarmEventArgs args)
        {
            Debugger.ForEach(x => x.OnBufferWarm(program, args));
            HandleEventReturn(args);
        }

        public class DebuggerAbortedException : Exception
        {

            public DebuggerAbortedException(string message) : this(message, null)
            {
            }

            public DebuggerAbortedException(string message, Exception inner) : base(message, inner)
            {
            }

        }

    }
}