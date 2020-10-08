using System;

namespace TwitchWrapper.Core.Commands
{
    public class AuthenticateCommand : ICommand
    {
        private readonly string _nick;
        private readonly string _token;


        public AuthenticateCommand(string nick, string token)
        {
            _nick = nick.ToLower();
            _token = token.StartsWith("oauth:") ? token[6..] : token;
        }

        public string Parse()
        {
            return $"PASS oauth:{_token}{Environment.NewLine}" +
                   $"NICK {_nick}";
        }
    }
}