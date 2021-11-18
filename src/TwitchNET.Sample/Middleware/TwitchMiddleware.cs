using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Middleware;

namespace TwitchNET.Sample.Middleware
{
    public class TwitchMiddleware : IMiddleware
    {
        public RequestContext Execute(RequestContext context)
        {
            context.Parameters[0] += "[this was added by middleware]";

            return context;
        }
    }
}