using System.Threading.Tasks;
using TwitchNET.Modules;

namespace TwitchNET.Samples
{
    public class TestModule : BaseModule
    {
        //Triggers on !test value

        [Command("test")]
        public async Task TestCommand(string value)
        {
            var test = CommandProxy;
            await SendAsync($"{UserProxy.Name} sent a message: {value}");
        }
    }
}