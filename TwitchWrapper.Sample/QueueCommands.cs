using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Attributes;

namespace TwitchWrapper.Sample
{
    public class QueueCommands : BaseModule
    {
        private readonly TestService _testService;

        public QueueCommands(TestService testService)
        {
            _testService = testService;
        }
        
        [Command("join")]
        public async Task JoinCommand()
        {
            await SendAsync($"{UserProxy.Name} has executed !join. His ChatColor is: {UserProxy.Color}");
        }
    }
}