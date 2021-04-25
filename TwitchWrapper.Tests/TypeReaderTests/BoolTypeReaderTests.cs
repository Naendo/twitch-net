using TwitchWrapper.Core.Exceptions.TypeReaderException;
using TwitchWrapper.Core.TypeReader;
using Xunit;

namespace TwitchWrapper.Tests.TypeReaderTests
{
    public class BoolTypeReaderTests
    {
        private readonly TypeReader<bool> _typeReader = new BoolTypeReader();


        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void ConvertTo_WithBool(string input)
        {
            var value = _typeReader.ConvertTo(input);

            Assert.IsType<bool>(value);
        }


        [Theory]
        [InlineData("1")]
        [InlineData("0")]
        public void ConvertTo_ThrowsOnInteger(string input)
        {
            Assert.Throws<TypeReaderException>(() => _typeReader.ConvertTo(input));
        }

        [Theory]
        [InlineData("21.42")]
        [InlineData("50000.423232321141")]
        public void ConvertTo_ThrowsOnDecimals(string input)
        {
            Assert.Throws<TypeReaderException>(() => _typeReader.ConvertTo(input));
        }

        [Theory]
        [InlineData("This is a Test")]
        [InlineData("And another one")]
        public void ConvertTo_ThrowsOnString(string input)
        {
            Assert.Throws<TypeReaderException>(() => _typeReader.ConvertTo(input));
        }
    }
}