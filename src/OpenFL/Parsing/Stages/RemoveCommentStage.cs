﻿using System.Collections.Generic;

using OpenFL.Core;
using OpenFL.Core.Parsing.StageResults;

using Utility.ADL;
using Utility.FastString;
using Utility.ObjectPipeline;

namespace OpenFL.Parsing.Stages
{
    public class RemoveCommentStage : PipelineStage<LoadSourceStageResult, LoadSourceStageResult>
    {

        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(OpenFLDebugConfig.Settings, "LoadSourceStage");

        private readonly FLParser parser;

        public RemoveCommentStage(FLParser parserInstance)
        {
            parser = parserInstance;
        }

        public override LoadSourceStageResult Process(LoadSourceStageResult input)
        {
            Logger.Log(LogType.Log, "Removing Comments.. ", 1);


            WorkItemRunner.RunInWorkItems(input.Source, RemoveCommentTask, parser.WorkItemRunnerSettings);


            Logger.Log(LogType.Log, "Optimizing Script Length..", 1);
            for (int i = input.Source.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(input.Source[i]))
                {
                    input.Source.RemoveAt(i);
                    continue;
                }

                input.Source[i] = input.Source[i].Trim();
            }

            return input;
        }


        private void RemoveCommentTask(List<string> input, int start, int count)
        {
            for (int i = start; i < start + count; i++)
            {
                input[i] = input[i].Trim();

                int idx = FString.FastIndexOf(input[i], FLKeywords.CommentBeginKey);

                if (idx == 0)
                {
                    input[i] = string.Empty;
                }
                else if (idx > 0)
                {
                    input[i] = input[i].Substring(0, idx).Trim();
                }
            }
        }

    }
}