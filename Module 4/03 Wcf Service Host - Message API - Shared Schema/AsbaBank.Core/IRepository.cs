﻿using System.Linq;

namespace AsbaBank.Core
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        void Add(TEntity newEntity);
        void Remove(TEntity entity);
    }
}