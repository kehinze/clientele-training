using System;
using System.Data.Entity;
using AsbaBank.Core.Persistence;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IContextFactory contextFactory;
        private DbContext context;
        private bool isDisposed;

        public UnitOfWork(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            context = contextFactory.GetContext();
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Rollback()
        {
            context.Dispose();
            context = contextFactory.GetContext();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(context.Set<TEntity>());
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            context.Dispose();
        }
    }
}