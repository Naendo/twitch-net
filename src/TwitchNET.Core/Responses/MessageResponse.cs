using System.Collections.Generic;
using System.Linq;

namespace TwitchNET.Core.Responses
{
    /// <inheritdoc cref="IResponse"/>
    internal class MessageResponse : IResponse
    {
        private readonly MessageResponseModel _response;

        public MessageResponse(string response)
        {
            var tags = new Dictionary<string, string>();

            var split = response.Split(';');
            foreach (var tag in split)
            {
                var splitedTag = tag.Split('=');
                tags.Add(splitedTag[0], splitedTag[1]);
            }


            //Condition necessary because colors are not always set.
            var indexOfSecondHashTag = response.Count(x => x == '#') > 1
                ? response.IndexOf('#', response.IndexOf('#') + 1) + 1
                : response.IndexOf('#') + 1;
            
            
            _response = new MessageResponseModel
            {
                //first ':' to first '!'
                Name = response[(response.IndexOf(':') + 1)..response.IndexOf('!')],
                //second ':' to end
                Message = response[(response.IndexOf(':', response.IndexOf(':') + 1) + 1)..],
                ResponseType = ResponseType.PrivMsg,
                //First '#' to second ':' -1
                Channel = response[indexOfSecondHashTag..response.IndexOf(':', indexOfSecondHashTag)]
            };

            if (tags.TryGetValue("badges", out var badge))
            {
                if (badge is "")
                    return;

                var userBadges = badge.Split(',').Select(x => x[..x.IndexOf('/')]).ToList();

                foreach (var userBadge in userBadges)
                    if (userBadge == "broadcaster")
                        _response.IsBroadcaster = true;
                    else if (userBadge == "vip")
                        _response.IsVip = true;
                    else if (userBadge == "moderator")
                        _response.IsModerator = true;
                    else if (userBadge == "subscriber")
                        _response.IsSubscriber = true;
            }

            if (tags.TryGetValue("color", out var color))
                _response.Color = color;

            if (tags.TryGetValue("turbo", out var turbo))
                _response.IsTurbo = turbo != "0";
        }

        // :<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message @badge-info=subscriber/14;badges=broadcaster/1,subscriber/3012;client-nonce=725174698bf0ca4bc484d300c92494c4;color=#FFFFFF;display-name=ThatNandoTho;emotes=;flags=;id=0227c264-d0c8-4a67-b5a2-dd77bda2c1b0;mod=0;room-id=170382741;subscriber=1;tmi-sent-ts=1602178847063;turbo=0;user-id=170382741;user-type= :thatnandotho!thatnandotho@thatnandotho.tmi.twitch.tv PRIVMSG #thatnandotho :asd

        ///<example>PRIVMSG Response Template:</example>
        /// <inheritdoc/>
        MessageResponseModel IResponse.GetResult()
        {
            return _response;
        }
    }
}