using System.Collections.Generic;
using System.Linq;

using OpenFL.Core.Buffers.BufferCreators;
using OpenFL.Core.Instructions.InstructionCreators;

using PluginSystem.Core;
using PluginSystem.Core.Interfaces;
using PluginSystem.Core.Pointer;

using Utility.ObjectPipeline;

namespace OpenFL.Core.ProgramChecks
{
    public class FLProgramCheckBuilder : IPluginHost
    {

        public FLProgramCheckBuilder(FLInstructionSet iset, BufferCreator bc)
        {
            ProgramChecks = new List<FLProgramCheck>();
            InstructionSet = iset;
            BufferCreator = bc;
            StartProfile = FLProgramCheckType.None;
            PluginManager.LoadPlugins(this);
        }

        public FLProgramCheckBuilder(
            FLInstructionSet iset, BufferCreator bc,
            FLProgramCheckType profile = FLProgramCheckType.InputValidation)
        {
            StartProfile = profile;
            ProgramChecks = new List<FLProgramCheck>();


            ProgramChecks.Sort((x, y) => y.Priority.CompareTo(x.Priority));
            InstructionSet = iset;
            BufferCreator = bc;
            PluginManager.LoadPlugins(this);
        }

        public FLProgramCheckType StartProfile { get; }

        public FLInstructionSet InstructionSet { get; }

        public BufferCreator BufferCreator { get; }

        public List<FLProgramCheck> ProgramChecks { get; set; }

        public bool IsAttached { get; private set; }

        public Pipeline AttachedPipeline { get; private set; }

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

        public static FLProgramCheckBuilder CreateDefaultCheckBuilder(
            FLInstructionSet iset, BufferCreator bc,
            FLProgramCheckType profile = FLProgramCheckType.InputValidation)
        {
            return new FLProgramCheckBuilder(iset, bc, profile);
        }

        public void AddProgramCheck(FLProgramCheck check)
        {
            if (IsAttached || ProgramChecks.Any(x => x.GetType() == check.GetType()))
            {
                return;
            }

            if (!ProgramChecks.Contains(check))
            {
                ProgramChecks.Add(check);
                ProgramChecks.Sort((x, y) => y.Priority.CompareTo(x.Priority));
            }
        }

        public void RemoveAllProgramChecks()
        {
            if (IsAttached)
            {
                return;
            }

            ProgramChecks.Clear();
        }

        public void RemoveProgramCheck(FLProgramCheck check)
        {
            if (IsAttached)
            {
                return;
            }

            ProgramChecks.Remove(check);
        }

        public bool Attach(Pipeline target, bool verify)
        {
            if (IsAttached)
            {
                return false;
            }

            for (int i = 0; i < ProgramChecks.Count; i++)
            {
                ProgramChecks[i].SetValues(InstructionSet, BufferCreator, target);
            }

            foreach (FLProgramCheck flProgramCheck in ProgramChecks)
            {
                target.InsertAtFirstValidIndex(flProgramCheck);
            }

            AttachedPipeline = target;
            IsAttached = true;
            return !verify || target.Verify();
        }

        public bool Detach(bool verify)
        {
            if (AttachedPipeline == null)
            {
                return false;
            }

            foreach (FLProgramCheck flProgramCheck in ProgramChecks)
            {
                AttachedPipeline.RemoveSubStage(flProgramCheck);
            }

            Pipeline p = AttachedPipeline;

            IsAttached = false;
            AttachedPipeline = null;
            return !verify || p.Verify();
        }

    }
}