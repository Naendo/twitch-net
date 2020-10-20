using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.Models;
using TwitchWrapper.Core.Proxies;


namespace TwitchWrapper.Core
{
    public abstract class BaseModule
    {
        private TwitchBot TwitchBot { get; set; }
        protected UserProxy UserProxy { get; private set; }
        protected ChannelProxy ChannelProxy { get; private set; }

        protected CommandProxy CommandProxy { get; private set; }


        /// <summary>
        /// Send reply to connected chat via <see cref="TwitchBot"/>
        /// </summary>
        /// <param name="message">Your response <see cref="string"/></param>
        protected async Task SendAsync(string message)
        {
            await TwitchBot.Client.SendAsync(new MessageCommand(message, ChannelProxy.Channel));
        }
    }
}