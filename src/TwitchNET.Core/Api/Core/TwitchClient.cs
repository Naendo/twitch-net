using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using TwitchNET.Core.Commands;
using TwitchNET.Core.Delegates;
using TwitchNET.Core.IrcClient;
using TwitchNET.Core.Logging;
using TwitchNET.Core.Models;

namespace TwitchNET.Core
{
    public class TwitchClient
    {
        private string _nick;
        private string _token;
        private string _connectedChannel;

        internal readonly TwitchSocketClient Client;

        public TwitchClient(LogOutput logOutput = LogOutput.Console)
        {
            Client = new TwitchSocketClient(this);
            var logger = new Logger(logOutput);
            LogAsync += logger.OnLogHandlerAsync;
        }

        /// <summary>
        ///  Initialized connection to the Twitch-IRC chat.
        ///  </summary>
        ///  <summary>Sending authentication request to server.</summary>
        ///  <param name="nick">Username on Twitch</param>
        ///  <param name="token">OAuth2 Token. Recommended format oauth:token</param>
        public async Task LoginAsync(string nick, string token, bool isReconnecting = false)
        {
            try
            {
                await Client.ConnectAsync();

                _token = token;
                _nick = nick;

                await Client.SendAsync(new AuthenticateCommand(nick, token));

                if (!isReconnecting)
                    await LogAsync?.Invoke($"Bot authorized as [{nick}]");
            }
            catch (Exception ex)
            {
                await LogAsync?.Invoke($"Login failed with \"{ex.Message}\"", true);
            }
        }


        /// <summary>
        /// Initializes a connection to a given twitch channel
        /// </summary>
        /// <param name="channel">Twitch Channel Name</param>
        /// <param name="isReconnecting"></param>
        public async Task JoinAsync(string channel, bool isReconnecting = false)
        {
            _connectedChannel = channel;

            await Client.SendAsync(new JoinCommand(channel));
            if (!isReconnecting)
                await LogAsync?.Invoke($"Joined Channel: {channel}");

            await Client.SendAsync(new TagCapabilityCommand());
        }

        /// <summary>
        /// <see cref="StartAsync"/> will start the <see cref="TcpListener"/> to await content from Twitch.
        /// </summary>
        public async Task StartAsync()
        {
            await Client.ReadAsync();
        }

        /// <summary>
        ///Leave a certain Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel you want your bot to leave</param>
        public async Task PartAsync(string channel)
        {
            await Client.SendAsync(new PartCommand(channel));
            await LogAsync?.Invoke($"Leave Channel: {channel}");
        }

        internal Task OnLogAsync(Exception exception)
        {
            LogAsync?.Invoke(exception.Message, true);
            return Task.CompletedTask;
        }

        internal Task OnLogAsync(string message)
        {
            LogAsync?.Invoke(message);

            return Task.CompletedTask;
        }

        internal event LogAsyncDelegate LogAsync;
    }
}