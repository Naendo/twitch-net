using System;
using TwitchNET.Core.Exceptions.TypeReaderException;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core.Middleware
{
    internal sealed class TypeReaderBuilder : IMiddleware
    {
        RequestContext IMiddleware.Execute(RequestContext context)
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


                    if (paramterInfo.ParameterType.IsPrimitive
                        || typeCode == TypeCode.Decimal
                        || typeCode == TypeCode.String)
                        typeReaders[i] = MessageTypeReader.Default;
                    else if (context.CustomTypeReaders is not null &&
                             context.CustomTypeReaders.ContainsKey(paramterInfo.ParameterType))
                    {
                        typeReaders[i] = context.CustomTypeReaders[paramterInfo.ParameterType];
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"There is no TypeReader registerd for type: {paramterInfo.ParameterType}");
                    }
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