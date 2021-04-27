namespace TwitchNET.Core.Responses
{
    /// <inheritdoc cref="IResponse"/>
    public class PongResponse : IResponse
    {
        ///<example>Pong Response Template: PING :tmi.twitch.tv</example>
        /// <inheritdoc/>
        MessageResponseModel IResponse.GetResult()
        {
            return new(){
                Message = "PONG :tmi.twitch.tv",
                ResponseType = ResponseType.Ping
            };
        }
    }
}