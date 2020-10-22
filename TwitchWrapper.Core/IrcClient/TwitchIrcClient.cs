using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitchWrapper.Core.Exceptions;

[assembly: InternalsVisibleTo("TwitchWrapper.Test")]

namespace TwitchWrapper.Core.IrcClient
{
    internal class TwitchIrcClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly ResponseHandler _responseHandler;
        private bool IsListening = false;

        internal TwitchIrcClient(string host, int port)
        {
            _client = new TcpClient(host, port);
            _responseHandler = new ResponseHandler();
        }

        /// <summary>
        /// Send <see cref="ICommand"/>/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="IrcClientException"></exception>
        internal async Task SendAsync(ICommand command)
        {
            if (!_client.Connected) throw new IrcClientException("connection closed");

            var writer = new StreamWriter(_client.GetStream()) {NewLine = "\r\n"};
            await writer.WriteLineAsync(command.Parse());
            await writer.FlushAsync();
        }

        /// <summary>
        /// Start Listening on <see cref="TcpClient"/> Reader
        /// </summary>
        /// <exception cref="IrcClientException"></exception>
        internal void StartReceive()
        {
            if (!_client.Connected) throw new IrcClientException($"connection aborted on {nameof(StartReceive)}");
            if (IsListening) return;
            Task.Run(async () =>
            {
                IsListening = true;
                using var reader = new StreamReader(_client.GetStream());
                while (_client.Connected)
                {
                    try
                    {
                        var data = await reader.ReadLineAsync();
                        if (data is null) continue;

                        var response = _responseHandler.DeterminedResponseType(data);
                        if (response is null) continue;

                        SubscribeReceive?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
        });
    }

    internal event OnReceivedDelegate SubscribeReceive;

    public void Dispose()
    {
    _client.Close();
    _client.Dispose();
    }
}

}