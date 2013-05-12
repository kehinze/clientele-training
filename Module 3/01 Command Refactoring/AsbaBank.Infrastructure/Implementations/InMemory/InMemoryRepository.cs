using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AsbaBank.Infrastructure.Interfaces;

/* ===========================================================================================
 * NB! This infratstructure code is meant to simulate various infrastructure concerns.
 * Attempting to understanding this is not recommended or required in order to understand the 
 * general principles being demonstrated.
 * =========================================================================================== */
namespace AsbaBank.Infrastructure.Implementations.InMemory
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
                .Single(propertyInfo => Attribute.IsDefined((MemberInfo) propertyInfo, typeof(KeyAttribute)));
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return dataStore.GetData<TEntity>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TEntity Get(object id)
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

        public void Update(object id, TEntity item)
        {
            TEntity oldItem = Get(id);
            dataStore.Remove(oldItem);
            dataStore.Add(item);
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

        public void Clear()
        {
            dataStore.Clear<TEntity>();
        }

        public bool Contains(TEntity item)
        {
            return dataStore
                .AsQueryable<TEntity>()
                .Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(TEntity item)
        {
            return dataStore.Remove(item);
        }
    }
}