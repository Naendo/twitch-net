using System;
using System.Text;
using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Commands;
using Xunit;

namespace TwitchWrapper.Test
{
    public class WebSocketClientTests
    {
        private const string OAUTH = "oauth:81ne20q96iai9dwbgfn4sjgvod8e6w";


        [Fact]
        public async Task WebsocketClient_SendAsync_Receive200()
        {
            using (var client = new WebSocketClient("wss://irc-ws.chat.twitch.tv:443"))
            {
                await client.StartAsync();
                

                var wasSuccessfull = false;
                var isRegisterd = false;
                while (true)
                {
                    Task.Run(() => client.SendAsync(new AuthenticateCommand("ThatNandoTho", OAUTH)));
                    if (!isRegisterd)
                    {
                        isRegisterd = true;
                        client.SubscribeReceived += (message) => { wasSuccessfull = true; };
                    }

                    if (wasSuccessfull)
                        break;
                }

                Assert.True(true);
            }
        }
    }
}