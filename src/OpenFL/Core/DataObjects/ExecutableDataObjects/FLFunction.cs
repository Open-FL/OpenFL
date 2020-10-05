using System.Collections.Generic;

using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Instructions.Variables;

namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public class FLFunction : FLParsedObject, IFunction
    {

        public FLFunction(string name, List<FLInstruction> instructions, FLFunctionElementModifiers mods)
        {
            Name = name;
            Instructions = instructions;
            Modifiers = mods;
        }

        internal FLFunction(string name, FLFunctionElementModifiers mods)
        {
            Name = name;
            Modifiers = mods;
        }

        public FLExecutableElementModifiers Modifiers { get; }

        public List<FLInstruction> Instructions { get; private set; }

        public VariableManager<decimal> Variables { get; private set; }

        public string Name { get; }

        public virtual void Process()
        {
            //FLProgram.Debugger?.ProcessEvent(this);
            FLDebuggerHelper.OnFunctionStepInto(Root, new FLDebuggerEvents.FunctionRunEventArgs(Root, this));
            for (int i = 0; i < Instructions.Count; i++)
            {
                FLDebuggerHelper.OnInstructionStepInto(
                                                       Root,
                                                       new FLDebuggerEvents.InstructionRunEventArgs(
                                                            Root,
                                                            this,
                                                            Instructions[i]
                                                           )
                                                      );

                //FLProgram.Debugger?.ProcessEvent(Instructions[i]);
                Instructions[i].Process();
                FLDebuggerHelper.AfterInstruction(
                                                  Root,
                                                  new FLDebuggerEvents.InstructionRunEventArgs(
                                                       Root,
                                                       this,
                                                       Instructions[i]
                                                      )
                                                 );
            }

            FLDebuggerHelper.AfterFunction(Root, new FLDebuggerEvents.FunctionRunEventArgs(Root, this));
        }

        public override void SetRoot(FLProgram root)
        {
            base.SetRoot(root);

            Variables = root.Variables.AddScope();

            if (Instructions == null)
            {
                return;
            }

            for (int i = 0; i < Instructions.Count; i++)
            {
                if (Instructions[i].Root == root)
                {
                    continue;
                }

                Instructions[i].SetRoot(root);
                Instructions[i].SetParent(this);
            }
        }

        internal void SetInstructions(List<FLInstruction> instructions)
        {
            Instructions = instructions;
        }

    }
}