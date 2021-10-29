using System.Threading.Tasks;
using TwitchNET.Core;
using TwitchNET.Core.Commands;
using TwitchNET.Core.Modules;

namespace TwitchNET.Core.Modules
{
    
    /// <summary>
    /// Provides a base class for a command module to inherit from
    /// </summary>
    public abstract class BaseModule
    {
        internal TwitchClient TwitchClient { get; set; } = null!;
        protected internal UserProxy UserProxy { get; internal set; } = null!;
        protected internal ChannelProxy ChannelProxy { get; internal set; } = null!;
        protected internal CommandProxy CommandProxy { get; internal set; } = null!;


        /// <summary>
        /// Send reply to connected chat via <see cref="TwitchClient" />
        /// </summary>
        /// <param name="message">Your response <see cref="string" /></param>
        protected async Task SendAsync(string message)
        {
            await TwitchClient.Client.SendAsync(new MessageCommand(message, ChannelProxy.Channel!));
        }
    }
}