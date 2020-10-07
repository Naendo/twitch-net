using System;
using System.Linq.Expressions;

namespace TwitchWrapper.Core.Commands
{
    public class MessageCommand : ICommand
    {
        private readonly string _user;
        private readonly string _message;


        //:<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>;
        public MessageCommand(string response)
        {
            try
            {


                _user = response[1..response.IndexOf('!')];
                _message = response[response.IndexOf(' ')..];
            }
            catch(Exception)
            {
                
            }
        }

        public string Parse()
        {
            return $"{_user}: ${_message}";
        }
    }
}