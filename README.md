<p align="center">
 <img src="https://img.shields.io/github/issues-raw/naendo/twitch-net">
 <img src="https://img.shields.io/badge/.NETCore-5.0-ff69b4.svg">
 <img src="https://img.shields.io/github/workflow/status/naendo/twitchwrapper/.NET%20Core">
 <img src="https://img.shields.io/discord/298408053970305024?logo=discord">
</p>

<p align="center">A comfortable, odular and amazing library for the twitch api | <a href="https://naendo.github.io/twitch-net/">Documentation🚀</a></p>

## About

Twitch .NET is an unofficial Twitch API Wrapper targeting **.NET 6**, **.NET 5** and  **.NET Standard 2.1**.
We are currently supporting the whole **Twitch Chat API**.

Twitch .NET is still work in progress, if you are missing any features don't hesitate to [contact](#notes) me.

## Why Twitch .NET?

Twitch .NET is focusing on clean architecture, decoupling and performance for every user without having deep .NET knowledge. If you are familiar with [`Discord .NET`](https://github.com/discord-net/Discord.Net) you will easily be able to write your very own Twitch Bot in under 10 minutes.

## Benchmarks:
Coming soon..

## Installation

Install Twitch .NET via nuget [now](https://www.nuget.org/packages/TwitchNET). <img src="https://img.shields.io/nuget/dt/TwitchNET?logo=nuget">

## Basic Setup

Below are basic examples of how to utilize the Twitch .NET API. For more information please visit our [docs]("https://naendo.github.io/twitch-net/").

#### Twitch.Client

```C#
 public class Program
 {
        static async Task Main(string[] args)
            => await new TwitchController().InitializeTwitchClientAsync();
 }

 public class TwitchController
 {
       private readonly TwitchClient _client;

       public TwitchController()
       {
           _client = new TwitchClient();
       }

       public async Task InitializeAsync()
       {
           var commander = new DummyCommander(_client);
           
           await commander.InitializeAsync();
           
           await _client.LoginAsync("nick", "oauth:oauth");

           await _client.JoinAsync("channel");

           await _client.StartAsync();

           await Task.Delay(-1);
        }
```

#### Twitch.DummyCommander

```c#
 public class DummyCommander : TwitchCommander<TwitchCommander>
 {
       public TwitchCommander(TwitchClient client) : base(client, "!")
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
            => new ServiceCollection().AddSingleton<YourDependency>();
 }

```



#### Twitch.CommandModule

```C#  
public class TestModule : BaseModule
    {
        private readonly YourDependency _dependency;
        
        public TestModule(YourDependency dependency)
        {
            _dependency = dependency;
        }

        //executes on !echo {message}
        [Command("echo")]
        public async Task TestCommand(string value)
        {
            await _dependency.AddAsync(value);
            await SendAsync($"{UserProxy.Name} sent a message: {value}");
        }

    }

```


## Notes
If you expirence any issues while using Twitch .NET, dont hesitate to contact me via Discord `ThatNandoTho#1852`, or just report an issue!

