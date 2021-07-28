﻿using System;
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
        private readonly TcpClient _client;
        private readonly ResponseHandler _responseHandler;
        private bool _isListening;
        private readonly int _reconnectInterval = 2;


        internal TwitchIrcClient(string host, int port)
        {
            _client = new TcpClient(host, port);
            _responseHandler = new ResponseHandler();
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

            var writer = new StreamWriter(_client.GetStream()) {NewLine = "\r\n"};
            await writer.WriteLineAsync(command.Parse());
            await writer.FlushAsync();
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
                _isListening = true;
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


                        if (_client.Connected == false)
                        {
                            _client.Close();
                            OnDisconnect?.Invoke(_reconnectInterval);
                        }
                    }
                    catch (Exception ex)
                    {
                        await InternalLogger.LogExceptionAsync(ex);
                    }
                }
            });
        }


        internal event OnReceivedDelegate SubscribeReceive;
        internal event OnDisconnectDelegate OnDisconnect;
    }
}