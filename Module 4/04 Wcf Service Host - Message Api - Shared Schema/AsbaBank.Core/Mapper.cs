using System;
using System.Linq.Expressions;

namespace AsbaBank.ApplicationService
{
    public abstract class Mapper<TEntity,TDto>
    {
        public abstract Expression<Func<TEntity, TDto>> Expression { get; }
        
        public virtual TDto Map(TEntity client)
        {
            return Expression.Compile().Invoke(client);
        }
    }
}