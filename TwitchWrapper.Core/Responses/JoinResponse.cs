namespace TwitchWrapper.Core.Responses
{
    internal class JoinResponse : IResponse
    {
        private readonly string _userName;
        private readonly string _message;

        public JoinResponse(string response)
        {
            if (!response.Contains('!'))
                return;

            _userName = response[1..response.IndexOf('!')];
            _message = response[response.IndexOf(' ')..];
        }

        //:<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>
        MessageResponseModel IResponse.MapResponse()
        {
            return new MessageResponseModel()
            {
                Message = _message,
                ResponseType = ResponseType.Join,
                Name = _userName
            };
        }
    }
}