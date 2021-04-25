using System;

namespace TwitchNET.Modules.TypeReader
{
    internal interface ITypeReader
    {
        object ConvertFrom(Type type, string input);
    }
}