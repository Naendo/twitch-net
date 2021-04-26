using System.Net.Sockets;
using TwitchNET.Core.Middleware;

namespace TwitchNET.Samples.Middleware
{
    public class AppendStringToFirstParameterMiddleware : IMiddleware
    {
        public RequestContext Execute(RequestContext context)
        {
            if (context.Parameters[0] is string)
                context.Parameters[0] += "asdf";

            return context;
        }
    }
}