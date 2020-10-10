using System;
using System.Collections.Generic;
using System.Reflection;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Exceptions;

namespace OpenFL.Core.Buffers.BufferCreators
{
    public class BufferCreator : IPluginHost
    {

        private readonly List<ASerializableBufferCreator> bufferCreators = new List<ASerializableBufferCreator>();

        private BufferCreator()
        {
        }

        public bool IsAllowedPlugin(IPlugin plugin)
        {
            return true;
        }

        public void OnPluginLoad(IPlugin plugin, BasePluginPointer ptr)
        {
        }

        public void OnPluginUnload(IPlugin plugin)
        {
        }


        public static BufferCreator CreateWithBuiltInTypes()
        {
            BufferCreator bc = new BufferCreator();
            bc.AddBufferCreatorsInAssembly(Assembly.GetExecutingAssembly());
            PluginManager.LoadPlugins(bc);
            return bc;
        }

        public void AddBufferCreator(ASerializableBufferCreator bufferCreator)
        {
            bufferCreators.Add(bufferCreator);
        }

        public void AddBufferCreatorsInAssembly(Assembly asm)
        {
            Type[] ts = asm.GetExportedTypes();

            Type target = typeof(ASerializableBufferCreator);

            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    ASerializableBufferCreator bc = (ASerializableBufferCreator) Activator.CreateInstance(ts[i]);
                    AddBufferCreator(bc);
                }
            }
        }

        public SerializableFLBuffer Create(
            string key, string name, string[] arguments, FLBufferModifiers modifiers,
            int size)
        {
            for (int i = 0; i < bufferCreators.Count; i++)
            {
                if (bufferCreators[i].IsCorrectBuffer(key))
                {
                    return bufferCreators[i].CreateBuffer(name, arguments, modifiers, size);
                }
            }

            throw new FLBufferCreatorNotFoundException($"'{key}' is not a valid Buffer Creator Name");
        }

    }
}