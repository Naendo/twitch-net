﻿using System;

namespace TwitchWrapper.Core.Commands
{
    public class MessageCommand : ICommand
    {
        private readonly string _message;
        private readonly string _channel;


        //:<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>;
        public MessageCommand(string message, string channel)
        {
            _message = message;
            _channel = channel.ToLower();
        }

        public string Parse()
        {
            return $"PRIVMSG #{_channel} :{_message}";
        }
    }
}