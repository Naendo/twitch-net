using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;


namespace TwitchNET.Core
{
    public class TwitchCommander
    {
        private static readonly Dictionary<string, CommandInfo> CommandCache = new Dictionary<string, CommandInfo>();


        private Assembly _assembly;

        private readonly string _prefix;

        private readonly TwitchBot _bot;

        private IServiceProvider _serviceProvider;

        private RequestBuilder _requestBuilder;


        public TwitchCommander(TwitchBot bot, string prefix = "!")
        {
            _prefix = prefix;
            _bot = bot;
            bot.OnLogAsync += OnLogHandlerAsync;
        }


        /// <summary>
        /// Initalize CommandModule Pattern and scan Methodes marked as <see cref="CommandAttribute"/> in Assembly
        /// </summary>
        /// <param name="serviceCollection">DI ServiceCollection</param>
        /// <param name="assembly">Assembly Containing CommandModules marked with <see cref="BaseModule"/></param>
        public Task InitalizeCommanderAsync(IServiceCollection serviceCollection, Assembly assembly)
        {
            _assembly = assembly;
            return Task.Run(() =>
            {
                _bot.Client.SubscribeReceive += HandleCommandRequest;
                ScanAssemblyForCommands(serviceCollection);
            });
        }


        /// <summary>
        /// Scan for Modules which inherit <see cref="BaseModule"/> and cache Methodes with <see cref="CommandAttribute"/>
        /// </summary>
        /// <exception cref="DuplicatedCommandException"></exception>
        private void ScanAssemblyForCommands(IServiceCollection serviceCollection)
        {
            if (_assembly is null)
                throw new ArgumentNullException(
                    $"{nameof(_assembly)} was null. {nameof(InitalizeCommanderAsync)} invokation failed.");

            var types = _assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseModule)));

            foreach (var type in types)
            {
                //ToDo: Remove LINQ
                var result = type.GetMethods()
                    .Where(x => Attribute.IsDefined(x, typeof(CommandAttribute)))
                    .Select(x => new{x.GetCustomAttribute<CommandAttribute>()!.Command, Method = x});


                ConfigureServiceCollection(type, serviceCollection);


                foreach (var item in result)
                {
                    if (!CommandCache.TryAdd(item.Command, new CommandInfo{
                        CommandKey = item.Command,
                        MethodInfo = item.Method
                    }))
                    {
                        throw new DuplicatedCommandException(
                            $"Duplicated entry on {item.Command} on method {item.Method.Name}");
                    }
                }
            }


            if (_serviceProvider is null)
                throw new ArgumentNullException(
                    $"{nameof(_serviceProvider)} was null. {nameof(InitalizeCommanderAsync)} invokation failed.");

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        /// <summary>
        /// Configure <see cref="RequestBuilder"/>
        /// </summary>
        private void ConfigureMiddleware()
        {
            _requestBuilder = new RequestBuilder();

            _requestBuilder.UseProxies();
            _requestBuilder.UseTypeReader();
        }


        /// <summary>
        /// Method for registering Modules marked as <see cref="BaseModule"/> in DI Container
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceCollection"></param>
        private void ConfigureServiceCollection(Type type, IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient(type);
        }


        /// <summary>
        /// CommandReceive EventHandler
        /// </summary>
        private async Task HandleCommandRequest(IResponse command)
        {
            if (!IsCommand(command))
                return;

            var result = command.GetResult();

            await ExecuteCommandAsync(result);
        }


        /// <summary>
        /// Check if <see cref="IResponse"/> is command by validating <see cref="_prefix"/> is first charactar
        /// </summary>
        private bool IsCommand(IResponse response)
        {
            var parsedResponse = response.GetResult();

            if (parsedResponse.ResponseType != ResponseType.PrivMsg)
                return false;

            return parsedResponse.Message.StartsWith(_prefix);
        }


        /// <summary>
        /// Execute Command if <see cref="IResponse"/> is registerd as <see cref="BaseModule"/> with Attribute <see cref="CommandAttribute"/>
        /// </summary>
        private async Task ExecuteCommandAsync(MessageResponseModel messageResponseModel)
        {
            //(1) Identify Command
            var commandModel = messageResponseModel.ParseResponse();

            //(2) Read Cache
            if (!CommandCache.TryGetValue(commandModel.CommandKey.ToLower(), out var commandInfo))
                return;

            //(3) Create Instance of Class and BaseModule
            var instance = (BaseModule) _serviceProvider!.GetService(commandInfo.MethodInfo.DeclaringType!);


            if (!await ValidateRoleAttributesAsync(commandInfo.MethodInfo, messageResponseModel))
                return;


            var context = _requestBuilder.ExecutePipeline(commandInfo, instance, _bot, messageResponseModel);

            await _requestBuilder.InvokeEndpointAsync(context).ConfigureAwait(false);
        }


        /// <summary>
        /// Basic Log EventHandler
        /// </summary>
        /// <param name="message">Message to be Logged</param>
        private Task OnLogHandlerAsync(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{DateTime.Now:MM/dd/yyyy, HH:mm:ss}]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            return Task.CompletedTask;
        }


        /// <summary>
        /// Role-Attribute-Handler for IRC-Client Permissions
        /// </summary>
        /// <param name="methodInfo">Executing Command MethodInfo</param>
        /// <param name="messageResponseModel">Executing Command ResponseModel</param>
        /// <returns></returns>
        private ValueTask<bool> ValidateRoleAttributesAsync(MethodInfo methodInfo,
            MessageResponseModel messageResponseModel)
        {
            if (Attribute.IsDefined(methodInfo, typeof(BroadcasterAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster);

            if (Attribute.IsDefined(methodInfo, typeof(ModeratorAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster || messageResponseModel.IsModerator);

            return new ValueTask<bool>(true);
        }
    }
}