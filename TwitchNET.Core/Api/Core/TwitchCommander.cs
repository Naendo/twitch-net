using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;
using TwitchWrapper.Core;

namespace TwitchNET.Core
{
    public class TwitchCommander
    {
        private static readonly Dictionary<string, CommandInfo> CommandCache = new();

        private readonly TwitchBot _bot;

        private readonly string _prefix;

        private Assembly _assembly;

        private RequestBuilder _requestBuilder;

        private IServiceProvider _serviceProvider;


        public TwitchCommander(TwitchBot bot, string prefix = "!")
        {
            _prefix = prefix;
            _bot = bot;
            bot.OnLogAsync += OnLogHandlerAsync;
        }


        /// <summary>
        ///     Initalize CommandModule Pattern and scan Methodes marked as <see cref="CommandAttribute" /> in Assembly
        /// </summary>
        /// <param name="serviceCollection">Dependency Injection - ServiceCollection</param>
        /// <param name="assembly">Required: The assembly cointaining Command Modules inhereting <see cref="BaseModule"/></param>
        /// <param name="requestBuilder"></param>
        public Task InitalizeCommanderAsync(IServiceCollection serviceCollection, Assembly assembly,
            RequestBuilder requestBuilder = null)
        {
            _assembly = assembly;
            return Task.Run(() =>
            {
                _bot.Client.SubscribeReceive += HandleCommandRequest;
                ScanAssemblyForCommands(serviceCollection);
                ConfigureMiddleware(requestBuilder ?? new RequestBuilder());
            });
        }


        /// <summary>
        ///     Scan for Modules which inherit <see cref="BaseModule" /> and cache Methodes with <see cref="CommandAttribute" />
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
                    if (!CommandCache.TryAdd(item.Command, new CommandInfo{
                        CommandKey = item.Command,
                        MethodInfo = item.Method
                    }))
                        throw new DuplicatedCommandException(
                            $"Duplicated entry on {item.Command} on method {item.Method.Name}");
            }


            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        /// <summary>
        ///     Configure <see cref="RequestBuilder" />
        /// </summary>
        private void ConfigureMiddleware(RequestBuilder requestBuilder)
        {
            requestBuilder.UseProxies();
            requestBuilder.UseTypeReader();

            _requestBuilder = requestBuilder;
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
            var commandModel = messageResponseModel.ParseResponse();


            if (!CommandCache.TryGetValue(commandModel.CommandKey.ToLower(), out var commandInfo))
                return;


            var instance = (BaseModule) _serviceProvider!.GetService(commandInfo.MethodInfo.DeclaringType!);


            if (!await ValidateRoleAttributesAsync(commandInfo.MethodInfo, messageResponseModel))
                return;


            if (_requestBuilder is null)
                throw new ArgumentNullException($"{nameof(_requestBuilder)}: Method {nameof(ExecuteCommandAsync)}");

            var context = _requestBuilder.ExecutePipeline(commandInfo, instance, _bot, messageResponseModel);

            await _requestBuilder.InvokeEndpointAsync(context).ConfigureAwait(false);
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
            if (Attribute.IsDefined(methodInfo, typeof(BroadcasterAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster);

            if (Attribute.IsDefined(methodInfo, typeof(ModeratorAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster || messageResponseModel.IsModerator);

            return new ValueTask<bool>(true);
        }
    }
}