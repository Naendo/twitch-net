using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Middleware;
using TwitchNET.Core.Modules;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core
{
    ///<summary>
    ///The <see cref="PipelineBuilder"/> defines a middleware framework for the chat request pipeline.
    ///<para>By default two internal middlewares: <see cref="ProxyBuilder"/> and <see cref="TypeReaderBuilder"/> are invoked before a
    ///custom middleware can be invoked.</para>
    ///<para>Lifecycle: -> <see cref="ProxyBuilder"/> -> <see cref="TypeReaderBuilder"/> -> custom middleware with <see cref="IMiddleware"/>
    /// -> CommandModule</para>
    ///</summary>
    ///<example>
    ///And example of how to utilize <see cref="PipelineBuilder"/>
    ///<code>
    ///public async Task InitializeTwitchClient()
    ///{
    ///   ...
    ///  
    ///   await commander.InitializeCommanderAsync(
    ///     serviceCollection: BuildServiceCollection(),
    ///     assembly: typeof(Program).Assembly,
    ///     requestBuilder: BuildRequest()
    ///   );
    /// 
    ///   await Task.Delay(-1);
    ///}
    /// 
    ///private static RequestBuilder BuildRequest() =>
    ///      new PipelineBuilder()
    ///         .UseMiddleware&lt;YourMiddleware&gt;();
    ///</code>
    ///</example>
    public sealed class PipelineBuilder
    {
        private readonly List<Type> _middlewareTypes = new();
        private readonly List<Type> _customMiddlewareTypes = new();

        private readonly Dictionary<Type, ITypeReader> _customTypeReader = new Dictionary<Type, ITypeReader>();

        private readonly IServiceCollection _serviceCollection = new ServiceCollection();


        internal RequestContext ExecutePipeline(CommandInfo commandInfo, ModuleProxyBase endpoint,
            TwitchClient botContext,
            MessageResponseModel messageResponse)
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var middlewareCollection = _customMiddlewareTypes.Count != 0
                ? _middlewareTypes.Concat(_customMiddlewareTypes).ToList()
                : _middlewareTypes;


            RequestContext context = null;

            foreach (var type in middlewareCollection)
            {
                var middleware = serviceProvider.GetService(type) as IMiddleware;

                if (middleware != null)
                {
                    context = middleware.Execute(context ?? new RequestContext
                    {
                        CommandInfo = commandInfo,
                        Endpoint = endpoint,
                        IrcResponseModel = messageResponse,
                        BotContext = botContext,
                        CustomTypeReaders = _customTypeReader
                    });
                }
            }

            return context;
        }

        internal Task InvokeEndpointAsync(RequestContext requestContext)
        {
            return (Task) requestContext.CommandInfo.MethodInfo.Invoke(requestContext.Endpoint,
                requestContext.Parameters.Values)!;
        }

        internal PipelineBuilder TryRegisterCustomTypeReader<TType, TTypeReader>() where TTypeReader : ITypeReader
        {
            var type = typeof(TType);

            if (_customTypeReader.ContainsKey(type))
                return this;

            _customTypeReader.Add(type, Activator.CreateInstance<TTypeReader>());
            return this;
        }

        internal PipelineBuilder TryRegisterMiddleware<TType>() where TType : IMiddleware
        {
            var type = typeof(TType);
            if (_middlewareTypes.Contains(type))
                return this;

            _middlewareTypes.Add(type);
            _serviceCollection.AddSingleton(type);
            return this;
        }

        internal PipelineBuilder TryRegisterCustomMiddleware<TMiddleware>() where TMiddleware : IMiddleware
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