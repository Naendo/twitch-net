using System;
using System.Reflection;
using System.Threading.Tasks;
using TwitchWrapper.Core;

namespace TwitchWrapper.Sample
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello World!");
            try
            {
                var bot = new TwitchBot("irc.twitch.tv", 6667, "thatnandotho");
                var commander = new TwitchCommander(Assembly.GetAssembly(typeof(Program)), bot);

                await commander.InitalizeCommanderAsync();

                await bot.ConnectAsync("thatnandotho", "oauth:wpsvvdjj6tru7o5fxmgwgct5kd3f1x");

                await bot.JoinAsync();

                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}