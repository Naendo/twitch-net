using System.ComponentModel;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.IrcClient;

namespace TwitchWrapper.Core
{
    public class TwitchBot
    {
        private readonly string _channel;
        internal readonly TwitchIrcClient Client;
        internal string Channel => _channel;
        
        public TwitchBot(string host, int port, string channel)
        {
            _channel = channel;
            Client = new TwitchIrcClient(host, port);
        }

        /// <summary>Sending authentication request to server.</summary>
        /// <param name="nick">twitch account username</param>
        /// <param name="token">oauth token</param>
        public async Task LoginAsync(string nick, string token)
        {
            await Client.SendAsync(new AuthenticateCommand(nick, token));
        }


        /// <summary>
        /// Join Twitch Channel.
        /// </summary>
        /// <param name="channel">twitch channel you want your bot to connect to</param>
        public async Task JoinAsync()
        {
            await StartListeningAsync();
            await Client.SendAsync(new JoinCommand(_channel));
            await Client.SendAsync(new UserStateCommand(_channel));
            await Client.SendAsync(new CapabilityCommand());
        }

        /// <summary>
        /// Start Listining on ChatMessages in Channel
        /// </summary>
        private Task StartListeningAsync()
        {
            Client.StartReceive();
            return Task.CompletedTask;
        }
    }
}