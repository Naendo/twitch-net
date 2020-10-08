namespace TwitchWrapper.Core.Responses
{
    internal class PartResponse : IResponse
    {
        private readonly string _userName;
        private readonly string _message;

        public PartResponse(string response)
        {
            _userName = response[1..response.IndexOf('!')];
            _message = response[response.IndexOf(' ')..];
        }

        //:<user>!<user>@<user>.tmi.twitch.tv PART #<channel>

        public MessageResponseModel MapResponse()
        {
            return new MessageResponseModel()
            {
                Message = _message,
                ResponseType = ResponseType.Part,
                Name = _userName
            };
        }
    }
}