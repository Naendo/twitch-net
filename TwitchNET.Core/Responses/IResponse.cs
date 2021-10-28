namespace TwitchNET.Core.Responses
{
    /// <summary>
    /// <see cref="IResponse"/> manages different irc formats.
    /// </summary>
    internal interface IResponse
    {
        /// <summary>
        /// Provides caller with readable Information about the IRC Response
        /// </summary>
        /// <returns>Returns the an readable ResponseModel for <see cref="TwitchCommander"/></returns>
        MessageResponseModel GetResult();
    }
}