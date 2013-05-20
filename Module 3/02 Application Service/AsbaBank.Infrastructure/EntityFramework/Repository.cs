using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext context;

        public Repository(DbContext dbContext)
        {
            this.context = dbContext;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return context.Set<TEntity>().AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TEntity item)
        {
            context.Set<TEntity>().Add(item);
        }

        public void Clear()
        {
   
        }

        public bool Contains(TEntity item)
        {
            return context.Set<TEntity>().Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
             throw new NotImplementedException("This method is not supported by Entity framework");
        }

        public bool Remove(TEntity item)
        {
            context.Set<TEntity>().Remove(item);

            return true;
        }

        public int Count { get { return context.Set<TEntity>().Count(); } }

        public bool IsReadOnly { get { return false; } }

        public TEntity Get(object id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public void Update(object id, TEntity item)
        {

        }
    }
}
