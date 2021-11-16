using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Benchmarks;
using TwitchNET.Core;
using TwitchNET.Core.Responses;

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
                serviceCollection: new ServiceCollection(),
                assembly: typeof(Program).Assembly
            );
        }
        
        protected override Task HandleCommandRequest(IResponse command)
        {
            Console.WriteLine("This gets called!" + command.GetResult().Message);
            return Task.CompletedTask;
        }
    }
}