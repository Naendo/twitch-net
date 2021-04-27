using System;
using TwitchNET.Core.Middleware;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core
{
    /// <summary>
    /// Extensions for <see cref="RequestBuilder"/>/>
    /// </summary>
    public static class RequestBuilderExtensions
    {
        /// <summary>
        /// Extension methode to register customized <see cref="IMiddleware"/>
        /// </summary>
        /// <typeparam name="TMiddleware">TMiddleware must implement IMiddleware</typeparam>
        public static RequestBuilder UseMiddleware<TMiddleware>(this RequestBuilder requestBuilder)
            where TMiddleware : IMiddleware
        {
            return requestBuilder.TryRegisterCustomMiddleware<TMiddleware>();
        }

        /// <summary>
        /// Extension methode to register customized <see cref="ITypeReader"/>
        /// </summary>
        /// <typeparam name="TType">Intended output <see cref="Type"/> for customized <see cref="ITypeReader"/></typeparam>
        /// <typeparam name="TTypeReader">Customized TypeReader which implements <see cref="ITypeReader"/></typeparam>
        public static RequestBuilder UseTypeReader<TType, TTypeReader>(this RequestBuilder requestBuilder)
            where TTypeReader : ITypeReader
        {
            return requestBuilder.TryRegisterCustomTypeReader<TType, TTypeReader>();
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