using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AsbaBank.Core.Persistence;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class EntityFrameworkQuery : IEntityQuery, ISqlQuery 
    {
        private readonly DbContext context;

        public EntityFrameworkQuery(IContextFactory contextFactory)
        {
            context = contextFactory.GetContext();
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            return context.Database.SqlQuery<TEntity>(sql, parameters);
        }

        public IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            return context.Database.SqlQuery(elementType, sql, parameters);
        }
    }
}
