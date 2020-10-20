using System;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchWrapper.Core;

namespace TwitchWrapper.Sample
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                var bot = new TwitchBot();

                var commander = new TwitchCommander(Assembly.GetAssembly(typeof(Program)),
                    bot);

                var serviceCollection = BuildServiceProvider();

                await commander.InitalizeCommanderAsync(serviceCollection);

                await bot.LoginAsync("thatnandotho", "oauth:wpsvvdjj6tru7o5fxmgwgct5kd3f1x");

                await bot.JoinAsync("niewoooooo");


                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static IServiceCollection BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton<TestService>();
        }
    }
}