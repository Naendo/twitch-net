using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core
{
    internal class ResponseHandler
    {
        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!
        //> :<user>!<user>@<user>.tmi.twitch.tv PART #<channel>
        //> :<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message
        //> :<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>
        internal IResponse DeterminedResponseType(string response)
        {
            //data[1] = ResponseType on Commands
            var data = response.Split(' ');

            if (data[0] == "PING")
                return new PongResponse();
            if (data[1] == "JOIN")
                return new JoinResponse(response);
            if (data[2] == "PRIVMSG")
                return new MessageResponse(response);
            if (data[1] == "NOTICE" && data[4] == "authentication")
                throw new IrcClientException("IRC-Authentication failed!");

            return null;
        }
    }
}