using System;
using TwitchNET.Modules.TypeReader;

namespace TwitchWrapper.Tests.MiddlewareTests.Setup
{
    public enum TestEnum
    {
        Test1,
        Test2
    }

    public class CustomTypeReader : ITypeReader
    {
        public object ConvertFrom(Type type, string input)
        {
            return Enum.Parse(type, input);
        }
    }
}