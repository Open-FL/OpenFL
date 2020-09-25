using System.Collections.Generic;

namespace OpenFL.Core.Parsing.StageResults
{
    public class LoadSourceStageResult
    {

        public LoadSourceStageResult(string filename, List<string> source, bool mainFile)
        {
            MainFile = mainFile;
            Filename = filename;
            Source = source;
        }

        public string Filename { get; }

        public bool MainFile { get; }

        public List<string> Source { get; }

    }
}