using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using AsbaBank.Core;

/* ===========================================================================================
 * NB! This infratstructure code is meant to simulate various infrastructure concerns.
 * Attempting to understanding this is not recommended or required in order to understand the 
 * general principles being demonstrated.
 * =========================================================================================== */
namespace AsbaBank.Infrastructure
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

    [DebuggerNonUserCode, DebuggerStepThrough]
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly JsonSerializer serializer = new JsonSerializer();
        private byte[] committedData;
        private readonly InMemoryDataStore dataStore = new InMemoryDataStore();
        
        public InMemoryUnitOfWork(InMemoryDataStore dataStore)
        {
            this.dataStore = dataStore;
            Commit();
        }

        public void Commit()
        {
            committedData = serializer.Serialize(dataStore);
        }

        public void Rollback()
        {
            var restored = serializer.Deserialize<InMemoryDataStore>(committedData);

            dataStore.Data = restored.Data;
            dataStore.EntityIdentities = restored.EntityIdentities;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where  TEntity : class 
        {
            return new InMemoryRepository<TEntity>(dataStore);
        }
    }

    [DataContract, DebuggerNonUserCode, DebuggerStepThrough]
    public class InMemoryDataStore
    {
        [DataMember]
        internal Dictionary<string, object> Data { get; set; }
        [DataMember]
        internal Dictionary<string, int> EntityIdentities { get; set; }

        public InMemoryDataStore()
        {
            Data = new Dictionary<string, object>();
            EntityIdentities = new Dictionary<string, int>();
        }

        internal int GetNextId<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();

            if (!EntityIdentities.ContainsKey(type))
            {
                EntityIdentities.Add(type, 1);
            }
            else
            {
                EntityIdentities[type]++;
            }

            return EntityIdentities[type];
        }

        internal IQueryable<TEntity> GetData<TEntity>() where TEntity : class
        {
            string type = typeof (TEntity).ToString();

            if (!Data.ContainsKey(type))
            {
                var collection = new HashSet<TEntity>();
                Data.Add(type, collection);
                return collection.AsQueryable();
            }

            return ((HashSet<TEntity>)Data[type]).AsQueryable();
        }

        internal bool Remove<TEntity>(TEntity oldItem) where TEntity : class
        {
            return GetEntityHashSet<TEntity>().Remove(oldItem);
        }

        internal void Add<TEntity>(TEntity item) where TEntity : class
        {
            GetEntityHashSet<TEntity>().Add(item);
        }

        internal int Count<TEntity>() where TEntity : class
        {
            return GetEntityHashSet<TEntity>().Count();
        }

        internal void Clear<TEntity>() where TEntity : class
        {
            GetEntityHashSet<TEntity>().Clear();
        }

        internal IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
        {
            return GetEntityHashSet<TEntity>().AsQueryable();
        }

        private HashSet<TEntity> GetEntityHashSet<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();

            if (!Data.ContainsKey(type))
            {
                var hashSet = new HashSet<TEntity>();
                Data.Add(type, hashSet);
                return hashSet;
            }

            return ((HashSet<TEntity>)Data[type]);
        }        
    }
}