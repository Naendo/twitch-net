using TwitchNET.Core.Middleware;

namespace TwitchNET.Core
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder UseMiddleware<TMiddleware>(this RequestBuilder requestBuilder)
            where TMiddleware : IMiddleware
        {
            return requestBuilder.TryRegisterCustomMiddleware<TMiddleware>();
        }

        internal static RequestBuilder UseTypeReader(this RequestBuilder requestBuilder)
        {
            return requestBuilder.TryRegisterMiddleware<TypeReaderBuilder>();
        }

        internal static RequestBuilder UseProxies(this RequestBuilder requestBuilder)
        {
            return requestBuilder.TryRegisterMiddleware<ProxyBuilder>();
        }
    }
}