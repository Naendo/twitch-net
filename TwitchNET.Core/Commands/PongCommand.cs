namespace TwitchNET.Core.Commands
{
    internal class PongCommand : ICommand
    {
        string ICommand.Parse()
        {
            return $"PONG:tmi.twitch.tv";
        }
    }
}