using TwitchNET.Tests.MiddlewareTests.Setup;
using Xunit;

namespace TwitchNET.Tests.MiddlewareTests.TypeReaderTests
{
    public class CustomTypeReaderTests
    {
        [Fact]
        public void CustomTypeReader_EnumConverter()
        {
            var customTypeReader = new CustomTypeReader();

            var testString = TestEnum.Test1.ToString();

            Assert.Equal(TestEnum.Test1, customTypeReader.ConvertFrom(typeof(TestEnum), testString));
        }
    }
}