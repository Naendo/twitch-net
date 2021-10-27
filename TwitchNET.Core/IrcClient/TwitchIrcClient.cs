using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Responses;

[assembly: InternalsVisibleTo("TwitchNET.Tests")]

namespace TwitchNET.Core.IrcClient
{
    internal class TwitchIrcClient : IDisposable
    {
        private const string URI = "irc.twitch.tv";
        private const int PORT = 6667;
        private TcpClient _client;
        private ResponseHandler _responseHandler;


        internal TwitchIrcClient()
        {
            _client = new TcpClient();
            _responseHandler = new ResponseHandler();
        }

        internal async Task ConnectAsync()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(URI, PORT);
        }

        public void Dispose()
        {
            _client.Close();
            _client.Dispose();
        }

        /// <summary>
        ///     Send <see cref="ICommand" />/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="IrcClientException">Ensures an open Irc-Connection</exception>
        internal async Task SendAsync(ICommand command)
        {
            if (!_client.Connected) throw new IrcClientException("connection closed");

            var writer = new StreamWriter(_client.GetStream()) {NewLine = "\r\n", AutoFlush = true};
            await writer.WriteLineAsync(command.Parse());
        }

        /// <summary>
        ///     Start Listening on <see cref="TcpClient" /> Reader
        /// </summary>
        /// <exception cref="IrcClientException"></exception>
        internal void StartReceive()
        {
            if (!_client.Connected) throw new IrcClientException($"connection aborted on {nameof(StartReceive)}");
            Task.Run(async () =>
            {
                await using var stream = _client.GetStream();
                using var reader = new StreamReader(stream);

                while (_client.Connected)
                {
                    var data = await reader.ReadLineAsync();
                    if (data is null)
                    {
                        OnDisconnect?.Invoke(1000);
                        return;
                    }

                    var response = _responseHandler.DeterminedResponseType(data);
                    if (response is null) continue;

                    SubscribeReceive?.Invoke(response);
                }
            });
        }


        internal void OnInvoke(string message)
        {
            SubscribeReceive?.Invoke(new MessageResponse(message));
        }
        
        internal event OnReceivedDelegate SubscribeReceive;
        internal event OnDisconnectDelegate OnDisconnect;
    }
}