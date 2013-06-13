using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AsbaBank.Core;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>  where TEntity : class
    {
        private readonly IDbSet<TEntity> dbSet;

        public Expression Expression { get { return dbSet.Expression; } }
        public Type ElementType { get { return dbSet.ElementType; } }
        public IQueryProvider Provider { get { return dbSet.Provider; } }

        public EntityFrameworkRepository(IDbSet<TEntity> dbSet)
        {
            this.dbSet = dbSet;
        }
       
        public virtual TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Add(TEntity newEntity)
        {
            dbSet.Add(newEntity);
        }

        public virtual void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual IEnumerator<TEntity> GetEnumerator()
        {
            return dbSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }      
    }
}