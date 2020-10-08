using System;
using System.Reflection;
using System.Threading.Tasks;
using TwitchWrapper.Core;

namespace TwitchWrapper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var bot = new TwitchBot("irc.twitch.tv", 6667,
                new TwitchCommandHandler(Assembly.GetAssembly(typeof(Program))));


            await bot.ConnectAsync("thatnandotho", "oauth:0vd62p7uvt1u7dflg4lb268d03ymfz");
            await bot.JoinChannelAsync("thatnandotho");

            await Task.Delay(-1);
        }
    }
}