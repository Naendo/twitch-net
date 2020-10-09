using System;
using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Attributes;

namespace TwitchWrapper.Sample
{
    public class QueueCommands : BaseModule
    {
        [Command("join")]
        public async Task JoinCommand()
        {
            await SendAsync($"{User.Name} has executed !join. His ChatColor is: {User.ChatColor}");
        }
    }
}