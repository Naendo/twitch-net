using TwitchNET.Modules;
using TwitchNET.Core.Exceptions.TypeReaderException;
using Xunit;

namespace TwitchNET.Tests.TypeReaderTests
{
    public class DoubleTypeReaderTests
    {
        private readonly BaseTypeReader _baseTypeReader = new DoubleBaseTypeReader();


        [Theory]
        [InlineData("21.4241")]
        [InlineData("50000.423232321141")]
        public void ConvertTo_WithDecimal(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<double>(value);
        }

        [Theory]
        [InlineData("153")]
        [InlineData("123120")]
        public void ConvertTo_WithInteger(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<double>(value);
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void ConvertTo_ThrowsOnBoolean(string input)
        {
            Assert.Throws<TypeReaderException>(() => _baseTypeReader.ConvertTo(input));
        }

        [Theory]
        [InlineData("This is a Test")]
        [InlineData("And another one")]
        public void ConvertTo_ThrowsOnString(string input)
        {
            Assert.Throws<TypeReaderException>(() => _baseTypeReader.ConvertTo(input));
        }
    }
}