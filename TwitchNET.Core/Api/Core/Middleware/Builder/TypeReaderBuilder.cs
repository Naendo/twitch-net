using System;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core.Middleware
{
    public sealed class TypeReaderBuilder : IMiddleware
    {
        public RequestContext Execute(RequestContext context)
        {
            var parameters = context.IrcResponseModel.ParseResponse().Parameter;

            context.Parameters = new ParameterCollection(new object[parameters.Length]);

            //(1) Manage Cache
            if (context.CommandInfo.TypeReaders is null)
            {
                //ToDo: MethodInfo null-check
                context.CommandInfo.Parameters = context.CommandInfo.MethodInfo.GetParameters();

                var typeReaders = new ITypeReader[parameters.Length];

                for (var i = 0; i < context.CommandInfo.Parameters.Count; i++)
                {
                    var paramterInfo = context.CommandInfo.Parameters[i];
                    var typeCode = Type.GetTypeCode(paramterInfo.ParameterType);


                    //ToDo: Implement Custom-TypeReader
                    if (paramterInfo.ParameterType.IsPrimitive || typeCode == TypeCode.Decimal ||
                        typeCode == TypeCode.String)
                        typeReaders[i] = MessageTypeReader.Default;
                    else
                        throw new NotImplementedException("CustomTypeReaders");
                }

                context.CommandInfo.TypeReaders = typeReaders;
            }

            //(2) On full Cache
            for (var i = 0; i < context.CommandInfo.Parameters.Count; i++)
                context.Parameters.Values[i] = context.CommandInfo.TypeReaders[i]
                    .ConvertFrom(context.CommandInfo.Parameters[i].ParameterType, parameters[i]);


            return context;
        }
    }
}