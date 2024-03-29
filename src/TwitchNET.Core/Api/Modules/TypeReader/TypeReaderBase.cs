﻿using System;
using TwitchNET.Core.Interfaces;

namespace TwitchNET.Core.Modules
{
    internal abstract class TypeReaderBase : ITypeReader
    {
        public abstract object ConvertFrom(Type type, string input);
        public abstract TType ConvertFrom<TType>(string input);
    }
}