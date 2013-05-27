using AsbaBank.Core.Queries;
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
                var result = service.Handle(query);
                return (TResult)result;
            }
        }
    }    
}