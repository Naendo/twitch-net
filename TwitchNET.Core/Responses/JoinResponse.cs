namespace TwitchNET.Core.Responses
{
    /// <inheritdoc cref="IResponse"/>
    internal class JoinResponse : IResponse
    {
        private readonly string _message;
        private readonly string _userName;

        public JoinResponse(string response)
        {
            if (!response.Contains('!'))
                return;

            _userName = response[1..response.IndexOf('!')];
            _message = response[response.IndexOf(' ')..];
        }

        //:<user>!<user>@<user>.tmi.twitch.tv JOIN #<channel>

        /// <inheritdoc/>
        MessageResponseModel IResponse.GetResult()
        {
            return new(){
                Message = _message,
                ResponseType = ResponseType.Join,
                Name = _userName
            };
        }
    }
}