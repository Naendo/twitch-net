using System;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Middleware;

namespace TwitchNET.Core.Extensions
{
    /// <summary>
    /// Extensions for <see cref="MiddlewareBuilder"/>. Use the extension methods to register your custom middleware.
    /// </summary>
    public static class RequestBuilderExtensions
    {
        /// <summary>
        /// Extension methode to register customized <see cref="IMiddleware"/>
        /// </summary>
        /// <typeparam name="TMiddleware">TMiddleware must implement IMiddleware</typeparam>
        public static MiddlewareBuilder UseMiddleware<TMiddleware>(this MiddlewareBuilder middlewareBuilder)
            where TMiddleware : IMiddleware
        {
            return middlewareBuilder.TryRegisterCustomMiddleware<TMiddleware>();
        }

        /// <summary>
        /// Extension methode to register customized <see cref="ITypeReader"/>
        /// </summary>
        /// <typeparam name="TType">Intended output <see cref="Type"/> for customized <see cref="ITypeReader"/></typeparam>
        /// <typeparam name="TTypeReader">Customized TypeReader which implements <see cref="ITypeReader"/></typeparam>
        public static MiddlewareBuilder UseTypeReader<TType, TTypeReader>(this MiddlewareBuilder middlewareBuilder)
            where TTypeReader : ITypeReader
        {
            return middlewareBuilder.TryRegisterCustomTypeReader<TType, TTypeReader>();
        }


        internal static MiddlewareBuilder UseTypeReader(this MiddlewareBuilder middlewareBuilder)
        {
            return middlewareBuilder.TryRegisterMiddleware<TypeReaderBuilder>();
        }

        internal static MiddlewareBuilder UseProxies(this MiddlewareBuilder middlewareBuilder)
        {
            return middlewareBuilder.TryRegisterMiddleware<ProxyBuilder>();
        }
    }
}