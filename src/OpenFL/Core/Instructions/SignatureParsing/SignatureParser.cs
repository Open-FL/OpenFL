﻿using System.Collections.Generic;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ProgramChecks.Checking.Signatures;

namespace OpenFL.Core.Instructions.SignatureParsing
{
    public class SignatureParser
    {

        public static bool ParseCreatorSig(string sig, out List<InstructionArgumentSignature> ret)
        {
            ret = new List<InstructionArgumentSignature>();
            if (sig == null)
            {
                return false;
            }

            string[] overloads = sig.Split('|');

            foreach (string overload in overloads)
            {
                List<InstructionArgumentCategory> signature = new List<InstructionArgumentCategory>();
                for (int i = 0; i < overload.Length; i++)
                {
                    signature.Add(ParseArgument(overload[i]));
                }

                ret.Add(new InstructionArgumentSignature { Signature = signature });
            }

            return true;
        }

        public static InstructionArgumentCategory ParseArgument(char input)
        {
            switch (input)
            {
                case 'N': //Value
                    return InstructionArgumentCategory.Value;
                case 'F': //Function
                    return InstructionArgumentCategory.Function;
                case 'S': //Script
                    return InstructionArgumentCategory.Script;
                case 'B': //Buffer
                    return InstructionArgumentCategory.Buffer;
                case 'E': //Defined Element
                    return InstructionArgumentCategory.DefinedElement;
                case 'X': //Defined Executable(Script|Function)
                    return InstructionArgumentCategory.DefinedFunction;
                case 'I': //Internal Defined Element(E but without Scripts)
                    return InstructionArgumentCategory.InternalDefinedElement;
                case 'D':
                    return InstructionArgumentCategory.Name;
                case 'A':
                    return InstructionArgumentCategory.AllElements;
                case 'V':
                    return InstructionArgumentCategory.NumberResolvable;
                case 'C':
                    return InstructionArgumentCategory.BufferArray;
                default:
                    return InstructionArgumentCategory.Invalid;
            }
        }

    }
}