using TwitchNET.Core.Exceptions;
using TwitchNET.Core.Modules;
using Xunit;

namespace TwitchNET.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public void DuplicateCommandException_ExpectedResult()
        {
            //Arrange

            var message = "message1234";
            
            //Act

            var exception = new DuplicatedCommandException(message);


            //Assert
            Assert.IsType<DuplicatedCommandException>(exception);
            Assert.True(message.Equals(exception.Message));
        }
        
        [Fact]
        public void IrcClientException_ExpectedResult()
        {
            //Arrange

            var message = "message1234";
            
            //Act

            var exception = new IrcClientException(message);


            //Assert
            Assert.IsType<IrcClientException>(exception);
            Assert.True(message.Equals(exception.Message));
        }
        
        [Fact]
        public void TypeReaderException_ExpectedResult()
        {
            //Arrange

            var message = "message1234";
            
            //Act

            var exception = new TypeReaderException(typeof(MessageTypeReader),message);


            //Assert
            Assert.IsType<TypeReaderException>(exception);
            Assert.True($"TypeReader of type {typeof(MessageTypeReader).FullName} can not parse input: [{message}].".Equals(exception.Message));
        }
        
        
    }
}