using System.Collections.Generic;

using OpenFL.Core.DataObjects.SerializableDataObjects;

namespace OpenFL.Core.Parsing.StageResults
{
    public class LoadSourceStageResult
    {

        public LoadSourceStageResult(
            string filename, List<string> source, bool mainFile, List<EmbeddedKernelData> embedded)
        {
            MainFile = mainFile;
            Filename = filename;
            Source = source;
            AdditionalKernels = embedded;
        }


        public List<EmbeddedKernelData> AdditionalKernels { get; }

        public string Filename { get; }

        public bool MainFile { get; }

        public List<string> Source { get; }

    }
}