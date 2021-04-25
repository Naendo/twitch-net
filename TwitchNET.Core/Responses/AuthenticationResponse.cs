namespace TwitchNET.Core.Responses
{
    internal class AuthenticationResponse : IResponse
    {
        private readonly string _user;
        private readonly string _message;

        public AuthenticationResponse(string response)
        {
            var data = response.Split(' ');
            _user = data[2];
            _message = data[3][1..];
        }

        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!

        MessageResponseModel IResponse.GetResult()
        {
            return new MessageResponseModel(){
                Message = _message,
                ResponseType = ResponseType.Authenticate,
                Name = _user
            };
        }
    }
}