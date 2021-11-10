using System;
using TwitchNET.Core;
using TwitchNET.Core.Commands;
using Xunit;
using Xunit.Sdk;

namespace TwitchWrapper.Tests.CommandTests
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
    }
}