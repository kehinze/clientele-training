using System;

using AsbaBank.Core.Queries;

namespace AsbaBank.Presentation.Shell
{
    //public sealed class RemoteQueryProcessor : IQueryProcessor
    //{
    //    public TResult Handle<TResult>(IQuery<TResult> query)
    //    {
    //        Type handlerGenericType = typeof(QueryHandlerProxy<,>);
    //        Type queryType = query.GetType();
    //        Type handlerType = handlerGenericType.MakeGenericType(queryType, typeof(TResult));

    //        var handler = Activator.CreateInstance(handlerType);

    //        var result = ((dynamic)handler).Handle((dynamic)query);

    //        return result;
    //    }
    //}
}