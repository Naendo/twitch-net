namespace TwitchNET.Core.Commands
{
    /// <inheritdoc cref="ICommand"/>
    internal class PongCommand : ICommand
    {
        string ICommand.Parse()
        {
            return "PONG:tmi.twitch.tv";
        }
    }
}