﻿using System;
using System.Collections.Generic;

using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Instructions.InstructionCreators
{
    public class DefaultInstructionCreator : FLInstructionCreator
    {

        private readonly string argumentSignature;
        private readonly string instructionDescription;
        private readonly string instructionKey;
        private readonly Type type;

        public DefaultInstructionCreator(
            string key, Type instructionType, string signature = null,
            string description = null, bool allowStaticUse = true)
        {
            AllowStaticUse = allowStaticUse;
            instructionDescription = description;
            argumentSignature = signature;
            instructionKey = key;
            type = instructionType;
        }

        public override string[] InstructionKeys => new[] { instructionKey };

        public override bool AllowStaticUse { get; }

        public override string GetArgumentSignatureForInstruction(string instruction)
        {
            return argumentSignature;
        }

        public override string GetDescriptionForInstruction(string instruction)
        {
            return instructionDescription ?? base.GetDescriptionForInstruction(instruction);
        }

        public override bool IsInstruction(string key)
        {
            return key == instructionKey;
        }

        public override FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction)
        {
            List<FLInstructionArgument> args = new List<FLInstructionArgument>();

            for (int i = 0; i < instruction.Arguments.Count; i++)
            {
                FLInstructionArgument arg = new FLInstructionArgument(instruction.Arguments[i].GetValue(script, func));
                args.Add(arg);
            }

            return (FLInstruction) Activator.CreateInstance(type, args);
        }

    }

    public class DefaultInstructionCreator<T> : DefaultInstructionCreator
        where T : FLInstruction
    {

        public DefaultInstructionCreator(
            string key, string signature = null, string description = null,
            bool allowStaticUse = true) : base(key, typeof(T), signature, description, allowStaticUse)
        {
        }

    }
}