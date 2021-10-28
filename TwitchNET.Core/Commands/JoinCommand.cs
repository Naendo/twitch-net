namespace TwitchNET.Core.Commands
{
    /// <inheritdoc cref="ICommand"/>
    internal class JoinCommand : ICommand
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