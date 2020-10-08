using System;
using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core.Commands
{
    internal class ResponseHandler
    {
        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!
        //> :<user>!<user>@<user>.tmi.twitch.tv PART #<channel>
        //> :<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message
        //> :<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>
        internal IResponse DeterminedResponseType(string response)
        {
            try
            {
                //data[1] = ResponseType on Commands
                var data = response.Split(' ');

                return data[0] switch
                {
                    ":tmi.twitch.tv" => new AuthenticationResponse(response),
                    "PING" => new PongResponse(),
                    _ => data[1].ToUpper() switch
                    {
                        "PRIVMSG" => new MessageResponse(response),
                        "PART" => new PartResponse(response),
                        "JOIN" => new JoinResponse(response),
                        _ => null
                    }
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}