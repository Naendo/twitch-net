using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core.Responses
{
    internal class MessageResponse : IResponse
    {
        private readonly string _userName;
        private readonly string _message;

        public MessageResponse(string response)
        {
            _userName = response[1..response.IndexOf('!')];
            _message = response[response.IndexOf(':')..];
        }

        // :<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message

        ResponseModel IResponse.Parse()
        {
            return new ResponseModel()
            {
                Message = _message,
                ResponseType = ResponseType.PrivMsg,
                UserName = _userName
            };
        }
    }
}