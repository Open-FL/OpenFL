using System.Collections.Generic;

using OpenFL.Core.ProgramChecks;

namespace OpenFL.Core.Parsing.StageResults
{
    public class StaticInspectionResult : FLPipelineResult
    {

        private ImportOptions Options;

        public StaticInspectionResult(
            string filename, List<string> source, List<StaticFunction> functions,
            DefineStatement[] definedBuffers, DefineStatement[] definedScripts, ImportOptions options)
        {
            Options = options;
            Filename = filename;
            Source = source;
            Functions = functions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
        }

        public string Filename { get; }

        public List<string> Source { get; }

        public DefineStatement[] DefinedBuffers { get; }

        public List<StaticFunction> Functions { get; }

        public DefineStatement[] DefinedScripts { get; }

    }
}