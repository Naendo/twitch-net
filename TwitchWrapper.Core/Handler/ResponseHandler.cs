using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core
{
    internal class ResponseHandler
    {
        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!
        //> :<user>!<user>@<user>.tmi.twitch.tv PART #<channel>
        //> :<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message
        //> :<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>
        internal IResponse? DeterminedResponseType(string response)
        {
            //data[1] = ResponseType on Commands
            var data = response.Split(' ');

            if (data[0] == "PING")
                return new PongResponse();
            if (data[1] == "JOIN")
                return new JoinResponse(response);
            
            return data[2] == "PRIVMSG" ? new MessageResponse(response) : null;
        }
    }
}