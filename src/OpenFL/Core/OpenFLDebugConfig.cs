using System;
using System.Reflection;

using Utility.ADL;
using Utility.ADL.Configs;

namespace OpenFL.Core
{
    public static class OpenFLDebugConfig
    {

        public static readonly ProjectDebugConfig<LogType, Verbosity> Settings =
            new ProjectDebugConfig<LogType, Verbosity>(
                                                       "FL",
                                                       LogType.All,
                                                       PrefixLookupSettings.AddPrefixIfAvailable |
                                                       PrefixLookupSettings.OnlyOnePrefix
                                                      );

        public static Version CommonVersion => Assembly.GetExecutingAssembly().GetName().Version;

    }
}