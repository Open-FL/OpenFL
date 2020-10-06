﻿using System.Collections.Generic;
using System.Linq;

using OpenCL.Wrapper;

using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Instructions.InstructionCreators;

namespace OpenFL.Core
{
    public static class FLInitializationExtensions
    {

        public static FLProgram Initialize(this SerializableFLProgram program, FLDataContainer dataContainer)
        {
            return Initialize(program, dataContainer.Instance, dataContainer.InstructionSet);
        }

        public static FLProgram Initialize(
            this SerializableFLProgram program, CLAPI instance,
            FLInstructionSet instructionSet)
        {
            Dictionary<string, FLBuffer> buffers = new Dictionary<string, FLBuffer>();
            Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>();
            Dictionary<string, IFunction> externalFunctions = new Dictionary<string, IFunction>();


            for (int i = 0; i < program.ExternalFunctions.Count; i++)
            {
                ExternalFlFunction extFunc = new ExternalFlFunction(
                                                                    program.ExternalFunctions[i].Name,
                                                                    program.ExternalFunctions[i].ExternalProgram,
                                                                    instructionSet,
                                                                    program.ExternalFunctions[i].Modifiers
                                                                   );

                externalFunctions.Add(program.ExternalFunctions[i].Name, extFunc);
            }

            for (int i = 0; i < program.DefinedBuffers.Count; i++)
            {
                FLBuffer extFunc = program.DefinedBuffers[i].GetBuffer();
                extFunc.SetKey(program.DefinedBuffers[i].Name);
                buffers.Add(extFunc.DefinedBufferName, extFunc);
            }


            for (int i = 0; i < program.Functions.Count; i++)
            {
                functions.Add(program.Functions[i].Name, new FLFunction(program.Functions[i].Name, program.Functions[i].Modifiers));
            }

            FLProgram p = new FLProgram(instance, externalFunctions, buffers, functions);
            p.SetRoot();
            for (int i = 0; i < program.Functions.Count; i++)
            {
                (functions[program.Functions[i].Name] as FLFunction).Initialize(
                                                                                program.Functions[i],
                                                                                p,
                                                                                instructionSet
                                                                               );
            }

            //TODO Resolve Functions first. then in a second step resolve the references of the arguments.
            //When a function is defined below it beeing used the program is crashing because
            //it resolves the argument before the function that the argument is pointing to is parsed(e.g. not null)
            //Create possibility to create the function objects in another loop than creating the arguments.
            //For functions
            //add the function objects with name to the dict
            //for functions
            //initialize function
            p.SetRoot();
            
			foreach (EmbeddedKernelData embeddedKernelData in program.KernelData)
            {
                if (instructionSet.Database.KernelNames.Contains(embeddedKernelData.Kernel)) continue;

                instructionSet.Database.AddProgram(
                                                   instance,
                                                   embeddedKernelData.Source,
                                                   "./",
                                                   false,
                                                   out CLProgramBuildResult res
                                                  );
                if (!res)
                {
                    throw res.GetAggregateException();
                }

            }

            return p;
        }

        private static void SetRoot(this FLProgram program)
        {
            foreach (KeyValuePair<string, FLBuffer> programDefinedBuffer in program.DefinedBuffers)
            {
                programDefinedBuffer.Value.SetRoot(program);
            }

            foreach (KeyValuePair<string, IFunction> programDefinedScript in program.DefinedScripts)
            {
                programDefinedScript.Value.SetRoot(program);
            }

            foreach (KeyValuePair<string, IFunction> programFlFunction in program.FlFunctions)
            {
                programFlFunction.Value?.SetRoot(program);
            }
        }

        private static void Initialize(
            this FLFunction function, SerializableFLFunction serializableFunction,
            FLProgram script,
            FLInstructionSet instructionSet)
        {
            function.SetInstructions(
                                     serializableFunction.Instructions
                                                         .Select(x => x.Initialize(script, function, instructionSet))
                                                         .ToList()
                                    );
        }

        public static FLInstruction Initialize(
            this SerializableFLInstruction instruction, FLProgram script,
            FLFunction func,
            FLInstructionSet instructionSet)
        {
            FLInstruction i = instructionSet.Create(script, func, instruction);
            return i;
        }

    }
}