using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AsbaBank.Core.Queries;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Shell.QueryServices;

namespace AsbaBank.Presentation.Shell.RemoteProxies
{
    public sealed class QueryHandlerProxy<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public TResult Handle(TQuery query)
        {
            using (var service = new QueryServiceClient())
            {
                var json = query.SerializeJson();
                var result = service.Handle(json);
                return result.DeserializeJson<TResult>(GetKnownTypes());
            }
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            var contractAssembly = Assembly.Load(new AssemblyName("AsbaBank.Presentation.Shell"));

            var queryTypes = (
               from type in contractAssembly.GetExportedTypes()
               where TypeIsQueryType(type)
               select type)
               .ToList();

            var resultTypes =
                from queryType in queryTypes
                select GetQueryResultType(queryType);

            return queryTypes.Union(resultTypes).ToArray();
        }

        private static bool TypeIsQueryType(Type type)
        {
            return GetQueryInterface(type) != null;
        }

        private static Type GetQueryResultType(Type queryType)
        {
            return GetQueryInterface(queryType).GetGenericArguments()[0];
        }

        private static Type GetQueryInterface(Type type)
        {
            return (
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType
                where typeof(IQuery<>).IsAssignableFrom(@interface.GetGenericTypeDefinition())
                select @interface)
                .SingleOrDefault();
        }
    }    
}