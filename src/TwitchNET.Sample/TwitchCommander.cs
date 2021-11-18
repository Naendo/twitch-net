using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Core;
using TwitchNET.Core.Extensions;
using TwitchNET.Core.Responses;
using TwitchNET.Sample.Middleware;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample
{
    public class DummyCommander : TwitchCommander<DummyCommander>
    {
        public DummyCommander(TwitchClient client) : base(client, "!")
        {
        }

        public async Task InitializeAsync()
        {
            await InitializeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                typeof(Program).Assembly,
                BuildRequestPipeline()
            );
        }

        protected override async Task HandleCommandRequest(IResponse command)
        {
            await base.HandleCommandRequest(command);
        }

        private static PipelineBuilder BuildRequestPipeline()
            => new PipelineBuilder()
                .UseMiddleware<TwitchMiddleware>();

        private static IServiceCollection BuildServiceCollection()
            => new ServiceCollection().AddSingleton<TwitchService>();
    }
}