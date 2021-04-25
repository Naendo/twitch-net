using System;
using System.Collections;
using System.Collections.Generic;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;

namespace TwitchNET.Core
{
    public class RequestContext
    {
        internal CommandInfo CommandInfo { get; set; }
        internal BaseModule Endpoint { get; set; }
        internal MessageResponseModel IrcResponseModel { get; set; }
        internal TwitchBot BotContext { get; set; }
        
        public ParameterCollection Parameters { get; internal set; }

        public string Message => IrcResponseModel.Message;
    }


    public class ParameterCollection : IReadOnlyList<object>
    {
        internal object[] Values { get; }


        internal ParameterCollection(object[] values)
        {
            Values = values;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>) Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Values.Length;

        public object this[int index] => Values[index];
    }
}