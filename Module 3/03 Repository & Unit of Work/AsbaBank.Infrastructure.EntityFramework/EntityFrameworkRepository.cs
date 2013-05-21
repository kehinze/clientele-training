using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AsbaBank.Core;

namespace AsbaBank.Infrastructure.EntityFramework
{
    class EntityFrameworkRepository<TEntity> : IRepository<TEntity>  where TEntity : class
    {
        private readonly IDbSet<TEntity> dbSet;

        internal EntityFrameworkRepository(IDbSet<TEntity> dbSet)
        {
            this.dbSet = dbSet;
        }
       
        public IQueryable<TEntity> FindAll()
        {
            return dbSet;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public void Add(TEntity newEntity)
        {
            dbSet.Add(newEntity);
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}