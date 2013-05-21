using System;
using System.Linq;
using System.Linq.Expressions;

namespace AsbaBank.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        void Add(TEntity newEntity);
        void Remove(TEntity entity);
    }
}