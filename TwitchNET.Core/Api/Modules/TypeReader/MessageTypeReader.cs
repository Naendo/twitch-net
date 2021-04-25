﻿using System;
using System.ComponentModel;

namespace TwitchNET.Modules.TypeReader
{
    public class MessageTypeReader : TypeReader
    {
        public static MessageTypeReader Default = new();

        private MessageTypeReader()
        {
        }

        public override TType ConvertFrom<TType>(string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TType));
            return (TType) converter.ConvertFrom(input);
        }

        public override object ConvertFrom(Type type, string input)
        {
            var converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFrom(input);
        }
    }
}