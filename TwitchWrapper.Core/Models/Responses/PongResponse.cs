namespace TwitchWrapper.Core.Responses
{
    public class PongResponse : IResponse
    {
        //PING :tmi.twitch.tv


        ResponseModel IResponse.MapResponse()
        {
            return new ResponseModel()
            {
                Message = "PONG :tmi.twitch.tv",
                ResponseType = ResponseType.Ping
            };
        }
    }
}