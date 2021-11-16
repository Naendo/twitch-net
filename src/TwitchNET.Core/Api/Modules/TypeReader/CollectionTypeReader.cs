using System;

namespace TwitchNET.Core.Modules
{
    public class CollectionTypeReader : TypeReaderBase
    {
        public override object ConvertFrom(Type type, string input)
        {
            return null;
        }

        public override TType ConvertFrom<TType>(string input)
        {
            return (TType) new object();
        }
    }
}