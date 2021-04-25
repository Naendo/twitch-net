﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitchWrapper.Core.Attributes;
using TwitchWrapper.Core.Exceptions;
using TwitchWrapper.Core.Models;
using TwitchWrapper.Core.Proxies;
using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core
{
    public class TwitchCommander
    {
        private static readonly Dictionary<string, MethodInfo> _commandCache = new Dictionary<string, MethodInfo>();

        private Assembly _assembly;

        private readonly string _prefix;

        private readonly TwitchBot _bot;

        private IServiceProvider _serviceProvider;

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
        public async Task InitalizeCommanderAsync(IServiceCollection serviceCollection, Assembly assembly)
        {
            _assembly = assembly;
            await Task.Run(() =>
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
            var types = _assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseModule)));

            foreach (var type in types)
            {
                var result = type.GetMethods()
                    .Where(x => Attribute.IsDefined(x, typeof(CommandAttribute)))
                    .Select(x => new{x.GetCustomAttribute<CommandAttribute>()!.Command, Method = x});


                RegisterTypeForDependencyInjection(type, serviceCollection);


                foreach (var item in result)
                {
                    if (!_commandCache.TryAdd(item.Command, item.Method))
                    {
                        throw new DuplicatedCommandException(
                            $"Duplicated entry on {item.Command} on method {item.Method.Name}");
                    }
                }
            }


            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        /// <summary>
        /// Method for registering Modules marked as <see cref="BaseModule"/> in DI Container
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serviceCollection"></param>
        private void RegisterTypeForDependencyInjection(Type type, IServiceCollection serviceCollection)
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
        /// <param name="messageResponseModel"></param>
        private async Task ExecuteCommandAsync(MessageResponseModel messageResponseModel)
        {
            //(1) Identify Command
            var commandModel = ParseResponse(messageResponseModel);

            //(2) Read Cache
            if (!_commandCache.TryGetValue(commandModel.CommandKey.ToLower(), out var methodInfo))
                return;

            //(3) Create Instance of Class and BaseModule
            var instance = (BaseModule) _serviceProvider.GetService(methodInfo.DeclaringType);


            if (!await ValidateRoleAttributesAsync(methodInfo, messageResponseModel))
                return;

            //(4) Initalize BaseModule.cs
            instance.ChannelProxy = new ChannelProxy{
                Channel = messageResponseModel.Channel
            };
            instance.UserProxy = new UserProxy{
                IsBroadcaster = messageResponseModel.IsBroadcaster,
                IsVip = messageResponseModel.IsVip,
                IsModerator = messageResponseModel.IsModerator,
                IsSubscriber = messageResponseModel.IsSubscriber,
                Name = messageResponseModel.Name,
                Color = messageResponseModel.Color
            };
            instance.CommandProxy = new CommandProxy{
                Message = messageResponseModel.Message
            };
            instance.TwitchBot = _bot;


            //(4) Invoke
            var paramIndex = methodInfo.GetParameters().Length;

            //(5) TypeReader Middleware


            // ReSharper disable once CoVariantArrayConversion
            var task = (Task) methodInfo.Invoke(instance, commandModel.Parameter[..paramIndex])!;
            await task.ConfigureAwait(false);
        }


        /// <summary>
        /// Parse <see cref="IResponse"/> to <see cref="CommandModel"/>
        /// </summary>
        private CommandModel ParseResponse(MessageResponseModel model)
        {
            var responseStringAsArray = model.Message.Split(' ');
            return new CommandModel{
                CommandKey = responseStringAsArray[0][1..],
                Parameter = responseStringAsArray[1..]
            };
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