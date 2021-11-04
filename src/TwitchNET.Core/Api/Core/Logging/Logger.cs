using System;
using System.Threading.Tasks;

namespace TwitchNET.Core.Logging
{
    /// <summary>
    /// <see cref="Logger"/> acts as an internal Logging Handler.
    /// The logger gets configured on <see cref="TwitchClient"/> initialisation.
    /// Is uses <see cref="LogOutput"/> to clarify whether the logger targets the system console or the log file "twitch.log". 
    /// </summary>
    internal class Logger
    {
        private const string PATH = "twitch.log";
        private readonly LogOutput _output;

        public Logger(LogOutput logOutput)
        {
            _output = logOutput;
        }


        internal async Task OnLogHandlerAsync(string message, bool isException)
        {
            switch (_output)
            {
                case LogOutput.Console when isException:
                    await InternalLogger.LogExceptionAsync(new Exception(message));
                    break;
                case LogOutput.Console:
                    await InternalLogger.LogEventsAsync(message);
                    break;
                case LogOutput.File:
                    break;
            }
        }
    }
}