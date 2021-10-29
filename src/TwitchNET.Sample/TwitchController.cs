using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Core;
using TwitchNET.Core.Extensions;
using TwitchNET.Sample.Middleware;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample
{
    public class TwitchController
    {
        private readonly TwitchClient _client;

        public TwitchController()
        {
            _client = new TwitchClient();
        }

        public async Task InitializeAsync()
        {
            var commander = new TwitchCommander(_client);


            await _client.LoginAsync("nick", "oauth:token");

            await _client.JoinAsync("channel");

            await _client.StartAsync();


            await commander.InitializeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                typeof(Program).Assembly,
                middlewareBuilder: BuildRequestPipeline()
            );
        }


        private static PipelineBuilder BuildRequestPipeline()
            => new PipelineBuilder().UseMiddleware<DummyMiddleware>();

        private static IServiceCollection BuildServiceCollection()
            => new ServiceCollection().AddSingleton<DummyService>();
    }
}