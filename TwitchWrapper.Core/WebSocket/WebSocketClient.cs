using System;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using Websocket.Client;

[assembly: InternalsVisibleTo("TwitchWrapper.Test")]

namespace TwitchWrapper.Core
{
    internal class WebSocketClient : IDisposable
    {
        private readonly WebsocketClient _client;

        public WebSocketClient(string uri)
        {
            _client = new WebsocketClient(new Uri(uri)) {ReconnectTimeout = TimeSpan.FromSeconds(30)};
            _client.ReconnectionHappened.Subscribe(info =>
                Console.WriteLine($"[{DateTime.Now.ToLongDateString()}] Reconnected"));
        }

        public WebSocketClient(Uri uri)
        {
            _client = new WebsocketClient(uri);
        }


        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await _client.Start();
        }


        /// <summary>
        /// Send <see cref="ICommand"/>
        /// </summary>
        internal Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            var exitEvent = new ManualResetEvent(false);
            _client.Send(command.Parse());
            exitEvent.WaitOne(TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        private void ReceiveAsync(CancellationToken cancellationToken = default)
        {
            /*Task.Run(async () =>
            {
                var buffer = new byte[1024];

                while (_client.State == WebSocketState.Open)
                {
                    try
                    {
                        await _client.ReceiveAsync(buffer, cancellationToken);
                        SubscribeReceived?.Invoke(await SocketResultParser.GetResultAsync(buffer));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }, cancellationToken);*/
        }

        /// <summary>
        /// Handle Twitchs regular PING requests
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
       /* private void HandlePingAsync(CancellationToken cancellationToken = default)
        {
            SubscribeReceived += async (message) =>
            {
                if (message == "PING :tmi.twitch.tv")
                {
                    await SendAsync(new PongCommand(), cancellationToken);
                }
            };
        }*/

        public void Dispose()
        {
            _client?.Dispose();
        }


        public event OnReceivedDelegate SubscribeReceived;
    }
}