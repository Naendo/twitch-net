using System;
using System.ComponentModel;
using TwitchNET.Core;
using TwitchNET.Core.Commands;
using Xunit;
using Xunit.Sdk;

namespace TwitchNET.Tests.CommandTests
{
    public class CommandTest
    {
        [Fact]
        public void AuthenticationCommand_ExpectedResult()
        {
            //Arrange
            var _nick = "nick";
            var _token = "token";

            var command = new AuthenticateCommand(_nick, _token);
            var implicitInterface = command as ICommand;


            var shouldResult = $"PASS oauth:{_token}{Environment.NewLine}" +
                               $"NICK {_nick}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void EchoCommand_ExpectedResult()
        {
            //Arrange

            var command = new EchoCommand();
            var implicitInterface = command as ICommand;


            var shouldResult = $"ECHO";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void JoinCommand_ExpectedResult()
        {
            //Arrange
            var channel = "channel";


            var command = new JoinCommand(channel);
            var implicitInterface = command as ICommand;


            var shouldResult = $"JOIN #{channel}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void MessageResult_ExpectedResult()
        {
            //Arrange
            var message = "message";
            var channel = "channel";

            var command = new MessageCommand(message, channel);
            var implicitInterface = command as ICommand;


            var shouldResult = $"PRIVMSG #{channel} :{message}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void PartCommand_ExpectedResult()
        {
            //Arrange
            var userName = "nick";

            var command = new PartCommand(userName);
            var implicitInterface = command as ICommand;


            var shouldResult = $"PART #{userName}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void PongCommand_ExpectedResult()
        {
            //Arrange


            var command = new PongCommand();
            var implicitInterface = command as ICommand;


            var shouldResult = "PONG:tmi.twitch.tv";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void TagCapabilityCommand_ExpectedResult()
        {
            //Arrange

            var command = new TagCapabilityCommand();
            var implicitInterface = command as ICommand;


            var shouldResult = $"CAP REQ :twitch.tv/tags {Environment.NewLine}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void UserStateCommand_ExpectedResult()
        {
            //Arrange
            var channel = "channel";

            var command = new UserStateCommand(channel);
            var implicitInterface = command as ICommand;


            var shouldResult = $":tmi.twitch.tv USERSTATE #{channel}";

            //Act
            var result = implicitInterface.Parse();

            //Assert
            Assert.True(result.Equals(shouldResult));
        }

        [Fact]
        public void Test()
        {
            var converter = TypeDescriptor.GetConverter(typeof(decimal));

            var res = converter.ConvertFrom("32,09");

            var floatConverter = TypeDescriptor.GetConverter(typeof(float)).ConvertFrom("32,02");

            var boolConverter = TypeDescriptor.GetConverter(typeof(bool)).ConvertFrom("true");

            var ushortConverter = TypeDescriptor.GetConverter(typeof(ushort)).ConvertFrom("10");

            var shortConverter = TypeDescriptor.GetConverter(typeof(short)).ConvertFrom("10");

            var charConverter = TypeDescriptor.GetConverter(typeof(char)).ConvertFrom("a");


            Assert.True(true);
        }
    }
}