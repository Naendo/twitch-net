using System;

namespace TwitchNET.Modules.TypeReader
{
    public interface ITypeReader
    {
        object ConvertFrom(Type type, string input);
    }
}