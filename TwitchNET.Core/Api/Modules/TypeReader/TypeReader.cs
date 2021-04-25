using System;

namespace TwitchNET.Modules.TypeReader
{
    public abstract class TypeReader : ITypeReader
    {   public abstract TType ConvertFrom<TType>(string input);
        public abstract object ConvertFrom(Type type, string input);
        
    }
}