#nullable enable
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TwitchNET.Core.Commands;
using TwitchNET.Core.IrcClient;
using TwitchNET.Core.Models;

namespace TwitchNET.Core
{
    internal delegate Task LogAsyncDelegate(string message);

    public class TwitchBot
    {
        internal TwitchIrcClient Client;

        public int Type { get; set; }


        private TwitchBotCredentials _credentials = new TwitchBotCredentials();


        public TwitchBot()
        {
            InitalizeTwitchIrcClient();
        }

        private void InitalizeTwitchIrcClient()
        {
            Client = new TwitchIrcClient("irc.twitch.tv", 6667);
            Client.OnDisconnect += ReconnectHandler;
        }

        /// <summary>Sending authentication request to server.</summary>
        /// <param name="nick">twitch account username</param>
        /// <param name="token">oauth token</param>
        public async Task LoginAsync(string nick, string token)
        {
            _credentials.Token = token;
            _credentials.Nick = nick;

            await Client.SendAsync(new AuthenticateCommand(nick, token));
            OnLogAsync?.Invoke($"Bot authorized as [{nick}]");
        }


        /// <summary>
        ///     Join Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel</param>
        public async Task JoinAsync(string channel)
        {
            _credentials.Channel = channel;

            await Client.SendAsync(new JoinCommand(channel));
            OnLogAsync?.Invoke($"Joined Channel: {channel}");
            await Client.SendAsync(new UserStateCommand(channel));
            await Client.SendAsync(new TagCapabilityCommand());

            await StartListeningAsync();
        }


        /// <summary>
        ///     Leave Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel you want your bot to leave</param>
        public async Task PartAsync(string channel)
        {
            await Client.SendAsync(new PartCommand(channel));
            OnLogAsync?.Invoke($"Leave Channel: {channel}");
            Client.OnDisconnect -= ReconnectHandler;
        }


        /// <summary>
        ///     Start Listening on ChatMessages in Channel
        /// </summary>
        private Task StartListeningAsync()
        {
            Client.StartReceive();
            OnLogAsync?.Invoke("Bot is now listining..");
            return Task.CompletedTask;
        }


        private async Task ReconnectHandler(int reconnectInterval)
        {
            InitalizeTwitchIrcClient();

            await Task.Delay(reconnectInterval * 1000);
            OnLogAsync?.Invoke($"Reconnecting to: {_credentials.Channel}");
            await LoginAsync(_credentials.Nick, _credentials.Token);
            await JoinAsync(_credentials.Channel);
        }


        internal event LogAsyncDelegate OnLogAsync;
    }
}