using TwitchNET.Modules;
using TwitchNET.Core.Exceptions.TypeReaderException;
using Xunit;

namespace TwitchNET.Tests.TypeReaderTests
{
    public class IntTypeReaderTests
    {
        private readonly BaseTypeReader _baseTypeReader = new IntBaseTypeReader();

        [Theory]
        [InlineData("1")]
        [InlineData("400000")]
        public void ConvertTo_WithInteger(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<int>((int) value);
        }

        /// <param name="input">2147483648 - Integer_Max border</param>
        [Theory]
        [InlineData("2147483648")]
        [InlineData("5999994277272")]
        public void CovertTo_WithIntergerOutOfBound(string input)
        {
            Assert.Throws<TypeReaderException>(() => _baseTypeReader.ConvertTo(input));
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

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void ConvertTo_ThrowsOnBool(string input)
        {
            Assert.Throws<TypeReaderException>(() => _baseTypeReader.ConvertTo(input));
        }
    }
}