namespace TwitchNET.Core.Commands
{
    internal class MessageCommand : ICommand
    {
        private readonly string _channel;
        private readonly string _message;


        //:<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>;
        public MessageCommand(string message, string channel)
        {
            _message = message;
            _channel = channel.ToLower();
        }

        string ICommand.Parse()
        {
            return $"PRIVMSG #{_channel} :{_message}";
        }
    }
}