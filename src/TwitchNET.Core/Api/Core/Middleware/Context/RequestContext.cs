using System;
using System.Collections.Generic;
using TwitchNET.Core.Modules;
using TwitchNET.Core.Responses;
using TwitchNET.Core.Interfaces;


namespace TwitchNET.Core.Middleware
{
    public class RequestContext
    {
        internal CommandInfo CommandInfo { get; init; }
        internal ModuleProxyBase Endpoint { get; init; }
        internal MessageResponseModel IrcResponseModel { get; init; }
        internal TwitchClient BotContext { get; init; }

        internal Dictionary<Type, ITypeReader> CustomTypeReaders { get; set; }

        public ParameterCollection Parameters { get; internal set; }

        public string Message => IrcResponseModel.Message;
    }
}