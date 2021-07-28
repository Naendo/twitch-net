#nullable enable
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TwitchNET.Core.Commands;
using TwitchNET.Core.IrcClient;
using TwitchNET.Core.Models;

namespace TwitchNET.Core
{
    internal delegate Task LogAsyncDelegate(string message);

    /// <summary>
    /// Surface Api to manage connection states to the twitch irc chat
    /// </summary>
    public class TwitchBot
    {
        internal TwitchIrcClient Client;

        public int Type { get; set; }
        
        private readonly TwitchBotCredentials _credentials = new TwitchBotCredentials();


        /// <summary>
        /// Initiates connection with irc.twitch.tv:6667
        /// </summary>
        public TwitchBot()
        {
            InitalizeTwitchIrcClient();
        }

        private void InitalizeTwitchIrcClient()
        {
            Client = new TwitchIrcClient("irc.twitch.tv", 6667);
            Client.OnDisconnect += ReconnectHandler;
        }

        
        ///<summary>
        /// Initialized connection to the Twitch-IRC chat.
        /// </summary>
        /// <summary>Sending authentication request to server.</summary>
        /// <param name="nick">Twitch Username</param>
        /// <param name="token">Twitch OAuth Token"/></param>
        public async Task LoginAsync(string nick, string token)
        {
            _credentials.Token = token;
            _credentials.Nick = nick;

            await Client.SendAsync(new AuthenticateCommand(nick, token));
            OnLogAsync?.Invoke($"Bot authorized as [{nick}]");
        }


        /// <summary>
        /// Initializes a connection to a given twitch channel
        /// </summary>
        /// <param name="channel">Twitch Channel Name</param>
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
        ///     Leave a certained Twitch Channel
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
            OnLogAsync?.Invoke("Bot is connected..");
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