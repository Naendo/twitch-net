using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.IrcClient;

namespace TwitchWrapper.Core
{
    public class TwitchBot
    {
        private readonly TwitchCommandHandler _commandHandler;
        private readonly TwitchIrcClient _client;


        public TwitchBot(string host, int port, TwitchCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            _client = new TwitchIrcClient(host, port);
        }
        
        /// <summary>Sending authentication request to server.</summary>
        /// <param name="nick">twitch account username</param>
        /// <param name="token">oauth token</param>
       
        public async Task ConnectAsync(string nick, string token)
        {
            await _client.SendAsync(new AuthenticateCommand(nick, token));
        }
        
        
        /// <summary>
        /// Join Twitch Channel.
        /// </summary>
        /// <param name="channel">twitch channel you want your bot to connect to</param>
        public async Task JoinChannelAsync(string channel)
        {
            await _client.SendAsync(new JoinCommand(channel));
            await StartListeningAsync();
        }

        /// <summary>
        /// Start Listining on ChatMessages in Channel
        /// </summary>
        private Task StartListeningAsync()
        {
            _client.SubscribeReceive += _commandHandler.HandleCommandRequest;
            _client.StartReceive();

            return Task.CompletedTask;
        }
    }
}