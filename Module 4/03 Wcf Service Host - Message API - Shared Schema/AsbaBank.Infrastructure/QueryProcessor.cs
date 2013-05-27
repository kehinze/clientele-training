using System;
using System.Collections.Generic;
using System.Linq;

using AsbaBank.Core.Queries;

namespace AsbaBank.Infrastructure
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly HashSet<object> handlers;

        public QueryProcessor()
        {
            handlers = new HashSet<object>();
        }

        public void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public TResult Handle<TResult>(IQuery<TResult> query) 
        {
            Type handlerGenericType = typeof(IQueryHandler<,>);
            Type queryType = query.GetType();
            Type handlerType = handlerGenericType.MakeGenericType(queryType, typeof(TResult));
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            return ((dynamic)handler).Handle((dynamic)query);
        }
    }    
}