using System;

namespace TwitchNET.Core.Modules
{
    /// <summary>
    /// Marks a <see cref="BaseModule"/> Member as a command
    /// </summary>
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string commandName)
        {
            Command = commandName;
        }

        /// <summary>
        /// Containing the Command-Key
        /// </summary>
        public string Command { get; }
    }
}