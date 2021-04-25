using System.Collections.Generic;
using System.Reflection;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Modules
{
    internal class CommandInfo
    {
        internal MethodInfo MethodInfo { get; set; }
        internal string CommandKey { get; set; }

        internal IReadOnlyList<ParameterInfo> Parameters { get; set; }
        internal IReadOnlyList<ITypeReader> TypeReaders { get; set; }
    }
}