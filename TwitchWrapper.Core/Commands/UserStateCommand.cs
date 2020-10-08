namespace TwitchWrapper.Core.Commands
{
    public class UserStateCommand : ICommand
    {
        private readonly string _channel;

        public UserStateCommand(string channel)
        {
            _channel = channel;
        }

        public string Parse()
        {
            return $":tmi.twitch.tv USERSTATE #{_channel}";
        }
    }
}