using System.Threading.Tasks;
using TwitchNET.Core.Modules;

namespace TwitchNET.Samples
{
    public class TestModule : BaseModule
    {
        //Triggers on !test value

        [Command("test")]
        public async Task TestCommand(int value = default)
        {
            var test = CommandProxy;
            await SendAsync($"{UserProxy.Name} sent a message: {value}");
        }
    }
}