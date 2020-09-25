using System.Collections.Generic;
using System.Text;

using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Parsing.StageResults;

namespace OpenFL.Core.DataObjects.SerializableDataObjects
{
    public class SerializableFLFunction : SerializableNamedObject
    {

        public readonly StaticInstruction[] Body;

        public SerializableFLFunction(
            string name, StaticInstruction[] body,
            List<SerializableFLInstruction> instructions, FLFunctionElementModifiers modifiers) : base(name)
        {
            Body = body;
            Modifiers = modifiers;
            Instructions = instructions;
        }

        public List<SerializableFLInstruction> Instructions { get; }

        public FLFunctionElementModifiers Modifiers { get; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name);


            sb.Append(": ");

            sb.AppendLine(Modifiers.ToString());

            for (int i = 0; i < Instructions.Count; i++)
            {
                sb.AppendLine("\t" + Instructions[i]);
            }

            sb.AppendLine();
            return sb.ToString();
        }

    }
}