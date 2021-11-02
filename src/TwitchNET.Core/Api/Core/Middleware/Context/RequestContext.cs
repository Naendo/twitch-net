using System;
using System.Collections.Generic;
using TwitchNET.Core.Modules;
using TwitchNET.Core.Responses;
using TwitchNET.Core.Interfaces;

namespace System.Runtime.CompilerServices
{
    using System.ComponentModel;

    /// <summary>
    /// Reserved to be used by the compiler for tracking metadata.
    /// This class should not be used by developers in source code.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}


namespace TwitchNET.Core.Middleware
{
    public class RequestContext
    {
        internal CommandInfo CommandInfo { get; init; }
        internal BaseModule Endpoint { get; init; }
        internal MessageResponseModel IrcResponseModel { get; init; }
        internal TwitchClient BotContext { get; init; }

        internal Dictionary<Type, ITypeReader> CustomTypeReaders { get; set; }

        public ParameterCollection Parameters { get; internal set; }

        public string Message => IrcResponseModel.Message;
    }
}