namespace TwitchNET.Core.Responses
{
    /// <inheritdoc/>
    internal class AuthenticationResponse : IResponse
    {
        private readonly string _message;
        private readonly string _user;

        public AuthenticationResponse(string response)
        {
            var data = response.Split(' ');
            _user = data[2];
            _message = data[3][1..];
        }

        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!

        /// <inheritdoc/>
        MessageResponseModel IResponse.GetResult()
        {
            return new(){
                Message = _message,
                ResponseType = ResponseType.Authenticate,
                Name = _user
            };
        }
    }
}