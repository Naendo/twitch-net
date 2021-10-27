#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitchNET.Core.Commands;
using TwitchNET.Core.IrcClient;
using TwitchNET.Core.Models;

[assembly: InternalsVisibleTo("TwitchNET.Benchmarks")]

namespace TwitchNET.Core
{
    internal delegate Task LogAsyncDelegate(string message, bool isException = false);

    /// <summary>
    /// Surface Api to manage connection states to the twitch irc chat
    /// </summary>
    public class TwitchBot
    {
        internal TwitchIrcClient Client;

        private readonly TwitchBotCredentials _credentials = new TwitchBotCredentials();


        /// <summary>
        /// Initiates connection with irc.twitch.tv:6667
        /// </summary>
        public TwitchBot()
        {
            Client = new TwitchIrcClient();
            Client.OnDisconnect += ReconnectHandler;
        }


        /// <summary>
        ///  Initialized connection to the Twitch-IRC chat.
        ///  </summary>
        ///  <summary>Sending authentication request to server.</summary>
        ///  <param name="nick">Twitch Username</param>
        ///  <param name="token">Twitch OAuth Token"/></param>
        /// <param name="isReconnecting">Optional Parameter to handel Logging</param>
        public async Task LoginAsync(string nick, string token, bool isReconnecting = false)
        {
            try
            {
                await Client.ConnectAsync();

                _credentials.Token = token;
                _credentials.Nick = nick;

                await Client.SendAsync(new AuthenticateCommand(nick, token));

                if (!isReconnecting)
                    await OnLogAsync.Invoke($"Bot authorized as [{nick}]");
            }
            catch (Exception ex)
            {
                await OnLogAsync.Invoke($"Login failed with \"{ex.Message}\"", true);
            }
        }


        /// <summary>
        /// Initializes a connection to a given twitch channel
        /// </summary>
        /// <param name="channel">Twitch Channel Name</param>
        /// <param name="isReconnecting"></param>
        public async Task JoinAsync(string channel, bool isReconnecting = false)
        {
            _credentials.Channel = channel;

            await Client.SendAsync(new JoinCommand(channel));
            if (!isReconnecting)
                await OnLogAsync.Invoke($"Joined Channel: {channel}");

            await Client.SendAsync(new TagCapabilityCommand());
            StartListening();
        }


        /// <summary>
        ///     Leave a certain Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel you want your bot to leave</param>
        public async Task PartAsync(string channel)
        {
            await Client.SendAsync(new PartCommand(channel));
            await OnLogAsync.Invoke($"Leave Channel: {channel}");
            Client.OnDisconnect -= ReconnectHandler;
        }


        /// <summary>
        ///     Start Listening on ChatMessages in Channel
        /// </summary>
        private void StartListening()
        {
            Client.StartReceive();
            OnLogAsync.Invoke("Bot is connected..");
        }


        private async Task ReconnectHandler(int reconnectInterval)
        {
            await OnLogAsync.Invoke($"Reconnecting to: {_credentials.Channel}");

            await Client.ConnectAsync();
            await LoginAsync(_credentials.Nick, _credentials.Token, true);
            await JoinAsync(_credentials.Channel, true);
        }


        internal void OnInvoke(string message)
        {
            Client.OnInvoke(message);
        }

        internal event LogAsyncDelegate OnLogAsync = null!;
    }
}