using System;

namespace TwitchNET.Core.Modules
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrefixAttribute : Attribute
    {
        internal string Prefix { get; }

        public PrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }
    }
}