using TwitchWrapper.Core.Exceptions.TypeReaderException;
using TwitchWrapper.Core.TypeReader;
using Xunit;

namespace TwitchWrapper.Tests.TypeReaderTests
{
    public class NullableIntTypeReaderTests
    {
        private readonly TypeReader<int?> _typeReader = new NullableIntTypeReader();

        [Theory]
        [InlineData("1")]
        [InlineData("400000")]
        public void ConvertTo_WithInteger(string input)
        {
            var value = _typeReader.ConvertTo(input);

            Assert.IsType<int>(value);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        public void ConvertTo_WithEmptyInput(string input)
        {
            var value = _typeReader.ConvertTo(input);

            Assert.Null(value);
        }
    }
}