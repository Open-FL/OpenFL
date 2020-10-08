using System.Collections.Generic;
using System.Linq;

using OpenFL.Core.Exceptions;

using Utility.FastString;

namespace OpenFL.Core.ElementModifiers
{
    public abstract class FLElementModifiers
    {

        protected readonly List<string> Modifiers;

        public FLElementModifiers(string elementName, IEnumerable<string> modifiers)
        {
            ElementName = elementName;
            Modifiers = new List<string>();
            foreach (string modifier in modifiers)
            {
                if (!Modifiers.Contains(modifier))
                {
                    Modifiers.Add(modifier);
                }
            }

            InternalValidate();
        }

        public string ElementName { get; }

        protected abstract string[] ValidKeywords { get; }


        public List<string> GetModifiers()
        {
            return new List<string>(Modifiers);
        }


        public void AddModifier(string mod)
        {
            if (!Modifiers.Contains(mod))
            {
                Modifiers.Add(mod);
                InternalValidate();
            }
        }

        private void InternalValidate()
        {
            for (int i = 0; i < Modifiers.Count; i++)
            {
                if (!ValidKeywords.Contains(Modifiers[i]))
                {
                    throw new FLInvalidFLElementModifierUseException(
                                                                     ElementName,
                                                                     Modifiers[i],
                                                                     "This modifier is not valid on item"
                                                                    );
                }
            }


            Validate();
        }

        protected abstract void Validate();

        public override string ToString()
        {
            return Modifiers.Unpack(" ");
        }

    }
}