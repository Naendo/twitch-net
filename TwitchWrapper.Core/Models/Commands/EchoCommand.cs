namespace TwitchWrapper.Core.Commands
{
    internal class EchoCommand : ICommand
    {
        public string Parse()
        {
            return $"ECHO";
        }
    }
}