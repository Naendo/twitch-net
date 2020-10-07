using System;

namespace TwitchWrapper.Core.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string Command { get; private set; }
        
        public CommandAttribute(string commandName)
        {
            Command = commandName;
        }           
    }
}