﻿using System.Threading.Tasks;
using TwitchNET.Core.Modules;
using TwitchNET.Sample.Services;

namespace TwitchNET.Sample.Module
{
    public class AnotherModule : BaseModule<AnotherCommander>
    {
        private readonly TwitchService _service;

        public AnotherModule(TwitchService service)
        {
            _service = service;
        }


        [Command("say")]
        public async Task EchoCommandAsync(string echo)
        {
            await SendAsync(string.Join(" ", echo));
        }
    }
}