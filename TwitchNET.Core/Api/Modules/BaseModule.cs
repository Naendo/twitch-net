using System.Threading.Tasks;
using TwitchNET.Core;
using TwitchNET.Core.Commands;

namespace TwitchNET.Modules
{
    public abstract class BaseModule
    {
        internal TwitchBot TwitchBot { get; set; } = null!;
        protected internal UserProxy UserProxy { get; internal set; } = null!;
        protected internal ChannelProxy ChannelProxy { get; internal set; } = null!;
        protected internal CommandProxy CommandProxy { get; internal set; } = null!;


        /// <summary>
        ///     Send reply to connected chat via <see cref="TwitchBot" />
        /// </summary>
        /// <param name="message">Your response <see cref="string" /></param>
        protected async Task SendAsync(string message)
        {
            await TwitchBot.Client.SendAsync(new MessageCommand(message, ChannelProxy.Channel!));
        }
    }
}