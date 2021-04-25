namespace TwitchWrapper.Core.Commands
{
    internal class PongCommand : ICommand
    {
        string ICommand.Parse()
        {
            return $"PONG:tmi.twitch.tv";
        }
    }
}