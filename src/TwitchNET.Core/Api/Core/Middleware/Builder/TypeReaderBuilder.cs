using System;
using System.Reflection;
using TwitchNET.Core.Interfaces;
using TwitchNET.Core.Modules;


namespace TwitchNET.Core.Middleware
{
    /// <summary>
    /// Workflow: Middleware is setting up infos about the modules parameters. Next, the context cache will check if any typeReaders are
    /// registered in cache for that module, if not it will manage the cache. Next the middleware is Seeding the Context with TypeReaders that
    /// just got bumped into the cache or were already in cache
    /// </summary>
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
                context.CommandInfo.TypeReaders = SeedTypeReadersByParameterInfo(context, parameters);
            }

            //(2) On full Cache
            context = SeedContextFromCache(context, parameters);


            return context;
        }

        private RequestContext SeedContextFromCache(RequestContext context, string[] parameters)
        {
            for (var i = 0; i < context.CommandInfo.Parameters.Count; i++)

                context.Parameters.Values[i] = context.CommandInfo.TypeReaders[i]
                    .ConvertFrom(context.CommandInfo.Parameters[i].ParameterType,
                        parameters.Length == 0
                            ? context.CommandInfo.Parameters[i].DefaultValue?.ToString()
                            : parameters[i]);

            return context;
        }


        private ITypeReader[] SeedTypeReadersByParameterInfo(RequestContext context,
            string[] parameters)
        {
            var typeReaders = new ITypeReader[context.CommandInfo.Parameters.Count];

            for (var i = 0; i < context.CommandInfo.Parameters.Count; i++)
            {
                var parameterInfo = context.CommandInfo.Parameters[i];

                var typeCode = Type.GetTypeCode(parameterInfo.ParameterType);


                if (parameterInfo.IsOptional && parameters.Length < context.Parameters.Count)
                {
                    typeReaders[i] = MessageTypeReader.Default;
                }
                else if (parameterInfo.ParameterType.IsPrimitive
                         || typeCode == TypeCode.Decimal
                         || typeCode == TypeCode.String)
                    typeReaders[i] = MessageTypeReader.Default;
                else if (context.CustomTypeReaders is not null &&
                         context.CustomTypeReaders.ContainsKey(parameterInfo.ParameterType))
                {
                    typeReaders[i] = context.CustomTypeReaders[parameterInfo.ParameterType];
                }
                else
                {
                    throw new ArgumentException(
                        $"There is no TypeReader registerd for type: {parameterInfo.ParameterType}");
                }
            }

            return typeReaders;
        }
    }
}