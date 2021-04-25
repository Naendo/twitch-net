namespace TwitchWrapper.Core.Commands
{
    internal class UserStateCommand : ICommand
    {
        private readonly string _channel;

        public UserStateCommand(string channel)
        {
            _channel = channel.ToLower();
        }

        string ICommand.Parse()
        {
            return $":tmi.twitch.tv USERSTATE #{_channel}";
        }
    }
}