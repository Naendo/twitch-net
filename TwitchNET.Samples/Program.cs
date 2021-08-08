﻿using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TwitchNET.Modules;
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
        private readonly TwitchBot _twitchBot;

        public TwitchController()
        {
            _twitchBot = new TwitchBot();
        }

        public async Task InitializeTwitchClient()
        {
            var commander = new TwitchCommander(_twitchBot);

            await _twitchBot.LoginAsync("thatnandotho", "oauth:bw7v6tykdoazc1l2a1qcvcouvktynv");

            await _twitchBot.JoinAsync("thatnandotho");


            await commander.InitalizeCommanderAsync(
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