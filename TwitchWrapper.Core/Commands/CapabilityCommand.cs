using System;

namespace TwitchWrapper.Core.Commands
{
    public class CapabilityCommand : ICommand
    {
        public string Parse()
        {
            return $"CAP REQ :twitch.tv/commands {Environment.NewLine}" +
                   $"CAP REQ :twitch.tv/tags";
        }
    }
}