namespace TwitchNET.Core.Commands
{
    internal class EchoCommand : ICommand
    {
        string ICommand.Parse()
        {
            return "ECHO";
        }
    }
}