## About

Twitch .NET is an unoffical Twitch API Wrapper targeting .NET Core 3.1. 
Currently supporting full capabilities of Twitchs IRC Chat.

## Why Twitch .NET?

Twitch .NET focusing on clean code archtitecture, decoupling and performance. <br/>
Full Dependency Injection support.

Benchmarks:
Coming soon..

## Installation

Install Twitch .NET via nuget [now](_blank). <img src="https://img.shields.io/nuget/dt/_blank?logo=nuget">

## Basic Setup

Below are basic examples of how to utilize the Twitch .NET API.

#### Twitch.Client

```C#
 public class Program
    {
        static async Task Main(string[] args)
            => await new TwitchController().InitializeTwitchClientAsync();
    }


    public class TwitchController
    {
        private readonly TwitchBot _twitchBot;

        public TwitchController()
        {
            _twitchBot = new TwitchBot();
        }

        public async Task InitializeTwitchClientAsync()
        {
            await _twitchBot.LoginAsync("nick", "oauth:token");

            await _twitchBot.JoinAsync("yourChannel");


            var commander = new TwitchCommander(_twitchBot);

            await commander.InitalizeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                assembly: Assembly.GetEntryAssembly()
            );
        }

        private static IServiceCollection BuildServiceCollection()
            => new ServiceCollection()
            .AddSingelton<YourDependency>();
```


#### Twitch.Command

```C#  
public class TestModule : BaseModule
    {
        private readonly YourDependency _dependency;
        
        public TestModule(YourDependency dependency)
        {
            _dependency = dependency;
        }


        //Triggers on !test textAfterCommand
        [Command("test")]
        public async Task TestCommand(string value)
        {
            await _dependency.AddAsync(value);
            await SendAsync($"{UserProxy.Name} sent a message: {value}");
        }



```
