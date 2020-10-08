using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwitchWrapper.Core.Attributes;
using TwitchWrapper.Core.Exceptions;
using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core
{
    public class TwitchCommandHandler
    {
        private static Dictionary<string, MethodInfo> _commandCache = new Dictionary<string, MethodInfo>();
        private readonly Assembly _assembly;
        private readonly string _prefix;

        public TwitchCommandHandler(Assembly assembly, string prefix = "!")
        {
            _assembly = assembly;
            ScanAssemblyForCommands();
            _prefix = prefix;
        }

        public TwitchCommandHandler(Type typeInAssembly)
        {
            _assembly = typeInAssembly.Assembly;
            ScanAssemblyForCommands();
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
        /// Check if <see cref="IResponse"/> is command by validating <see cref="_prefix"/> is first charactar
        /// </summary>
        private bool IsCommand(IResponse response)
        {
            var parsedResponse = response.MapResponse();

            if (parsedResponse.ResponseType != ResponseType.PrivMsg)
                return true;

            return parsedResponse.Message.StartsWith(_prefix);
        }


        /// <summary>
        /// PLES GET RECEIVE AND DO THE COMMAND EXECUTION ON COMMAND TEXT YES
        /// </summary>
        internal void HandleCommandRequest(IResponse command)
        {
            if (!IsCommand(command))
                return;

            var result = command.MapResponse();

            ExecuteCommand(result);
        }


        private void ExecuteCommand(ResponseModel responseModel)
        {
            var responseStringAsArray = responseModel.Message.Split(' ');

            var commandText = responseStringAsArray[0][1..];

            if (responseStringAsArray.Length > 1)
            {
                var commandParam = responseStringAsArray[1..];
            }
        }
    }
}