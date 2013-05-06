using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
/* ===========================================================================================
 * NB! This infratstructure code is meant to simulate various infrastructure concerns.
 * Attempting to understanding this is not recommended or required in order to understand the 
 * general principles being demonstrated.
 * =========================================================================================== */
namespace AsbaBank.Infrastructure
{
    using Newtonsoft.Json;

    public interface IRepository<TEntity> : ICollection<TEntity> where TEntity : class
    {
        TEntity Get(object id);
        void Update(object id, TEntity item);
    }

    [DebuggerNonUserCode, DebuggerStepThrough]
    sealed class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DataStore dataStore;
        private readonly ICollection<TEntity> data;
        private readonly PropertyInfo keyProperty;

        public int Count { get { return data.Count; } }
        public bool IsReadOnly { get { return false; } }

        public InMemoryRepository(DataStore dataStore)
        {
            this.dataStore = dataStore;
            keyProperty = GetKeyProperty<TEntity>();
            data = dataStore.GetData<TEntity>();
        }

        private static PropertyInfo GetKeyProperty<TPersistable>() where TPersistable : class
        {
            return typeof(TPersistable)
                .GetProperties()
                .Single(propertyInfo => Attribute.IsDefined(propertyInfo, typeof(KeyAttribute)));
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return data.GetEnumerator();
             
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TEntity Get(object id)
        {
            Expression<Func<TEntity, bool>> lambda = BuildLambdaExpressionForKey<TEntity>(id, keyProperty);
            return data.AsQueryable().SingleOrDefault(lambda);
        }

        public void Update(object id, TEntity item)
        {
            TEntity oldItem = Get(id);
            data.Remove(oldItem);
            data.Add(item);
        }

        public void Add(TEntity item)
        {
            object key = keyProperty.GetValue(item);

            if (key is Guid)
            {
                keyProperty.SetValue(item, Guid.NewGuid());
            }
            else
            {
                keyProperty.SetValue(item, dataStore.GetNextKey<TEntity>());
            }

            data.Add(item);
        }

        private static Expression<Func<TPersistable, bool>> BuildLambdaExpressionForKey<TPersistable>(object id, PropertyInfo keyProperty) where TPersistable : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TPersistable), "x");
            Expression property = Expression.Property(parameter, keyProperty.Name);
            Expression target = Expression.Constant(id);
            Expression equalsMethod = Expression.Equal(property, target);
            return Expression.Lambda<Func<TPersistable, bool>>(equalsMethod, parameter);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(TEntity item)
        {
            return data.Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        public bool Remove(TEntity item)
        {
            return data.Remove(item);
        }
    }
    
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }

    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private byte[] committedData;
        readonly JsonSerializer serializer = new JsonSerializer();
        private DataStore dataStore = new DataStore();

        public void Commit()
        {
            committedData = serializer.Serialize(dataStore);
        }

        public void Rollback()
        {
            dataStore = serializer.Deserialize<DataStore>(committedData);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where  TEntity : class 
        {
            return new InMemoryRepository<TEntity>(dataStore);
        }
    }

    [Serializable]
    internal class DataStore
    {
        public Dictionary<string, object> Data { get; set; }
        public Dictionary<string, int> Keys { get; set; }

        public DataStore()
        {
            Data = new Dictionary<string, object>();
            Keys = new Dictionary<string, int>();
        }

        public HashSet<TPersistable> GetData<TPersistable>() where TPersistable : class
        {
            string type = typeof (TPersistable).ToString();

            if (!Data.ContainsKey(type))
            {
                var collection = new HashSet<TPersistable>();
                Data.Add(type, collection);
                return collection;
            }

            return Data[type] as HashSet<TPersistable>;
        }

        public int GetNextKey<TPersistable>() where TPersistable : class
        {
            string type = typeof (TPersistable).ToString();

            if (!Keys.ContainsKey(type))
            {
                Keys.Add(type, 1);
            }
            else
            {
                Keys[type]++;
            }

            return Keys[type];
        }
    }

    class JsonSerializer 
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;
        private readonly Encoding encoding = Encoding.UTF8;

        public JsonSerializer()
        {
            serializer = new Newtonsoft.Json.JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.All,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public JsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public virtual void Serialize<T>(Stream output, T graph)
        {
            using (TextWriter writer = new StreamWriter(output, encoding))
            {
                Serialize(writer, graph);
            }
        }

        public virtual void Serialize<T>(TextWriter writer, T graph)
        {
            using (var jsonWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented })
            {
                serializer.Serialize(jsonWriter, graph);
            }
        }

        public virtual T Deserialize<T>(Stream input)
        {
            using (var streamReader = new StreamReader(input, encoding))
            {
                return (T)Deserialize(streamReader);
            }
        }

        protected virtual object Deserialize(StreamReader reader)
        {
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Deserialize(jsonReader);
            }
        }

        protected virtual object Deserialize(JsonReader jsonReader)
        {
            try
            {
                return serializer.Deserialize(jsonReader);
            }
            catch (JsonSerializationException e)
            {
                // Wrap in a standard .NET exception.
                throw new SerializationException(e.Message, e);
            }
        }

        public byte[] Serialize(object value)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] serialized)
        {
            serialized = serialized ?? new byte[] { };

            if (serialized.Length == 0)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(serialized))
            {
                return Deserialize<T>(stream);
            }
        }
    }
}