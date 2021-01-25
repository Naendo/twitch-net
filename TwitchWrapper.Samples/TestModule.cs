using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Attributes;

namespace TwitchWrapper.Samples
{
    public class TestModule : BaseModule
    {
        //Triggers on !test value
        [Command("test")]
        public async Task TestCommand(string value)
        {
            await SendAsync($"{UserProxy.Name} sent a message: {value}");
        }
    }
}