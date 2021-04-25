using System;

namespace TwitchNET.Modules
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string commandName)
        {
            Command = commandName;
        }

        public string Command { get; }
    }
}