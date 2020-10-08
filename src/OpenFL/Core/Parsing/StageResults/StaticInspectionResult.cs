using System.Collections.Generic;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ProgramChecks;

namespace OpenFL.Core.Parsing.StageResults
{
    public class StaticInspectionResult : FLPipelineResult
    {

        private ImportOptions Options;

        public StaticInspectionResult(
            string filename, List<string> source, List<StaticFunction> functions,
            DefineStatement[] definedBuffers, DefineStatement[] definedScripts, ImportOptions options,
            List<EmbeddedKernelData> embedded)
        {
            Options = options;
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
            KernelData = embedded;
        }

        public string Filename { get; }

        public List<string> Source { get; }

        public DefineStatement[] DefinedBuffers { get; }

        public List<StaticFunction> Functions { get; }

        public DefineStatement[] DefinedScripts { get; }

        public List<EmbeddedKernelData> KernelData { get; }

    }
}