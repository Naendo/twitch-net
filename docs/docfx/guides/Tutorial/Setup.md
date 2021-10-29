---
uid: Guides.GettingStarted.Setup
title: Setup for Twitch .NET
---

## Twitch .NET Setup

One of the best ways to get started with Twitch .NET is to write a basic echo bot. This bot will respond to a simple command !say 'Whatever you want to say' with an exact copy of your message. We will expand on this to create more diverse commands later, but for now, it is a good starting point.

### Create a Twitch Bot

Before you write your very own bot, we recommend creating a new account via [twitch.tv]("https://www.twitch.tv/").

1. Create your [account]("https://www.twitch.tv/").
2. Now get your OAuth Token for your bot via. a token [generator]("https://twitchapps.com/tmi/") for example. You can also write a service to get the Token though the [Twitch API]("https://dev.twitch.tv/docs/irc/guide"). This 'll be a inbuild feature in future releases.
3. Get ready to code! 🚀

### Connect to Twitch

If you haven't already created a project and installed Twitch .NET, do that now.

For more information, see [Installing Twitch .NET]("").

#### Async

Twitch .NET uses .NET's [Asynchronous Pattern]("https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/") in pretty much every operation. In normal cases it is necessary to await every operation that can be, to establish an async context whenever possible. 

To create an async context right away, we will rewrite the static main method to handle async operations.

```c#
public static class Program
{
    static async Task Main(string[] args)
        => await SetupAsync();
    
    public async static Task SetupAsync()
    {
    }
}
```

#### Creating a Twitch Client

Now the <strong>fun</strong> begins.😋

To get started we will establish a connection to Twitch using our [TwitchClient]("").

> [!IMPORTANT]
>
> We want to clarify how important the security of your OAuth Token really is. Giving away your OAuth Token can be compared to  telling everyone your password. Make sure to store it safely!



```c#
     public static async Task SetupAsync()
        {
            var twitchClient = new TwitchClient();
            
            // You can assign your oauth token to a string, and pass that in to connect.
            //This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            
            //Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>                     	(File.ReadAllText("config.json")).Token;
            
            await _twitchBot.LoginAsync("nick", "oauth:token");
            
            await _twitchBot.JoinAsync("channel");

            await _twitchBot.StartAsync();
         
        
            // This Line blockes the task until the program is finally closed.
            await Task.Delay(-1);
        }   
      
```

At this point, we established a working connection! We'll now move on, to building our very own commands.

#### Adding our Commands

Creating commands for your bot was never easier. We'll guide you through a few steps and explain some functionality behind the szene.

##### Step 1: Build the Command

To build commands we use a base class called [BaseModule](""). It will later provide us with information about the command context and it is also necessary to later register your commands with our [TwitchCommander]("").

```c#
public class EchoModule : BaseModule
{
    [Command("say")]
    public async Task EchoCommandAsync(string value)
    {
        await SendAsync($"{UserProxy.Name} sent a message: {value}");
    }
}
```

To break down the code. 

- The attribute [Command]("") registers our method as an command. It's parameter defines a keyword. Through this keyword, the system will invoke the method when the right command was sent.

- The method name is completely up to you. There are no naming schemes in Twitch .NET.

- In our example, the method parameter is a string. Parameters are by default divided by a Space -> " ". To trigger this exact method the message has to look like `!say ThisIsAMessage`. It is important to not have spaces between your words if you work with one parameter. Although, if a dynamic number of parameters is required, we offer full [IEnumerable]("https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-5.0") support for parameters.

- The base class gives us the functionality of `await SendAsync()`. This method will return a string back to the twitch channel. It also gives us the functionality of [UserProxy](""). Via proxies we can get informtion about the user, for example its name, chat color, subscriber status and so on.

  



Congratulations👏 You just built your very first command with Twitch .NET. 

> [!NOTE]
>
> If you wish to see the full capability and functionality of the Command Framework please visit our [Docs]("").

##### Step 2: Register the Command

To put the whole framework together, Twitch .NET includes our [TwitchCommander](""). His main mission is to register commands and map incoming command context to the correct method. 

```C#
public async static Task SetupAsync()
{
    // ...
        
    var commander = new TwitchCommander(_twitchBot);   
    
    await commander.InitializeCommanderAsync(
        serviceCollection: BuildServiceCollection(),
        assembly: typeof(Program).Assembly
    );
    
    await Task.Delay(-1);
}

private static IServiceCollection BuildServiceCollection()
    => new ServiceCollection();
```

With these lines added your bot is ready and working but lets no get to excited yet.

What exactly does this code do. Well, first we initialize the Commander and tell him which bot he is working for. (Yes, this implies you can have multiple bots running side-by-side!) Next, in which assembly he can find our commands and last but not least, we tell him if there are any services to inject. (ref. [Dependency Injection](""))