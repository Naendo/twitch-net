using TwitchNET.Modules;

namespace TwitchNET.Core.Middleware
{
    internal sealed class ProxyBuilder : IMiddleware
    {
        public RequestContext Execute(RequestContext context)
        {
           
            context.Endpoint.ChannelProxy = new ChannelProxy{
                Channel = context.IrcResponseModel.Channel
            };
            context.Endpoint.UserProxy = new UserProxy{
                IsBroadcaster = context.IrcResponseModel.IsBroadcaster,
                IsVip = context.IrcResponseModel.IsVip,
                IsModerator = context.IrcResponseModel.IsModerator,
                IsSubscriber = context.IrcResponseModel.IsSubscriber,
                Name = context.IrcResponseModel.Name,
                Color = context.IrcResponseModel.Color
            };
            context.Endpoint.CommandProxy = new CommandProxy{
                Message = context.IrcResponseModel.Message
            };
            context.Endpoint.TwitchBot = context.BotContext;
            
            return context;
        }
    }
}