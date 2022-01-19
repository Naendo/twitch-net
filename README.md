<p align="center">
 <img src="https://img.shields.io/github/issues-raw/naendo/twitch-net">
 <img src="https://img.shields.io/badge/.NETCore-5.0-ff69b4.svg">
 <img src="https://img.shields.io/github/workflow/status/naendo/twitchwrapper/.NET%20Core">
 <img src="https://img.shields.io/discord/298408053970305024?logo=discord">
</p>

<p align="center">A comfortable, modular and amazing library for the twitch api | <a href="https://naendo.github.io/twitch-net/">Documentation🚀</a></p>

## About

Twitch .NET is an unoffical Twitch API Wrapper targeting .netstandard 2.1. 
Currently supporting full capabilities of Twitchs IRC Chat.

Twitch .NET is still work in progress, if you are missing any features dont hesitate to [contact](#notes) me.

## Why Twitch .NET?

Twitch .NET is focusing on clean archtitecture, decoupling and performance for every user by default.<br/>
We offer full [`Dependency Injection`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1) support.

If you are familiar with [`Discord .NET`](https://github.com/discord-net/Discord.Net) you will easily be able to write your very own Twitch Bot in under 10 minutes.


Benchmarks:
Coming soon..

## Installation

Install Twitch .NET via nuget [now](https://www.nuget.org/packages/TwitchNET). <img src="https://img.shields.io/nuget/dt/TwitchNET?logo=nuget">

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
       var commander = new TwitchCommander(_twitchBot);

       await _twitchBot.LoginAsync("nick", "oauth:token");

       await _twitchBot.JoinAsync("yourChannel");

       await commander.InitalizeCommanderAsync(
                serviceCollection: BuildServiceCollection(),
                assembly: Assembly.GetEntryAssembly()
       );
            
       await Task.Delay(-1);
 }

       private static IServiceCollection BuildServiceCollection()
           => new ServiceCollection()
              .AddSingelton<YourDependency>();
 }
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

  }

```


## Notes
If you expirence any issues while using Twitch .NET, dont hesitate to contact me via Discord `ThatNandoTho#1852`, or just report an issue!

