﻿using System;

namespace OpenFL.Serialization.Serializers.Internal.FileFormatSerializer
{
    public class FLProgramHeader
    {

        public FLProgramHeader(string programName, string author, Version version)
        {
            ProgramName = programName;
            Author = author;
            Version = version;
        }

        public string ProgramName { get; }

        public string Author { get; }

        public Version Version { get; }

    }
}