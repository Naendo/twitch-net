﻿namespace TwitchWrapper.Core.Commands
{
    //JOIN #<channel>
    public class JoinCommand : ICommand
    {
        private readonly string _channel;

        public JoinCommand(string channel)
        {
            _channel = channel.ToLower();
        }

        public string Parse()
        {
            return $"JOIN #{_channel}";
        }
    }
}