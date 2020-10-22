using System;

namespace TwitchWrapper.Core.Commands
{
    public class TagCapabilityCommand : ICommand
    {
        public string Parse()
        {
            return $"CAP REQ :twitch.tv/tags {Environment.NewLine}";
        }
    }
}