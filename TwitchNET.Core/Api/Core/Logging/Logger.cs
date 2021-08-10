using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchWrapper.Core;

namespace TwitchNET.Core
{
    public class Logger
    {
        private LogOutput _output;

        public Logger(LogOutput logOutput)
        {
            _output = logOutput;
        }


        internal async Task OnLogHandlerAsync(string message)
        {
            switch (_output)
            {
                case LogOutput.Console:
                    await InternalLogger.LogEventsAsync(message);
                    break;
                case LogOutput.File:
                {
                    message = $"[{DateTime.Now:MM/dd/yyyy, HH:mm:ss}]: {message}";
                    await using var file = File.Create("log.txt");
                    await file.WriteAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message)));
                    break;
                }
            }
        }
    }
}