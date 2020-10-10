using System.Collections.Generic;
using System.Linq;

using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.ProgramChecks.Checking.Signatures
{
    public class InstructionArgumentSignature
    {

        public List<InstructionArgumentCategory> Signature;

        public bool IsAvailableForStatic()
        {
            for (int i = 0; i < Signature.Count; i++)
            {
                if (Signature[i] == InstructionArgumentCategory.DefinedElement)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Compare(List<InstructionArgumentCategory> sig)
        {
            if (sig.Count != Signature.Count)
            {
                return false;
            }

            bool matches = true;

            for (int i = 0; i < sig.Count; i++)
            {
                matches &= (sig[i] & Signature[i]) != 0;
            }

            return matches;
        }

        public override string ToString()
        {
            if (Signature.Count == 0)
            {
                return "NoSignature";
            }

            return Signature.Select(x => x.ToString()).Unpack("; ");
        }

    }
}