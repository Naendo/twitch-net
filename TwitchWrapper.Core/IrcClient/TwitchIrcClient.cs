using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.Exceptions;

[assembly: InternalsVisibleTo("TwitchWrapper.Test")]

namespace TwitchWrapper.Core.IrcClient
{
    internal class TwitchIrcClient : IDisposable
    {
        private readonly string _host;
        private readonly TcpClient _client;
        private readonly int _port;
        private readonly CommandManager _manager;

        internal TwitchIrcClient(string host, int port)
        {
            _host = host;
            _port = port;
            _client = new TcpClient(host, _port);
            _manager = new CommandManager();
        }
        
        internal async Task SendAsync(ICommand command)
        {
            if (!_client.Connected) throw new IrcClientException("connection closed");
            var writer = new StreamWriter(_client.GetStream()) {NewLine = "\r\n"};
            await writer.WriteLineAsync(command.Parse());
            await writer.FlushAsync();
        }

        internal void StartReceive()
        {
            Task.Run(async () =>
            {
                using (var reader = new StreamReader(_client.GetStream()))
                {
                    while (_client.Connected)
                    {
                        var data = await reader.ReadLineAsync();
                        if (data is null) continue;

                        var command = _manager.DetermindCommandType(data);
                        SubscribeReceive?.Invoke(command);
                    }
                }
            });
        }

        internal event OnReceivedDelegate SubscribeReceive;

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}