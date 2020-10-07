using System;
using System.Text;

namespace TwitchWrapper.Core.Commands
{
    internal static class CommandParser
    {
        internal static ArraySegment<byte> ConvertToArraySegment(string command)
        {
            var buffer = Encoding.UTF8.GetBytes(command);
            return new ArraySegment<byte>(buffer);
        }
    }
}