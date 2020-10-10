using System.Collections.Generic;

using OpenFL.Core;
using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Debugging
{
    public class FLDebugger : IDebugger
    {

        public delegate IProgramDebugger CreateDebugger(FLProgram program);

        private readonly CreateDebugger debuggerCreator;

        private readonly Dictionary<FLProgram, IProgramDebugger> Debuggers =
            new Dictionary<FLProgram, IProgramDebugger>();

        public FLDebugger(CreateDebugger creator)
        {
            debuggerCreator = creator;
        }

        public void OnProgramStart(FLProgram program, FLDebuggerEvents.ProgramStartEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnProgramStart(args);
            }
        }

        public void OnProgramExit(FLProgram program, FLDebuggerEvents.ProgramExitEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnProgramExit(args);
                Debuggers.Remove(program);
            }
        }

        public void OnSubProgramStart(FLProgram program, FLDebuggerEvents.SubProgramStartEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnSubProgramStart(args);
                if (Debuggers[program].FollowScripts)
                {
                    Register(args.Root);
                    OnProgramStart(args.Root, args);
                }
            }
        }

        public void OnSubProgramExit(FLProgram program, FLDebuggerEvents.SubProgramExitEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                if (Debuggers[program].FollowScripts)
                {
                    OnProgramExit(args.Root, args);
                }

                Debuggers[program].OnSubProgramExit(args);
            }
        }

        public void OnBufferWarm(FLProgram program, FLDebuggerEvents.WarmEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnBufferWarm(args);
            }
        }

        public void OnInternalBufferLoad(FLProgram program, FLDebuggerEvents.InternalBufferLoadEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnInternalBufferLoad(args);
            }
        }

        public void OnFunctionStepInto(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnFunctionStepInto(args);
            }
        }

        public void OnInstructionStepInto(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnInstructionStepInto(args);
            }
        }

        public void AfterFunction(FLProgram program, FLDebuggerEvents.FunctionRunEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].AfterFunction(args);
            }
        }

        public void AfterInstruction(FLProgram program, FLDebuggerEvents.InstructionRunEventArgs args)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].AfterInstruction(args);
            }
        }

        public void Register(FLProgram program)
        {
            Debuggers.Add(program, debuggerCreator(program));
        }

        public static void Initialize(CreateDebugger debuggerCreator)
        {
            FLDebuggerHelper.AttachDebugger(new FLDebugger(debuggerCreator));
        }

        public static void Start(CLAPI instance, FLProgram program, int width, int height, int depth)
        {
            FLDebuggerHelper.Register(program);
            program.Run(new FLBuffer(instance, width, height, depth, "DebugInput"), true);
            program.FreeResources();
        }

        public static void StartContainer(
            FLDataContainer container, FLProgram program, int width, int height, int depth)
        {
            Start(container.Instance, program, width, height, depth);
        }

    }
}