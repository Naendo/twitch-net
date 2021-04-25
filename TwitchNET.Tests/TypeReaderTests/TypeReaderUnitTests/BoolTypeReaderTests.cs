using TwitchNET.Modules;
using TwitchNET.Core.Exceptions.TypeReaderException;
using Xunit;

namespace TwitchNET.Tests.TypeReaderTests
{
    public class BoolTypeReaderTests
    {
        private readonly BaseTypeReader _baseTypeReader = new BoolBaseTypeReader();


        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public void ConvertTo_WithBool(string input)
        {
            var value = _baseTypeReader.ConvertTo(input);

            Assert.IsType<bool>(value);
        }


        [Theory]
        [InlineData("1")]
        [InlineData("0")]
        public void ConvertTo_ThrowsOnInteger(string input)
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
    }
}