using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using TwitchNET.Core;
using TwitchNET.Modules;
using TwitchWrapper.Tests.MiddlewareTests.Setup;
using Xunit;

namespace TwitchWrapper.Tests.MiddlewareTests.ProxyTests
{
    public class ProxyFactoryTests
    {
        [Fact]
        public void ProxySetter_Intended()
        {
            var module = new DummyModule();

            var baseModule = (BaseModule) module;

            module.ChannelProxy = new ChannelProxy{
                Channel = "test"
            };

            module.CommandProxy = new CommandProxy(){
                Message = "this is a message ;)"
            };

            module.TwitchBot = new TwitchBot();

            module.UserProxy = new UserProxy{
                Color = "green",
                IsBroadcaster = false,
                IsModerator = true,
                IsSubscriber = true,
                IsTurbo = false,
                IsVip = false,
                Name = "testName"
            };


            Assert.NotNull(baseModule.ChannelProxy);
            Assert.NotNull(baseModule.CommandProxy);
            Assert.NotNull(baseModule.TwitchBot);
            Assert.NotNull(baseModule.UserProxy);
        }
    }
}