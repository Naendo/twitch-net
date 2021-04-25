namespace TwitchNET.Core.Responses
{
    internal class PartResponse : IResponse
    {
        private readonly string _message;
        private readonly string _userName;

        public PartResponse(string response)
        {
            _userName = response[1..response.IndexOf('!')];
            _message = response[response.IndexOf(' ')..];
        }

        //:<user>!<user>@<user>.tmi.twitch.tv PART #<channel>

        MessageResponseModel IResponse.GetResult()
        {
            return new(){
                Message = _message,
                ResponseType = ResponseType.Part,
                Name = _userName
            };
        }
    }
}