using System;
using System.Collections.Generic;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Middleware;
using TwitchNET.Core.Responses;
using TwitchNET.Core.Modules;
using TwitchNET.Tests.MiddlewareTests.Setup;
using Xunit;

namespace TwitchNET.Tests.MiddlewareTests.TypeReaderTests
{
    public class TypeReaderBuilderTests
    {
        [Fact]
        public void Execute_PrimitivType_NoCache()
        {
            var builder = new TypeReaderBuilder() as IMiddleware;

            var result = builder.Execute(new RequestContext(){
                CommandInfo = new CommandInfo(){
                    CommandKey = "key",
                    MethodInfo = typeof(DummyModule).GetMethod(nameof(DummyModule.DummyMethode))
                },
                Endpoint = new DummyModule(),
                IrcResponseModel = new MessageResponseModel{
                    Message = "!test asdf 12",
                    Channel = "testchannel",
                    ResponseType = ResponseType.PrivMsg
                }
            });

            Assert.NotNull(result.Parameters);
            Assert.NotNull(result.CommandInfo.Parameters);
            Assert.NotNull(result.CommandInfo.TypeReaders);
            Assert.IsType(result.CommandInfo.MethodInfo.GetParameters()[0].ParameterType,
                result.Parameters[0]);
            Assert.IsType(result.CommandInfo.MethodInfo.GetParameters()[1].ParameterType,
                result.Parameters[1]);
        }

        [Fact]
        public void Execute_CustomType_NoCache()
        {
            var builder = new TypeReaderBuilder() as IMiddleware;

            var context = new RequestContext(){
                CommandInfo = new CommandInfo(){
                    CommandKey = "key",
                    MethodInfo = typeof(DummyModule).GetMethod(nameof(DummyModule.CustomTypeReaderDummyMethode))
                },
                Endpoint = new DummyModule(),
                IrcResponseModel = new MessageResponseModel{
                    Message = $"!test {TestEnum.Test1}",
                    Channel = "testchannel",
                    ResponseType = ResponseType.PrivMsg
                },
                CustomTypeReaders = new Dictionary<Type, ITypeReader>{
                    {typeof(TestEnum), new CustomTypeReader()}
                }
            };

            var result = builder.Execute(context);

            Assert.NotNull(result.Parameters);
            Assert.NotNull(result.CommandInfo.Parameters);
            Assert.NotNull(result.CommandInfo.TypeReaders);
            Assert.IsType(result.CommandInfo.MethodInfo.GetParameters()[0].ParameterType,
                result.Parameters[0]);
        }

        [Fact]
        public void Execute_CustomType_NoTypeReaderRegisterd()
        {
            var builder = new TypeReaderBuilder() as IMiddleware;

            Assert.Throws<ArgumentException>(() =>
            {
                builder.Execute(new RequestContext(){
                    CommandInfo = new CommandInfo(){
                        CommandKey = "key",
                        MethodInfo = typeof(DummyModule).GetMethod(nameof(DummyModule.CustomTypeReaderDummyMethode))
                    },
                    Endpoint = new DummyModule(),
                    IrcResponseModel = new MessageResponseModel{
                        Message = $"!test {TestEnum.Test1}",
                        Channel = "testchannel",
                        ResponseType = ResponseType.PrivMsg
                    }
                });
            });
        }
    }
}