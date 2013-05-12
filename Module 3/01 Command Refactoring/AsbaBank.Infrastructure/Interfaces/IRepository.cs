using System.Collections.Generic;

namespace AsbaBank.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> : ICollection<TEntity> where TEntity : class
    {
        TEntity Get(object id);
        void Update(object id, TEntity item);
    }
}