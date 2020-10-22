﻿using System;
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
            
            await _twitchBot.LoginAsync("nick", "token");

            await _twitchBot.JoinAsync("channel");
            

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