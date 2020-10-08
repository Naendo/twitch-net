namespace TwitchWrapper.Core.Responses
{
    public class PongResponse : IResponse
    {
        //PING :tmi.twitch.tv


        MessageResponseModel IResponse.MapResponse()
        {
            return new MessageResponseModel()
            {
                Message = "PONG :tmi.twitch.tv",
                ResponseType = ResponseType.Ping
            };
        }
    }
}