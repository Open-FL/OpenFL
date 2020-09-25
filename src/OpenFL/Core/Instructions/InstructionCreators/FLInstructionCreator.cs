﻿using System;

using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Instructions.InstructionCreators
{
    public abstract class FLInstructionCreator : IDisposable
    {

        public abstract string[] InstructionKeys { get; }

        public virtual bool AllowStaticUse => true;

        public virtual void Dispose()
        {
        }

        public abstract FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction);

        public virtual string GetArgumentSignatureForInstruction(string instruction)
        {
            return null;
        }

        public virtual string GetDescriptionForInstruction(string instruction)
        {
            return "No Description Provided";
        }

        public abstract bool IsInstruction(string key);

    }
}