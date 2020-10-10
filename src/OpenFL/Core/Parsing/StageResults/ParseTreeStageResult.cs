using System.Collections.Generic;

using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Core.Parsing.StageResults
{
    public class ParseTreeStageResult
    {

        public ParseTreeStageResult(
            CLAPI instance, Dictionary<string, FLFunction> definedScripts,
            Dictionary<string, FLBuffer> definedBuffers, FLFunction[] flFunctions)
        {
            Instance = instance;
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public CLAPI Instance { get; }

        //Task:
        //  Replace FLBuffer Class with AParsableBuffer that implements GetBuffer() which returns the FLBuffer object
        //      Because: Serializer can then Serialize the Parsable Buffer
        public Dictionary<string, FLBuffer> DefinedBuffers { get; }

        public FLFunction[] FlFunctions { get; }

        public Dictionary<string, FLFunction> DefinedScripts { get; }

    }
}