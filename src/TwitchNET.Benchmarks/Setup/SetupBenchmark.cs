using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchNET.Core;
using TwitchNET.Sample;
using TwitchClient = TwitchNET.Core.TwitchClient;

namespace TwitchNET.Benchmarks.TwitchNET.Setup
{
    [WarmupCount(40)]
    [LongRunJob(RuntimeMoniker.Net50)]
    public class SetupBenchmark
    {
        [Benchmark]
        public async Task CompleteSetup_TwitchNET()
        {
            var bot = new TwitchClient();
            
            var commander = new DummyCommander(bot);

        
        }

        [Benchmark]
        public void CompleteSetup_TwitchLib()
        {
            TwitchLib.Client.TwitchClient client;
            ConnectionCredentials credentials = new ConnectionCredentials("twitch_username", "access_token");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new  TwitchLib.Client.TwitchClient(customClient);
            client.Initialize(credentials, "channel");

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnNewSubscriber(object? sender, OnNewSubscriberArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnLog(object? sender, OnLogArgs e)
        {
            throw new NotImplementedException();
        }
    }
}