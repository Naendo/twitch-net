using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchWrapper.Core;

namespace TwitchWrapper.Samples
{
    public class Program
    {
        static async Task Main(string[] args)
            => await new TwitchController().InitializeTwitchClient();
    }


    public class TwitchController
    {
        private readonly TwitchBot _twitchBot;

        public TwitchController()
        {
            _twitchBot = new TwitchBot();
        }

        public async Task InitializeTwitchClient()
        {
            var commander = new TwitchCommander(_twitchBot);
            
            await _twitchBot.LoginAsync("talkmaster_", "oauth:wpsvvdjj6tru7o5fxmgwgct5kd3f1x");

            await _twitchBot.JoinAsync("jetpat");
            

            await commander.InitalizeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                assembly: Assembly.GetEntryAssembly()
            );


            await Task.Delay(-1);
        }

        private static IServiceCollection BuildServiceCollection()
            => new ServiceCollection();
    }
}