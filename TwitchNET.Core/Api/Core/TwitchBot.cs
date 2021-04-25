using System.Threading.Tasks;
using TwitchNET.Core.Commands;
using TwitchNET.Core.IrcClient;
using TwitchNET.Core.Models;

namespace TwitchNET.Core
{
    internal delegate Task LogAsyncDelegate(string message);

    public class TwitchBot
    {
        internal readonly TwitchIrcClient Client;

        public TwitchBot()
        {
            Client = new TwitchIrcClient("irc.twitch.tv", 6667);
        }

        /// <summary>Sending authentication request to server.</summary>
        /// <param name="nick">twitch account username</param>
        /// <param name="token">oauth token</param>
        public async Task LoginAsync(string nick, string token)
        {
            await Client.SendAsync(new AuthenticateCommand(nick, token));
            OnLogAsync?.Invoke($"Bot authorized as [{nick}]");
        }


        /// <summary>
        /// Join Twitch Channel.
        /// </summary>
        /// <param name="channel">twitch channel you want your bot to connect to</param>
        public async Task JoinAsync(string channel)
        {
            await Client.SendAsync(new JoinCommand(channel));
            OnLogAsync?.Invoke($"Joined Channel: {channel}");
            await Client.SendAsync(new UserStateCommand(channel));
            await Client.SendAsync(new TagCapabilityCommand());

            await StartListeningAsync();
        }


        /// <summary>
        /// Leave Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel you want your bot to leave</param>
        public async Task PartAsync(string channel)
        {
            await Client.SendAsync(new PartCommand(channel));
            OnLogAsync?.Invoke($"Leave Channel: {channel}");
        }


        /// <summary>
        /// Start Listening on ChatMessages in Channel
        /// </summary>
        private Task StartListeningAsync()
        {
            Client.StartReceive();
            OnLogAsync?.Invoke($"Bot is now listining..");
            return Task.CompletedTask;
        }


        internal event LogAsyncDelegate? OnLogAsync;
    }
}