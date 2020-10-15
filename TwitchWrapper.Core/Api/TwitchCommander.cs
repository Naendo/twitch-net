using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using TwitchWrapper.Core.Attributes;
using TwitchWrapper.Core.Exceptions;
using TwitchWrapper.Core.Models;
using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core
{
    public class TwitchCommander
    {
        private static readonly Dictionary<string, MethodInfo> _commandCache = new Dictionary<string, MethodInfo>();

        private static readonly Dictionary<Type, ObjectActivator>
            _objectActivatorCache = new Dictionary<Type, ObjectActivator>();

        private readonly Assembly _assembly;
        private readonly string _prefix;
        private readonly TwitchBot _bot;

        public TwitchCommander(Assembly assembly, TwitchBot bot, string prefix = "!")
        {
            _assembly = assembly;
            _prefix = prefix;
            _bot = bot;
        }

        public async Task InitalizeCommanderAsync(IServiceProvider serviceProvider)
        {
            await Task.Run(() =>
            {
                _bot.Client.SubscribeReceive += HandleCommandRequest;
                ScanAssemblyForCommands();
            });
        }


        /// <summary>
        /// Scan for Modules which inherit <see cref="BaseModule"/> and cache Methodes with <see cref="CommandAttribute"/>
        /// </summary>
        /// <exception cref="DuplicatedCommandException"></exception>
        private void ScanAssemblyForCommands()
        {
            var types = _assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseModule)));

            foreach (var type in types)
            {
                var result = type.GetMethods()
                    .Where(x => Attribute.IsDefined(x, typeof(CommandAttribute)))
                    .Select(x => new {x.GetCustomAttribute<CommandAttribute>()!.Command, Method = x});

                foreach (var item in result)
                {
                    if (!_commandCache.TryAdd(item.Command, item.Method))
                    {
                        throw new DuplicatedCommandException(
                            $"Duplicated entry on {item.Command} on method {item.Method.Name}");
                    }
                }
            }
        }


        /// <summary>
        /// PLES GET RECEIVE AND DO THE COMMAND EXECUTION ON COMMAND TEXT YES
        /// </summary>
        private async Task HandleCommandRequest(IResponse command)
        {
            if (!IsCommand(command))
                return;

            var result = command.MapResponse();


            Console.WriteLine($"User: {result.Name}, Message: {result.Message}");
            await ExecuteCommandAsync(result);
        }


        /// <summary>
        /// Check if <see cref="IResponse"/> is command by validating <see cref="_prefix"/> is first charactar
        /// </summary>
        private bool IsCommand(IResponse response)
        {
            var parsedResponse = response.MapResponse();

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
            var commandIdentifier = ParseResponse(messageResponseModel);

            //(2) Read Cache
            if (!_commandCache.TryGetValue(commandIdentifier.CommandKey, out var methodInfo))
                return;

            var user = new UserModel(messageResponseModel.IsBroadcaster,
                messageResponseModel.IsVip,
                messageResponseModel.IsSubscriber,
                messageResponseModel.Name,
                messageResponseModel.Color);


            //(3) Create Instance of Class and BaseModule
            var instance = CreateInstance(methodInfo.DeclaringType).Invoke();

            var instanceType = instance.GetType();

            instanceType.BaseType!
                    .GetField("_bot", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(instance, _bot);

            instanceType.BaseType
                    .GetField("User", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(instance, user);

            //(4) Invoke
            var paramIndex = methodInfo.GetParameters().Length;

            // ReSharper disable once CoVariantArrayConversion
            var task = (Task) methodInfo.Invoke(instance, commandIdentifier.Parameter[..paramIndex]);

            await task.ConfigureAwait(false);
        }


        /// <summary>
        /// Parse <see cref="IResponse"/> to <see cref="CommandModel"/>
        /// </summary>
        private CommandModel ParseResponse(MessageResponseModel model)
        {
            var responseStringAsArray = model.Message.Split(' ');
            return new CommandModel
            {
                CommandKey = responseStringAsArray[0][1..],
                Parameter = responseStringAsArray[1..]
            };
        }

        /// <summary>
        /// Creating <see cref="ObjectActivator"/> delegate for faster <see cref="Activator"/>
        /// </summary>
        /// <param name="type">Instantiating <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static ObjectActivator CreateInstance(Type type)
        {
            if (_objectActivatorCache.TryGetValue(type, out var objectActivator))
                return objectActivator;

            if (type == null)
            {
                throw new NullReferenceException("type");
            }

            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            var activator = (ObjectActivator) dynamicMethod.CreateDelegate(typeof(ObjectActivator));

            _objectActivatorCache[type] = activator;
            return activator;
        }

        private delegate object ObjectActivator();
    }
}