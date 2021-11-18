using System.Threading.Tasks;
using TwitchNET.Core.Modules;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample.Module
{
    public class DummyModule : BaseModule<DummyCommander>
    {
        private readonly TwitchService _service;

        public DummyModule(TwitchService service) : base()
        {
            _service = service;
        }

        
        [Command("say")]
        public async Task EchoCommandAsync(string echo)
        {
            await SendAsync(echo);
        }
    }
}