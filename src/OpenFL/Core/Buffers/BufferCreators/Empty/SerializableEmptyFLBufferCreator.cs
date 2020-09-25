﻿using System;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.Buffers.BufferCreators.BuiltIn.Empty
{
    public class SerializableEmptyFLBufferCreator : ASerializableBufferCreator
    {

        public override SerializableFLBuffer CreateBuffer(
            string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            if (modifiers.IsArray)
            {
                if (arraySize <= 0)
                {
                    throw new InvalidOperationException(
                                                        $"Empty Array buffer \"{name}\" has to be initialized with a size as the first argument"
                                                       );
                }

                return new SerializableEmptyFLBuffer(name, arraySize, modifiers);
            }

            return new SerializableEmptyFLBuffer(name, modifiers);
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "empty";
        }

    }
}