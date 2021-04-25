using TwitchNET.Core.Responses;
using TwitchNET.Modules;

namespace TwitchNET.Core
{
    public class RequestContext
    {
        internal CommandInfo CommandInfo { get; init; }
        internal BaseModule Endpoint { get; init; }
        internal MessageResponseModel IrcResponseModel { get; init; }
        internal TwitchBot BotContext { get; init; }

        public ParameterCollection Parameters { get; internal set; }

        public string Message => IrcResponseModel.Message;
    }
}