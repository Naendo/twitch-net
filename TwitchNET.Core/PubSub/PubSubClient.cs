using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchNET.Core.PubSub
{
    internal class PubSubClient
    {
        private readonly ClientWebSocket _clientWebSocket;
        private readonly Uri _uri;

        public PubSubClient(string host)
        {
            _clientWebSocket = new ClientWebSocket();
            _uri = new Uri(host.StartsWith("wss://") ? host : $"wss://{host}");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _clientWebSocket.ConnectAsync(_uri, cancellationToken);
        }


        private async Task StartReceivingAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (_clientWebSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024];
                    await _clientWebSocket.ReceiveAsync(buffer, cancellationToken);

                    var result = Encoding.Default.GetString(buffer);
                    ReceivePubSubEvent?.Invoke(result);
                }
            }, cancellationToken);
        }


        internal event ReceivePubSubDelegate? ReceivePubSubEvent;
    }
}