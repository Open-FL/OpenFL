using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Instructions.InstructionCreators;

namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public class ExternalFlFunction : FLParsedObject, IFunction
    {

        private readonly SerializableFLProgram ExternalFunctionBlueprint;
        private readonly FLInstructionSet InstructionSet;

        public ExternalFlFunction(
            string name, SerializableFLProgram external, FLInstructionSet iset,
            FLExecutableElementModifiers modifiers) : base("ext-func")
        {
            Name = name;
            Modifiers = modifiers;
            ExternalFunctionBlueprint = external;
            InstructionSet = iset;
        }

        public FLExecutableElementModifiers Modifiers { get; }

        public string Name { get; }

        public void Process()
        {
            FLBuffer input = Root.ActiveBuffer;
            FLProgram externalFunction = ExternalFunctionBlueprint.Initialize(Root.Instance, InstructionSet);

            input.SetRoot(externalFunction);

            externalFunction.ActiveChannels = Root.ActiveChannels;
            externalFunction.SetCLVariables(input, false);

            //Not making it internal to the subscript because that would dispose the buffer later in the method
            //FLProgram.Debugger?.SubProgramStarted(Root, this, externalFunction);
            FLDebuggerHelper.OnSubProgramStart(
                                               Root,
                                               new FLDebuggerEvents.SubProgramStartEventArgs(
                                                    externalFunction,
                                                    this,
                                                    externalFunction
                                                        .EntryPoint,
                                                    false
                                                   )
                                              );

            //FLDebuggerHelper.OnProgramStart(externalFunction, new FLDebuggerEvents.ProgramStartEventArgs(externalFunction, externalFunction.EntryPoint, false, false));

            externalFunction.EntryPoint.Process();

            //FLProgram.Debugger?.SubProgramEnded(Root, externalFunction);
            FLDebuggerHelper.OnSubProgramExit(
                                              Root,
                                              new FLDebuggerEvents.SubProgramExitEventArgs(externalFunction, this)
                                             );

            //FLDebuggerHelper.OnProgramExit(externalFunction, new FLDebuggerEvents.ProgramExitEventArgs(externalFunction, false)); //Fire On Program Exit as External Function as well

            FLBuffer buf = externalFunction.ActiveBuffer;

            buf.SetRoot(Root);
            input.SetRoot(Root);
            externalFunction.RemoveFromSystem(buf);
            externalFunction.RemoveFromSystem(input);

            Root.ActiveChannels = externalFunction.ActiveChannels;
            Root.ActiveBuffer = buf;


            externalFunction.FreeResources();
        }


        public override string ToString()
        {
            return $"{FLKeywords.DefineScriptKey} {Name}: [UNKNOWN]";
        }

    }
}