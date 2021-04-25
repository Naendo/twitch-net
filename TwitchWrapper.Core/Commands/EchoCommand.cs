namespace TwitchWrapper.Core.Commands
{
    internal class EchoCommand : ICommand
    {
        string ICommand.Parse()
        {
            return $"ECHO";
        }
    }
}