using System;

namespace TwitchWrapper.Core.Commands
{
    public class CommandManager
    {
        
        public ICommand DetermindCommandType(string response)
        {
            var data = response.Split(' ');

            return data[0].ToUpper() switch
            {
                "PING" => new PongCommand(),
                //:tmi.twitch.tv 001 thatnandotho :Welcome, GLHF!
                ":TMI.TWITCH.TV" => new ConnectedCommand(response),
                _ => new MessageCommand(response),
            };
        }
    }
}