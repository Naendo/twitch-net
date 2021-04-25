using TwitchNET.Modules;
using Xunit;

namespace TwitchNET.Tests.TypeReaderTests
{
    public class NullableIntTypeReaderTests
    {
        private readonly BaseTypeReader _baseTypeReader = new NullableIntBaseTypeReader();

        [Theory]
        [InlineData("1")]
        [InlineData("400000")]
        public void ConvertTo_WithInteger(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<int>(value);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        public void ConvertTo_WithEmptyInput(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.Null(value);
        }
    }
}