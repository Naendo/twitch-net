using System;

namespace TwitchNET.Core.Commands
{
    internal class TagCapabilityCommand : ICommand
    {
        string ICommand.Parse()
        {
            return $"CAP REQ :twitch.tv/tags {Environment.NewLine}";
        }
    }
}