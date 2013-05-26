using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

using AsbaBank.Core.Queries;
using AsbaBank.Infrastructure;

namespace AsbaBank.ApplicationService.Wcf
{
    [ServiceContract(Namespace = "Asba.Queries")]
    public class QueryService
    {
        [OperationContract]
        public string Handle(string queryString)
        {
            var query = queryString.DeserializeJson<GenericQuery>(GetKnownTypes());
            IQueryProcessor handler = Environment.GetQueryHandlers();
            
            var result = handler.Handle((dynamic)query);

            return JsonSerializerExtensions.SerializeJson(result);
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            var contractAssembly = Assembly.Load(new AssemblyName("AsbaBank.Contracts"));

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
