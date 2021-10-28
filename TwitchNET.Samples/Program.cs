using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Core.Modules;
using TwitchNET.Core;
using TwitchNET.Samples.Middleware;

namespace TwitchNET.Samples
{
    public static class Program
    {
        static async Task Main(string[] args)
            => await new TwitchController().InitializeTwitchClient();
    }


    public class TwitchController
    {
        private readonly TwitchClient _twitchBot;

        public TwitchController()
        {
            _twitchBot = new TwitchClient();
        }

        public async Task InitializeTwitchClient()
        {
            var commander = new TwitchCommander(_twitchBot, logOutput: LogOutput.Console);

            await _twitchBot.LoginAsync("thatnandotho", "oauth:3l85zlratpecvxc0qxq1q52el0ygpw");

            await _twitchBot.JoinAsync("thatnandotho");


            await commander.InitializeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                assembly: typeof(Program).Assembly
            );


            await Task.Delay(-1);
        }

        /* private static RequestBuilder BuildRequest() =>
             new RequestBuilder().UseMiddleware<AppendStringToFirstParameterMiddleware>();*/

        private static IServiceCollection BuildServiceCollection()
            => new ServiceCollection();
    }
}