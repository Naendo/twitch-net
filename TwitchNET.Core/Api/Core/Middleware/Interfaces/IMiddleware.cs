namespace TwitchNET.Core.Middleware
{
    public interface IMiddleware
    {
        RequestContext Execute(RequestContext context);
    }
}