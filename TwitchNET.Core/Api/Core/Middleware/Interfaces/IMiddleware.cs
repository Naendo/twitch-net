using TwitchNET.Core.Middleware;

namespace TwitchNET.Core.Interfaces
{
    public interface IMiddleware
    {
        RequestContext Execute(RequestContext context);
    }
}