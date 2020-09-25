using System;
using System.Linq;

using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.Parsing.StageResults
{
    public class StaticFunction
    {

        public readonly StaticInstruction[] Body;
        public readonly FLFunctionElementModifiers Modifiers;
        public readonly string Name;

        public StaticFunction(string name, string[] body, string[] modifiers)
        {
            Modifiers = new FLFunctionElementModifiers(name, modifiers);


            Name = name;
            Body = body.Select(
                               x => new StaticInstruction(x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                              )
                       .ToArray();
        }

        public override string ToString()
        {
            return $"{Name}: {Modifiers}";
        }

    }
}