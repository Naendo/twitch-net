using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using TwitchNET.Core.Delegates;
using TwitchNET.Core.Exceptions;

namespace TwitchNET.Core.IrcClient
{
    internal class TwitchSocketClient
    {
        private const string URI = "irc.twitch.tv";
        private const int PORT = 6667;

        private readonly TwitchClient _client;
        private TcpClient _tcpClient;

        internal TwitchSocketClient(TwitchClient client)
        {
            _client = client;
            _tcpClient = new TcpClient();
        }

        internal async Task ConnectAsync()
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(URI, PORT);
        }

        internal async Task ReconnectAsync()
        {
            var timer = 1;
            do
            {
                await ConnectAsync();
                await Task.Delay(timer * 1000);
                if (timer == 1)
                    timer++;
                else
                    timer *= timer;
            } while (!_tcpClient.Connected);
        }

        internal async Task SendAsync(ICommand command)
        {
            if (!_tcpClient.Connected) throw new IrcClientException("connection closed");

            var writer = new StreamWriter(_tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
            await writer.WriteLineAsync(command.Parse());
        }

        internal Task ReadAsync()
        {
            Task.Run(async () =>
            {
                try
                {
                    await using var stream = _tcpClient.GetStream();
                    using var reader = new StreamReader(stream);

                    while (_tcpClient.Connected)
                    {
                        var data = await reader.ReadLineAsync();
                        if (data is null)
                        {
                            await ReconnectAsync();
                            return;
                        }

                        var response = ResponseHandler.DeterminedResponseType(data);
                        if (response is null) continue;

                        OnMessageReceive?.Invoke(response);
                    }
                }
                catch (Exception ex)
                {
                    await _client.OnLogAsync(ex);
                }
            });

            return Task.CompletedTask;
        }


        internal event OnReceivedDelegate OnMessageReceive;
    }
}