using TwitchWrapper.Core.Exceptions.TypeReaderException;
using TwitchWrapper.Core.TypeReader;
using Xunit;

namespace TwitchWrapper.Tests.TypeReaderTests
{
    public class LongTypeReaderTests
    {
        private readonly TypeReader<bool> _typeReader = new BoolTypeReader();


        [Theory]
        [InlineData("1231231238")]
        [InlineData("10000000000")]
        [InlineData("1")]
        public void ConvertTo_WithLong(string input)
        {
            var value = _typeReader.ConvertTo(input);

            Assert.IsType<long>(value);
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

        [Theory]
        [InlineData("false")]
        [InlineData("true")]
        public void ConvertTo_ThrowsOnBool(string input)
        {
            Assert.Throws<TypeReaderException>(() => _typeReader.ConvertTo(input));
        }
    }
}