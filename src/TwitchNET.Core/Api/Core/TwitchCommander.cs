﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Extensions;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Modules;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core
{
    /// <summary>
    /// The <see cref="TwitchCommander{TCommander}"/> defines a framework for our module framework. The Commander tasks are to receive incoming messages from <see cref="TwitchClient"/> and to invokes the request pipeline using
    /// <see cref="PipelineBuilder"/> and <see cref="ServiceCollection"/>.
    /// To utilize the module framework see <see cref="BaseModule{TCommander}"/>
    /// </summary>
    /// <example>
    /// The following code shows, how to utilize the <see cref="TwitchCommander{TCommander}"/>.
    /// <code lang="cs">
    /// public class DummyCommander : TwitchCommander{T}
    ///{
    ///public DummyCommander(TwitchClient client) : base(client, "!")
    ///{
    ///}
    ///
    ///public async Task InitializeAsync()
    ///{
    ///        await InitializeCommanderAsync(
    ///       serviceCollection: BuildServiceCollection(),
    ///     typeof(Program).Assembly,
    ///     BuildRequestPipeline()
    /// );
    ///}
    ///
    ///protected override async Task HandleCommandRequest(IResponse command)
    ///{
    ///  await base.HandleCommandRequest(command);
    ///}
    ///
    ///private static PipelineBuilder BuildRequestPipeline()
    ///  => new PipelineBuilder()
    ///       .UseMiddleware{TMiddleware}();
    ///
    ///private static IServiceCollection BuildServiceCollection()
    ///  => new ServiceCollection().AddSingleton{TService}();
    ///}
    /// 
    /// </code>
    ///
    /// This Methode will now scan all classes that inherit from <see cref="BaseModule{TCommander}"/>.
    ///
    /// <code>
    ///  public class ExampleModule : BaseModule{TCommander} { }
    /// </code>
    /// 
    /// </example>
    public abstract class TwitchCommander<TCommander> where TCommander : class
    {
        private readonly Dictionary<string, CommandInfo> _commandCache = new();

        private readonly TwitchClient _twitchClient;

        private readonly string _prefix;

        private Assembly _assembly;

        private PipelineBuilder _pipelineBuilder;

        private IServiceProvider _serviceProvider;


        /// <summary>
        /// Initialize a new <see cref="TwitchCommander"/>
        /// </summary>
        /// <param name="bot">Instance of <see cref="TwitchClient"/></param>
        /// <param name="prefix">Choose your command prefix!</param>
        protected TwitchCommander(TwitchClient twitchClient, string prefix)
        {
            _twitchClient = twitchClient;
            _prefix = prefix;
        }


        ///<summary>
        /// Initializes the module framework and scans all commands marked with a <see cref="CommandAttribute"/> in a given <see cref="Assembly"/>
        /// </summary>
        /// <remarks>Calling this methode will result in mapping the IRC-Inputs to registered modules. Executing this method is required.</remarks>
        /// <param name="serviceCollection">Dependency Injection - ServiceCollection</param>
        /// <param name="assembly">The assembly containing Command Modules inheriting <see cref="BaseModule"/></param>
        /// <param name="middlewareBuilder">Optional: ServiceCollection to register customized <see cref="IMiddleware"/></param>
        protected Task InitializeCommanderAsync(IServiceCollection serviceCollection, Assembly assembly,
            PipelineBuilder middlewareBuilder = null)
        {
            _assembly = assembly;

            return Task.Run(() =>
            {
                _twitchClient.Client.OnMessageReceive += HandleCommandRequest;
                ScanAssemblyForCommands(serviceCollection);
                ConfigureMiddleware(middlewareBuilder ?? new PipelineBuilder());
            });
        }
        
        /// <summary>
        /// <see cref="HandleCommandRequest"/> is a virtual base method that acts as event handler for all incoming message events.
        /// Overriding <see cref="HandleCommandRequest"/> allows you to add custom behavior before the request pipeline gets executed.
        /// > [!IMPORTANT]
        /// >
        /// > IMPORTANT: not calling base.HandleCommandRequest(command) will lead to the request pipeline not getting executed!
        ///  </summary>
        protected  virtual async Task HandleCommandRequest(IResponse command)
        {
            if (!IsCommand(command))
                return;

            var result = command.GetResult();

            await ExecuteCommandAsync(result);
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
                .Where(type => type.IsSubclassOf(typeof(BaseModule<TCommander>)));


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
        ///     Configure <see cref="PipelineBuilder" />
        /// </summary>
        private void ConfigureMiddleware(PipelineBuilder pipelineBuilder)
        {
            pipelineBuilder.UseProxies();
            pipelineBuilder.UseTypeReader();

            _pipelineBuilder = pipelineBuilder;
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

                if (!await ValidateRoleAttributesAsync(commandInfo.MethodInfo, messageResponseModel))
                    return;

                var instance =
                    (BaseModule<TCommander>) _serviceProvider!.GetService(commandInfo.MethodInfo.DeclaringType!);


                if (_pipelineBuilder is null)
                    throw new ArgumentNullException(
                        $"{nameof(_pipelineBuilder)}: Method {nameof(ExecuteCommandAsync)}");


                var context =
                    _pipelineBuilder.ExecutePipeline(commandInfo, instance, _twitchClient, messageResponseModel);

                await _pipelineBuilder.InvokeEndpointAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _twitchClient.OnLogAsync(ex);
            }
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

            else if (Attribute.IsDefined(methodInfo, typeof(BroadcasterAttribute)))
                return new ValueTask<bool>(messageResponseModel.IsBroadcaster);

            return new ValueTask<bool>(true);
        }
    }
}