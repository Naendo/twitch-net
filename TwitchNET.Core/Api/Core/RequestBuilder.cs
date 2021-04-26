using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Core.Middleware;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core
{
    public sealed class RequestBuilder
    {
        private readonly List<Type> _middlewareTypes = new();
        private readonly List<Type> _customMiddlewareTypes = new();

        private readonly Dictionary<Type, ITypeReader> _customTypeReader = new Dictionary<Type, ITypeReader>();

        private readonly IServiceCollection _serviceCollection = new ServiceCollection();


        internal RequestContext ExecutePipeline(CommandInfo commandInfo, BaseModule endpoint, TwitchBot botContext,
            MessageResponseModel messageResponse)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            List<Type> middlewareCollection;

            middlewareCollection = _customMiddlewareTypes.Count != 0
                ? _middlewareTypes.Concat(_customMiddlewareTypes).ToList()
                : _middlewareTypes;


            RequestContext context = null;

            foreach (var type in middlewareCollection)
            {
                var middleware = serviceProvider.GetService(type) as IMiddleware;
                context = middleware.Execute(context ?? new RequestContext{
                    CommandInfo = commandInfo,
                    Endpoint = endpoint,
                    IrcResponseModel = messageResponse,
                    BotContext = botContext,
                    CustomTypeReaders = _customTypeReader
                });
            }

            return context;
        }

        internal Task InvokeEndpointAsync(RequestContext requestContext)
        {
            return (Task) requestContext.CommandInfo.MethodInfo.Invoke(requestContext.Endpoint,
                requestContext.Parameters.Values)!;
        }


        internal RequestBuilder TryRegisterCustomTypeReader<TType, TTypeReader>() where TTypeReader : ITypeReader
        {
            var type = typeof(TType);

            if (_customTypeReader.ContainsKey(type))
                return this;

            _customTypeReader.Add(type, Activator.CreateInstance<TTypeReader>());
            return this;
        }

        internal RequestBuilder TryRegisterMiddleware<TType>() where TType : IMiddleware
        {
            var type = typeof(TType);
            if (_middlewareTypes.Contains(type))
                return this;

            _middlewareTypes.Add(type);
            _serviceCollection.AddSingleton(type);
            return this;
        }

        internal RequestBuilder TryRegisterCustomMiddleware<TMiddleware>() where TMiddleware : IMiddleware
        {
            var type = typeof(TMiddleware);
            if (_customMiddlewareTypes.Contains(type))
                return this;

            _customMiddlewareTypes.Add(type);
            _serviceCollection.AddSingleton(type);
            return this;
        }
    }
}