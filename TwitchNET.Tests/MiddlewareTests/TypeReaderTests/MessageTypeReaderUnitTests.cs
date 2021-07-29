using System;
using TwitchNET.Modules.TypeReader;
using Xunit;

namespace TwitchNET.Tests.MiddlewareTests.TypeReaderTests
{
    public class MessageTypeReaderUnitTests
    {
        [Theory]
        [InlineData("this is a string")]
        [InlineData("1231231 this is also a string")]
        public void ConvertFrom_WithStringGeneric(string input)
        {
            var result = MessageTypeReader.Default.ConvertFrom<string>(input);
            Assert.IsType<string>(result);
        }


        [Theory]
        [InlineData("this is a string")]
        [InlineData("1231231 this is also a string")]
        public void ConvertFrom_WithString(string input)
        {
            var result = MessageTypeReader.Default.ConvertFrom(typeof(string), input);
            Assert.IsType<string>(result);
        }

        [Theory]
        [InlineData(null)]
        public void ConvertFrom_WithNullableString(string? input)
        {
            var result = MessageTypeReader.Default.ConvertFrom(typeof(string), input);
            Assert.Null(result);
        }

        [Fact]
        public void ConvertFrom_WithInt()
        {
            var min = int.MinValue.ToString();
            var max = (int.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(int), min);
            Assert.IsType<int>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(int), max);
            Assert.IsType<int>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithUInt()
        {
            var min = uint.MinValue.ToString();
            var max = (uint.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(uint), min);
            Assert.IsType<uint>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(uint), max);
            Assert.IsType<uint>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithNullableInt()
        {
            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(int?), null);
            Assert.Null(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(int?), null);
            Assert.Null(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithLong()
        {
            var min = long.MinValue.ToString();
            var max = (long.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(long), min);
            Assert.IsType<long>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(long), max);
            Assert.IsType<long>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithULong()
        {
            var min = ulong.MinValue.ToString();
            var max = (ulong.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(ulong), min);
            Assert.IsType<ulong>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(ulong), max);
            Assert.IsType<ulong>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithNullableLong()
        {
            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(int?), null);
            Assert.Null(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(int?), null);
            Assert.Null(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithDouble()
        {
            var min = double.MinValue.ToString();
            var max = (double.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(double), min);
            Assert.IsType<double>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(double), max);
            Assert.IsType<double>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithNullableDouble()
        {
            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(double?), null);
            Assert.Null(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(double?), null);
            Assert.Null(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithByte()
        {
            var min = byte.MinValue.ToString();
            var max = (byte.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(byte), min);
            Assert.IsType<byte>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(byte), max);
            Assert.IsType<byte>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithSByte()
        {
            var min = sbyte.MinValue.ToString();
            var max = (sbyte.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(sbyte), min);
            Assert.IsType<sbyte>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(sbyte), max);
            Assert.IsType<sbyte>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithNullableByte()
        {
            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(byte?), null);
            Assert.Null(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(byte?), null);
            Assert.Null(resultMax);
        }


        [Fact]
        public void ConvertFrom_WithDecimal()
        {
            var min = decimal.MinValue.ToString();
            var max = (decimal.MaxValue - 1).ToString();

            var resultMin = MessageTypeReader.Default.ConvertFrom(typeof(decimal), min);
            Assert.IsType<decimal>(resultMin);

            var resultMax = MessageTypeReader.Default.ConvertFrom(typeof(decimal), max);
            Assert.IsType<decimal>(resultMax);
        }

        [Fact]
        public void ConvertFrom_WithNullableDecimal()
        {
            var resultMin = (decimal?) MessageTypeReader.Default.ConvertFrom(typeof(decimal?), null);

            Assert.Null(resultMin);

            var resultMax = (decimal?) MessageTypeReader.Default.ConvertFrom(typeof(decimal?), null);
            Assert.Null(resultMax);
        }
    }
}