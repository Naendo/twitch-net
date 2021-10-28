namespace TwitchNET.Core.Commands
{
    /// <inheritdoc cref="ICommand"/>
    internal class EchoCommand : ICommand
    {
        string ICommand.Parse()
        {
            return "ECHO";
        }
    }
}