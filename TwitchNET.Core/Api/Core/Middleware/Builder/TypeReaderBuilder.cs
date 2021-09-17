using System;
using TwitchNET.Modules.TypeReader;

namespace TwitchNET.Core.Middleware
{
    internal sealed class TypeReaderBuilder : IMiddleware
    {
        RequestContext IMiddleware.Execute(RequestContext context)
        {
            var parameters = context.IrcResponseModel.ParseResponse().Parameter;
            //ToDo: MethodInfo null-check
            context.CommandInfo.Parameters = context.CommandInfo.MethodInfo.GetParameters();

            context.Parameters = new ParameterCollection(new object[context.CommandInfo.Parameters.Count]);

            //(1) Manage Cache
            if (context.CommandInfo.TypeReaders is null)
            {
                var typeReaders = new ITypeReader[context.CommandInfo.Parameters.Count];

                for (var i = 0; i < context.CommandInfo.Parameters.Count; i++)
                {
                    var paramterInfo = context.CommandInfo.Parameters[i];
                    var typeCode = Type.GetTypeCode(paramterInfo.ParameterType);

                    if (paramterInfo.IsOptional && parameters.Length < context.Parameters.Count)
                    {
                        typeReaders[i] = MessageTypeReader.Default;
                    }
                    else if (paramterInfo.ParameterType.IsPrimitive
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
                    .ConvertFrom(context.CommandInfo.Parameters[i].ParameterType,
                        parameters.Length == 0
                            ? context.CommandInfo.Parameters[i].DefaultValue?.ToString()
                            : parameters[i]);


            return context;
        }
    }
}