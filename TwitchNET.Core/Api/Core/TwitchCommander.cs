using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Extensions;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Logging;
using TwitchNET.Core.Modules;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core
{
    /// <summary>
    /// The <see cref="TwitchCommander"/> defines a framework for our module framework. The Commander invokes the request pipeline using
    /// <see cref="MiddlewareBuilder"/> and <see cref="ServiceCollection"/>.
    /// To utilize the module framework see <see cref="BaseModule"/>
    /// </summary>
    /// <example>
    /// The following code shows, how to utilize the <see cref="TwitchCommander"/>.
    /// <code>
    /// await commander.InitializeCommanderAsync(
    ///     serviceCollection: BuildServiceCollection(),
    ///     assembly: typeof(Program).Assembly,
    ///     requestBuilder: null
    /// );
    /// </code>
    ///
    /// This Methode will now scan all classes that inherit from <see cref="BaseModule"/>.
    ///
    /// <code>
    ///  public class ExampleModule : BaseModule { }
    /// </code>
    /// 
    /// </example>
    public class TwitchCommander
    {
        private static readonly Dictionary<string, CommandInfo> _commandCache = new();

        private readonly TwitchBot _bot;

        private readonly string _prefix;

        private Assembly _assembly;

        private MiddlewareBuilder _middlewareBuilder;

        private IServiceProvider _serviceProvider;


        /// <summary>
        /// Initialize a new <see cref="TwitchCommander"/>
        /// </summary>
        /// <param name="bot">Instance of <see cref="TwitchBot"/></param>
        /// <param name="prefix">Choose your command prefix!</param>
        /// <param name="logOutput">Log via File or Console</param>
        public TwitchCommander(TwitchBot bot, string prefix = "!", LogOutput logOutput = LogOutput.Console)
        {
            _prefix = prefix;
            _bot = bot;
            var logger = new Logger(logOutput);
            bot.OnLogAsync += logger.OnLogHandlerAsync;
        }


        ///<summary>
        /// Initializes the module framework and scans all commands marked with a <see cref="CommandAttribute"/> in a given <see cref="Assembly"/>
        /// </summary>
        /// <remarks>Calling this methode will result in mapping the IRC-Inputs to registered modules. Executing this method is required.</remarks>
        /// <param name="serviceCollection">Dependency Injection - ServiceCollection</param>
        /// <param name="assembly">The assembly containing Command Modules inheriting <see cref="BaseModule"/></param>
        /// <param name="middlewareBuilder">Optional: ServiceCollection to register customized <see cref="IMiddleware"/></param>
        public Task InitializeCommanderAsync(IServiceCollection serviceCollection, Assembly assembly,
            MiddlewareBuilder middlewareBuilder = null)
        {
            _assembly = assembly;
            return Task.Run(() =>
            {
                _bot.Client.SubscribeReceive += HandleCommandRequest;
                ScanAssemblyForCommands(serviceCollection);
                ConfigureMiddleware(middlewareBuilder ?? new MiddlewareBuilder());
            });
        }


        /// <summary>
        ///     Scan for Modules which inherit <see cref="BaseModule" /> and cache Methodes with <see cref="CommandAttribute" />
        /// </summary>
        private void ScanAssemblyForCommands(IServiceCollection serviceCollection)
        {
            if (_assembly is null)
                throw new ArgumentNullException(
                    $"{nameof(_assembly)} was null. {nameof(InitializeCommanderAsync)} invokation failed.");

            var types = _assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseModule)));

            foreach (var type in types)
            {
                //ToDo: Remove LINQ
                var result = type.GetMethods()
                    .Where(x => Attribute.IsDefined(x, typeof(CommandAttribute)))
                    .Select(x => new {x.GetCustomAttribute<CommandAttribute>()!.Command, Method = x});


                ConfigureServiceCollection(type, serviceCollection);


                foreach (var item in result)
                    if (!_commandCache.TryAdd(item.Command, new CommandInfo
                    {
                        CommandKey = item.Command,
                        MethodInfo = item.Method
                    }))
                        throw new DuplicatedCommandException(
                            $"Duplicated entry on {item.Command} on method {item.Method.Name}");
            }


            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        /// <summary>
        ///     Configure <see cref="MiddlewareBuilder" />
        /// </summary>
        private void ConfigureMiddleware(MiddlewareBuilder middlewareBuilder)
        {
            middlewareBuilder.UseProxies();
            middlewareBuilder.UseTypeReader();

            _middlewareBuilder = middlewareBuilder;
        }


        /// <summary>
        ///     Method for registering Modules marked as <see cref="BaseModule" /> in DI Container
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceCollection"></param>
        private void ConfigureServiceCollection(Type type, IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient(type);
        }


        /// <summary>
        ///     CommandReceive EventHandler
        /// </summary>
        private async Task HandleCommandRequest(IResponse command)
        {
            if (!IsCommand(command))
                return;

            var result = command.GetResult();

            await ExecuteCommandAsync(result);
        }


        /// <summary>
        ///     Check if <see cref="IResponse" /> is command by validating <see cref="_prefix" /> is first charactar
        /// </summary>
        private bool IsCommand(IResponse response)
        {
            var parsedResponse = response.GetResult();

            if (parsedResponse.ResponseType != ResponseType.PrivMsg)
                return false;

            return parsedResponse.Message.StartsWith(_prefix);
        }


        /// <summary>
        ///     Execute Command if <see cref="IResponse" /> is registerd as <see cref="BaseModule" /> with Attribute
        ///     <see cref="CommandAttribute" />
        /// </summary>
        private async Task ExecuteCommandAsync(MessageResponseModel messageResponseModel)
        {
            try
            {
                var commandModel = messageResponseModel.ParseResponse();


                if (!_commandCache.TryGetValue(commandModel.CommandKey.ToLower(), out var commandInfo))
                    return;


                var instance = (BaseModule) _serviceProvider!.GetService(commandInfo.MethodInfo.DeclaringType!);


                if (!await ValidateRoleAttributesAsync(commandInfo.MethodInfo, messageResponseModel))
                    return;


                if (_middlewareBuilder is null)
                    throw new ArgumentNullException($"{nameof(_middlewareBuilder)}: Method {nameof(ExecuteCommandAsync)}");


                var context = _middlewareBuilder.ExecutePipeline(commandInfo, instance, _bot, messageResponseModel);

                await _middlewareBuilder.InvokeEndpointAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        /// <summary>
        ///     Basic Log EventHandler
        /// </summary>
        /// <param name="message">Message to be Logged</param>
        private async Task OnLogHandlerAsync(string message)
        {
            await InternalLogger.LogEventsAsync(message);
        }


        /// <summary>
        ///     Role-Attribute-Handler for IRC-Client Permissions
        /// </summary>
        /// <param name="methodInfo">Executing Command MethodInfo</param>
        /// <param name="messageResponseModel">Executing Command ResponseModel</param>
        /// <returns></returns>
        private ValueTask<bool> ValidateRoleAttributesAsync(MethodInfo methodInfo,
            MessageResponseModel messageResponseModel)
        {
            if (Attribute.IsDefined(methodInfo, typeof(ModeratorAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster || messageResponseModel.IsModerator);

            if (Attribute.IsDefined(methodInfo, typeof(BroadcasterAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster);

            return new ValueTask<bool>(true);
        }
    }
}