﻿using System;

using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Core.Arguments
{
    public class SerializeArrayElementArgumentVariableIndex : SerializeArrayElementArgument
    {

        public readonly string Index;

        public SerializeArrayElementArgumentVariableIndex(string value, string index)
        {
            Index = index;
            Value = value;
        }

        public string Value { get; }

        public override string Identifier => Value + $"[{Index}]";

        public override ImplicitCastBox GetValue(FLProgram script, FLFunction func)
        {
            if (script.DefinedBuffers[Value] is IEditableBuffer buffer)
            {
                return new ImplicitCastBox<decimal>(
                                                    () =>
                                                        func.Variables.IsDefined(Index)
                                                            ? buffer.GetData()[(int) func.Variables.GetVariable(Index)]
                                                            : 0
                                                   ); //really slow
            }

            throw new InvalidOperationException($"{script.DefinedBuffers[Value]} does not implement IEditableBuffer");
        }


        public override string ToString()
        {
            return Identifier;
        }

    }
}