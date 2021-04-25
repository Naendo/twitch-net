using TwitchWrapper.Core.Exceptions.TypeReaderException;
using TwitchWrapper.Core.TypeReader;
using Xunit;

namespace TwitchWrapper.Tests.TypeReaderTests
{
    public class DoubleTypeReaderTests
    {
        private readonly TypeReader<double> _typeReader = new DoubleTypeReader();


        [Theory]
        [InlineData("21.4241")]
        [InlineData("50000.423232321141")]
        public void ConvertTo_WithDecimal(string input)
        {
            var value = _typeReader.ConvertTo(input);

            Assert.IsType<double>(value);
        }

        [Theory]
        [InlineData("153")]
        [InlineData("123120")]
        public void ConvertTo_WithInteger(string input)
        {
            Assert.Throws<TypeReaderException>(() => _typeReader.ConvertTo(input));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void ConvertTo_ThrowsOnBoolean(string input)
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