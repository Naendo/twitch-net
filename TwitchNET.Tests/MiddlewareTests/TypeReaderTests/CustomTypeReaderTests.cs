using TwitchWrapper.Tests.MiddlewareTests.Setup;
using Xunit;

namespace TwitchWrapper.Tests.MiddlewareTests.TypeReaderTests
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