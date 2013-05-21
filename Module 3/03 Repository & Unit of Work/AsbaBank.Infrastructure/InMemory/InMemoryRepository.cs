using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AsbaBank.Core;

/* ===========================================================================================
 * NB! This infratstructure code is meant to simulate various infrastructure concerns.
 * Attempting to understanding this is not recommended or required in order to understand the 
 * general principles being demonstrated.
 * =========================================================================================== */
namespace AsbaBank.Infrastructure.InMemory
{
    [DebuggerNonUserCode, DebuggerStepThrough]
    sealed class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly InMemoryDataStore dataStore;
        private readonly PropertyInfo identityPropertyInfo;

        public int Count { get { return dataStore.Count<TEntity>(); } }
        public bool IsReadOnly { get { return false; } }

        public InMemoryRepository(InMemoryDataStore dataStore)
        {
            this.dataStore = dataStore;
            identityPropertyInfo = GetIdentityPropertyInformation();
        }

        private PropertyInfo GetIdentityPropertyInformation() 
        {
            return typeof(TEntity)
                .GetProperties()
                .Single(propertyInfo => Attribute.IsDefined(propertyInfo, typeof(KeyAttribute)));
        }

        public TEntity Get(int id)
        {
            return dataStore
                .AsQueryable<TEntity>()
                .AsQueryable()
                .SingleOrDefault(WithMatchingId(id));
        }

        private Func<TEntity, bool> WithMatchingId(object id)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression property = Expression.Property(parameter, identityPropertyInfo.Name);
            Expression target = Expression.Constant(id);
            Expression equalsMethod = Expression.Equal(property, target);
            Func<TEntity, bool> predicate = Expression.Lambda<Func<TEntity, bool>>(equalsMethod, parameter).Compile();
            
            return predicate;
        }

        public IQueryable<TEntity> FindAll()
        {
            return dataStore.AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dataStore
                .AsQueryable<TEntity>()
                .Where(predicate);
        }       

        public void Add(TEntity item)
        {
            object entityId = identityPropertyInfo.GetValue(item);

            if (entityId is Guid)
            {
                identityPropertyInfo.SetValue(item, Guid.NewGuid());
            }
            else
            {
                identityPropertyInfo.SetValue(item, dataStore.GetNextId<TEntity>());
            }

            dataStore.Add(item);
        }      
  
        public void Remove(TEntity item)
        {
            dataStore.Remove(item);
        }
    }
}