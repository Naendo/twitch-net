using System;

namespace TwitchNET.Modules.TypeReader
{
    public abstract class TypeReader : ITypeReader
    {
        public abstract object ConvertFrom(Type type, string input);
        public abstract TType ConvertFrom<TType>(string input);
    }
}