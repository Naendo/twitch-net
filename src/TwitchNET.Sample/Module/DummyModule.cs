using System.Threading.Tasks;
using TwitchNET.Core.Modules;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample.Module
{
    [Prefix("!")]
    public class DummyModule : BaseModule
    {
        private readonly DummyService _service;

        public DummyModule(DummyService service)
        {
            _service = service;
        }


        [Command("say")]
        public async Task EchoCommandAsync(string echo)
        {
            var result = _service.TransformResponse(echo);

            await SendAsync(result);
        }
    }
}