using System;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Middleware;

namespace TwitchNET.Core.Extensions
{
    /// <summary>
    /// Extensions for <see cref="PipelineBuilder"/>. Use the extension methods to register your custom middleware.
    /// </summary>
    public static class PipelineBuilderExtensions
    {
        /// <summary>
        /// Extension methode to register customized <see cref="IMiddleware"/>
        /// </summary>
        /// <typeparam name="TMiddleware">TMiddleware must implement IMiddleware</typeparam>
        public static PipelineBuilder UseMiddleware<TMiddleware>(this PipelineBuilder pipelineBuilder)
            where TMiddleware : IMiddleware
        {
            return pipelineBuilder.TryRegisterCustomMiddleware<TMiddleware>();
        }

        /// <summary>
        /// Extension methode to register customized <see cref="ITypeReader"/>
        /// </summary>
        /// <typeparam name="TType">Intended output <see cref="Type"/> for customized <see cref="ITypeReader"/></typeparam>
        /// <typeparam name="TTypeReader">Customized TypeReader which implements <see cref="ITypeReader"/></typeparam>
        public static PipelineBuilder UseTypeReader<TType, TTypeReader>(this PipelineBuilder pipelineBuilder)
            where TTypeReader : ITypeReader
        {
            return pipelineBuilder.TryRegisterCustomTypeReader<TType, TTypeReader>();
        }


        internal static PipelineBuilder UseTypeReader(this PipelineBuilder pipelineBuilder)
        {
            return pipelineBuilder.TryRegisterMiddleware<TypeReaderBuilder>();
        }

        internal static PipelineBuilder UseProxies(this PipelineBuilder pipelineBuilder)
        {
            return pipelineBuilder.TryRegisterMiddleware<ProxyBuilder>();
        }
    }
}