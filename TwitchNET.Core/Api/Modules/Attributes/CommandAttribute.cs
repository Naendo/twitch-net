using System;

namespace TwitchNET.Modules
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