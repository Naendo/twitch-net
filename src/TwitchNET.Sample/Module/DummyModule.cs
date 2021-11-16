﻿using System.Threading.Tasks;
using TwitchNET.Core.Modules;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample.Module
{
    public class DummyModule : BaseModule<DummyCommander>
    {
        private readonly DummyService _service;

        public DummyModule(DummyService service) : base()
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