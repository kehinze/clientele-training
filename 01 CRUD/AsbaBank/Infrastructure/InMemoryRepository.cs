using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AsbaBank.Infrastructure
{
    /* NB! Do not bother reading this, it is infratstructure code meant to simulate our database. 
     * Understanding this code is not required in order to understand the general principles being demonstrated.
     */
    [DebuggerNonUserCode, DebuggerStepThrough]
    public sealed class InMemoryRepository : IRepository
    {
        public bool AutoIncrementIds { get; set; }
        readonly Dictionary<Type, object> repository = new Dictionary<Type, object>();
        readonly Dictionary<Type, int> keys = new Dictionary<Type, int>();

        public IEnumerable<TPersistable> All<TPersistable>() where TPersistable : class
        {
            return GetCollection<TPersistable>();
        }

        public TPersistable Get<TPersistable>(object id) where TPersistable : class
        {
            PropertyInfo keyProperty = GetKeyProperty<TPersistable>();
            Expression<Func<TPersistable, bool>> lambda = BuildLambdaExpressionForKey<TPersistable>(id, keyProperty);
            IQueryable<TPersistable> collectionQuery = GetCollection<TPersistable>().AsQueryable();

            return collectionQuery.SingleOrDefault(lambda);
        }

        private static Expression<Func<TPersistable, bool>> BuildLambdaExpressionForKey<TPersistable>(object id, PropertyInfo keyProperty) where TPersistable : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TPersistable), "x");
            Expression property = Expression.Property(parameter, keyProperty.Name);
            Expression target = Expression.Constant(id);
            Expression equalsMethod = Expression.Equal(property, target);
            return Expression.Lambda<Func<TPersistable, bool>>(equalsMethod, parameter);
        }

        private static PropertyInfo GetKeyProperty<TPersistable>() where TPersistable : class
        {
            return typeof(TPersistable)
                .GetProperties()
                .Single(propertyInfo => Attribute.IsDefined(propertyInfo, typeof(KeyAttribute)));
        }

        public TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();

            if (AutoIncrementIds)
            {
                PropertyInfo keyProperty = GetKeyProperty<TPersistable>();
                object key = keyProperty.GetValue(item);

                if (key is Guid)
                {
                    keyProperty.SetValue(item, Guid.NewGuid());
                }
                else
                {
                    keyProperty.SetValue(item, GetNextKey<TPersistable>());
                }
            }
            
            collection.Add(item);
            return item;
        }

        private int GetNextKey<TPersistable>() where TPersistable : class
        {
            if (!keys.ContainsKey(typeof(TPersistable)))
            {
                keys.Add(typeof(TPersistable), 1);
            }
            else
            {
                keys[typeof(TPersistable)]++;
            }

            return keys[typeof(TPersistable)];
        }

        public TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();

            collection.Remove(item);
            return item;
        }

        public TPersistable Update<TPersistable>(object id, TPersistable item) where TPersistable : class
        {
            var oldItem = Get<TPersistable>(id);
            var collection = GetCollection<TPersistable>();
            collection.Remove(oldItem);
            collection.Add(item);
            return item;
        }

        private HashSet<TPersistable> GetCollection<TPersistable>() where TPersistable : class
        {
            if (!repository.ContainsKey(typeof(TPersistable)))
            {
                var collection = new HashSet<TPersistable>();
                repository.Add(typeof(TPersistable), collection);
                return collection;
            }

            return repository[typeof(TPersistable)] as HashSet<TPersistable>;
        }       
    }
}