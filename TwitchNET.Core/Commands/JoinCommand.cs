namespace TwitchNET.Core.Commands
{
    //JOIN #<channel>
    public class JoinCommand : ICommand
    {
        private readonly string _channel;

        public JoinCommand(string channel)
        {
            _channel = channel.ToLower();
        }

        string ICommand.Parse()
        {
            return $"JOIN #{_channel}";
        }
    }
}