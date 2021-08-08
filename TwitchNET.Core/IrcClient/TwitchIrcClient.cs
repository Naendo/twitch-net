﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Responses;
using TwitchWrapper.Core;

[assembly: InternalsVisibleTo("TwitchNET.Tests")]

namespace TwitchNET.Core.IrcClient
{
    internal class TwitchIrcClient : IDisposable
    {
        private TcpClient _client;
        private ResponseHandler _responseHandler;
        private bool _isListening;
        private readonly int _reconnectInterval = 2;


        internal TwitchIrcClient()
        {

        }

        public void Dispose()
        {
            _client.Close();
            _client.Dispose();
        }

        internal void InitializeIrcClient(string host, int port)
        {
            _client = new TcpClient(host, port);
            _responseHandler = new ResponseHandler();
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
            if (_isListening) return;
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
                        break;
                    }

                    var response = _responseHandler.DeterminedResponseType(data);
                    if (response is null) continue;

                    SubscribeReceive?.Invoke(response);
                }
            });
        }


        internal event OnReceivedDelegate SubscribeReceive;
        internal event OnDisconnectDelegate OnDisconnect;
    }
}