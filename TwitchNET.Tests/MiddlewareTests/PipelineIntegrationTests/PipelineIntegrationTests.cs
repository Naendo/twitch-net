using TwitchNET.Core;
using TwitchNET.Core.Responses;
using TwitchNET.Modules;
using TwitchWrapper.Tests.MiddlewareTests.Setup;
using Xunit;

namespace TwitchWrapper.Tests.MiddlewareTests
{
    public class PipelineIntegrationTests
    {
        [Fact]
        public void ExecutePipeline_PrimitivParameters()
        {
            //Arange
            var requestBuilder = new RequestBuilder();
            requestBuilder.UseProxies();
            requestBuilder.UseTypeReader();


            var commandInfo = new CommandInfo{
                CommandKey = "test",
                MethodInfo = typeof(DummyModule).GetMethod(nameof(DummyModule.DummyMethode))
            };

            //Act
            var context = requestBuilder.ExecutePipeline(commandInfo, new DummyModule(), new TwitchBot(),
                new MessageResponseModel{
                    Message = "!test asdf 12",
                    Channel = "testchannel",
                    ResponseType = ResponseType.PrivMsg
                });


            //Assert
            Assert.NotNull(context.Endpoint.ChannelProxy);
            Assert.NotNull(context.Endpoint.CommandProxy);
            Assert.NotNull(context.Endpoint.TwitchBot);
            Assert.NotNull(context.Endpoint.UserProxy);


            Assert.NotNull(context.CommandInfo.Parameters);
            Assert.NotNull(context.CommandInfo.TypeReaders);
            Assert.IsType<string>(context.Parameters.Values[0]);
            Assert.IsType<int>(context.Parameters.Values[1]);
        }

        [Fact]
        public void ExecutePipeline_CustomParameters()
        {
            //Arange
            var requestBuilder = new RequestBuilder();
            requestBuilder.UseProxies();
            requestBuilder.UseTypeReader();
            requestBuilder.UseTypeReader<TestEnum, CustomTypeReader>();


            var commandInfo = new CommandInfo{
                CommandKey = "test",
                MethodInfo = typeof(DummyModule).GetMethod(nameof(DummyModule.CustomTypeReaderDummyMethode))
            };

            //Act
            var context = requestBuilder.ExecutePipeline(commandInfo, new DummyModule(), new TwitchBot(),
                new MessageResponseModel{
                    Message = $"!test {TestEnum.Test2}",
                    Channel = "testchannel",
                    ResponseType = ResponseType.PrivMsg
                });


            //Assert
            Assert.NotNull(context.Endpoint.ChannelProxy);
            Assert.NotNull(context.Endpoint.CommandProxy);
            Assert.NotNull(context.Endpoint.TwitchBot);
            Assert.NotNull(context.Endpoint.UserProxy);


            Assert.NotNull(context.CommandInfo.Parameters);
            Assert.NotNull(context.CommandInfo.TypeReaders);
            Assert.IsType<TestEnum>(context.Parameters.Values[0]);
        }
    }
}