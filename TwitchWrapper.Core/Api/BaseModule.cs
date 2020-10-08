using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.Models;


namespace TwitchWrapper.Core
{
    public abstract class BaseModule
    {
        /// <summary>
        /// Injected by Reflection in <see cref="TwitchCommander"/>
        /// </summary>
        private TwitchBot _bot;

        /// <summary>
        /// Injected by Reflection in <see cref="TwitchCommander"/>
        /// </summary>
        protected UserModel User;

        
        /// <summary>
        /// Send reply to connected chat via <see cref="TwitchBot"/>
        /// </summary>
        /// <param name="message">Your response <see cref="string"/></param>
        protected async Task SendAsync(string message)
        {
            await _bot.Client.SendAsync(new MessageCommand(message, _bot.Channel));
        }
    }
}