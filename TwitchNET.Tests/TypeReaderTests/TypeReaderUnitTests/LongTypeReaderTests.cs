using TwitchNET.Modules;
using TwitchNET.Core.Exceptions.TypeReaderException;
using Xunit;

namespace TwitchNET.Tests.TypeReaderTests
{
    public class LongTypeReaderTests
    {
        private readonly BaseTypeReader _baseTypeReader = new LongBaseTypeReader();


        [Theory]
        [InlineData("1231231238")]
        [InlineData("10000000000")]
        [InlineData("1")]
        public void ConvertTo_WithLong(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<long>(value);
        }


        [Theory]
        [InlineData("21.42")]
        [InlineData("50000.423232321141")]
        public void ConvertTo_ThrowsOnDecimals(string input)
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