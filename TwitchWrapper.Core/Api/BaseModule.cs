using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;

namespace TwitchWrapper.Core
{
    public abstract class BaseModule
    {
        private TwitchBot _bot;

        protected bool IsBroadcaster { get; private set; }

        protected bool IsModerator { get; private set; }

        protected bool IsVip { get; private set; }
        
        
        protected async Task SendAsync(string message)
        {
            await _bot.Client.SendAsync(new MessageCommand(message, _bot.Channel));
        }
    }
}