using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.Models;
using TwitchWrapper.Core.Proxies;


namespace TwitchWrapper.Core
{
    public abstract class BaseModule
    {
        internal TwitchBot TwitchBot { get; set; }
        protected internal UserProxy UserProxy { get; internal set; }
        protected internal ChannelProxy ChannelProxy { get; internal set; }
        protected internal CommandProxy CommandProxy { get; internal set; }


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