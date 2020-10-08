using System.Collections.Generic;
using System.Linq;

using OpenFL.Core;
using OpenFL.Core.Parsing.StageResults;

using Utility.ADL;
using Utility.ExtPP.API;
using Utility.ObjectPipeline;

namespace OpenFL.Parsing.Stages
{
    public class LoadSourceStage : PipelineStage<FLParserInput, LoadSourceStageResult>
    {

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSrc");

        public override LoadSourceStageResult Process(FLParserInput input)
        {
            if (input.Source != null)
            {
                return new LoadSourceStageResult(
                                                 input.Filename,
                                                 input.Source.ToList(),
                                                 input.MainFile,
                                                 input.KernelData
                                                );
            }

            Logger.Log(LogType.Log, "Loading Source: " + input.Filename, 1);

            Dictionary<string, bool> defines = input.Defines;


            return new LoadSourceStageResult(
                                             input.Filename,
                                             TextProcessorAPI.PreprocessLines(input.Filename, defines).ToList(),
                                             input.MainFile,
                                             input.KernelData
                                            );
        }

    }
}