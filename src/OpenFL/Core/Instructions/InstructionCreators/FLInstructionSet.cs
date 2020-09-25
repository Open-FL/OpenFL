using System;
using System.Collections.Generic;
using System.Reflection;

using OpenCL.Wrapper;
using OpenCL.Wrapper.TypeEnums;

using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Exceptions;

using PluginSystem.Core;
using PluginSystem.Core.Interfaces;
using PluginSystem.Core.Pointer;

namespace OpenFL.Core.Instructions.InstructionCreators
{
    public class FLInstructionSet : IDisposable, IPluginHost
    {

        private FLInstructionSet(KernelDatabase db)
        {
            Database = db;
        }
        private readonly List<FLInstructionCreator> creators = new List<FLInstructionCreator>();
        public readonly KernelDatabase Database;

        public int CreatorCount => creators.Count;

        public void Dispose()
        {
            for (int i = 0; i < creators.Count; i++)
            {
                creators[i].Dispose();
            }

            creators.Clear();
        }

        public static FLInstructionSet CreateWithBuiltInTypes(KernelDatabase db)
        {
            FLInstructionSet iset = new FLInstructionSet(db);
            PluginManager.LoadPlugins(iset);
            return iset;
        }

        public static FLInstructionSet CreateWithBuiltInTypes(CLAPI instance, string clKernelPath)
        {
            KernelDatabase db =
                new KernelDatabase(instance, clKernelPath, DataVectorTypes.Uchar1);

            return CreateWithBuiltInTypes(db);
        }

        public FLInstructionCreator GetCreatorAt(int idx)
        {
            if (idx >= 0 && idx < creators.Count)
            {
                return creators[idx];
            }

            throw new IndexOutOfRangeException();
        }

        public string[] GetInstructionNames()
        {
            List<string> keys = new List<string>();
            for (int i = 0; i < creators.Count; i++)
            {
                keys.AddRange(creators[i].InstructionKeys);
            }

            return keys.ToArray();
        }

        public bool HasInstruction(string key)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(key))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddInstructionWithDefaultCreator<T>(
            string key, string signature = null, string description = null,
            bool allowStaticUse = true) where T : FLInstruction
        {
            AddInstruction(new DefaultInstructionCreator<T>(key, signature, description, allowStaticUse));
        }

        public void AddInstruction(FLInstructionCreator creator)
        {
            if (creator == null)
            {
                throw new FLInstructionCreatorIsNullException("Trying to add an Instruction container that is null");
            }

            creators.Add(creator);
        }

        public FLInstructionCreator GetCreator(SerializableFLInstruction instruction)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                if (creators[i].IsInstruction(instruction.InstructionKey))
                {
                    return creators[i];
                }
            }

            throw new FLInstructionCreatorNotFoundException(
                                                           $"Could not find an Instruction called '{instruction.InstructionKey}'"
                                                           );
        }

        public FLInstruction Create(FLProgram script, FLFunction func, SerializableFLInstruction instruction)
        {
            return GetCreator(instruction).Create(script, func, instruction);
        }


        public void AddInstructionCreatorsFromAssembly(Assembly asm)
        {
            Type[] ts = asm.GetExportedTypes();
            Type target = typeof(FLInstructionCreator);
            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    FLInstructionCreator creator = (FLInstructionCreator) Activator.CreateInstance(ts[i]);
                    AddInstruction(creator);
                }
            }
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

    }
}