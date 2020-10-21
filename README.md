# Twitch .NET
<p align="center">
 <img src="https://img.shields.io/github/issues-raw/naendo/twitch-net">
 <img src="https://img.shields.io/badge/.NETCore-3.1-ff69b4.svg">
 <img src="https://img.shields.io/github/workflow/status/naendo/twitchwrapper/.NET%20Core">
 <img src="https://img.shields.io/discord/298408053970305024?logo=discord">
</p>

An unoffical .NET API Wrapper for twitch.tv

# Tutorial

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
            => new ServiceCollection();
```
