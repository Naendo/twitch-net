using System;
using System.Collections.Generic;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core.Middleware
{
    public class RequestContext
    {
        internal CommandInfo CommandInfo { get; init; }
        internal BaseModule Endpoint { get; init; }
        internal MessageResponseModel IrcResponseModel { get; init; }
        internal TwitchBot BotContext { get; init; }

        internal Dictionary<Type, ITypeReader> CustomTypeReaders { get; set; }

        public ParameterCollection Parameters { get; internal set; }

        public string Message => IrcResponseModel.Message;
    }
}