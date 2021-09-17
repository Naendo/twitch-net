using System;
using System.Threading.Tasks;

namespace TwitchWrapper.Core
{
    public class InternalLogger
    {
        internal static Task LogExceptionAsync(Exception message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:MM/dd/yyyy, HH:mm:ss}]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        internal static Task LogEventsAsync(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{DateTime.Now:MM/dd/yyyy, HH:mm:ss}]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}