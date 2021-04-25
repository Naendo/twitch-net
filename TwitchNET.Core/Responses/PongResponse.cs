namespace TwitchNET.Core.Responses
{
    public class PongResponse : IResponse
    {
        //PING :tmi.twitch.tv


        MessageResponseModel IResponse.GetResult()
        {
            return new(){
                Message = "PONG :tmi.twitch.tv",
                ResponseType = ResponseType.Ping
            };
        }
    }
}