namespace TwitchWrapper.Core.Commands
{
    public class PongCommand : ICommand
    {
        public string Parse()
        {
            return $"PONG:tmi.twitch.tv";
        }
    }
}