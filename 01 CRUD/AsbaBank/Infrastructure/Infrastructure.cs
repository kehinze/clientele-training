using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace AsbaBank.Infrastructure
{
    /* NB! Do not bother reading this, it is infratstructure code meant to simulate our database. 
     * Understanding this code is not required in order to understand the general principles being demonstrated.
     */
    public interface IRepository
    {
        IEnumerable<TPersistable> All<TPersistable>() where TPersistable : class;
        TPersistable Get<TPersistable>(object id) where TPersistable : class;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Update<TPersistable>(object id, TPersistable item) where TPersistable : class;
        void Commit();
        void Rollback();
    }

    [DebuggerNonUserCode, DebuggerStepThrough]
    public sealed class InMemoryRepository : IRepository
    {
        public bool AutoIncrementIds { get; set; }
        private byte[] committedData;
        readonly JsonSerializer serializer = new JsonSerializer();
        private DataStore dataStore = new DataStore();

        public IEnumerable<TPersistable> All<TPersistable>() where TPersistable : class
        {
            return dataStore.GetData<TPersistable>();
        }

        public void Commit()
        {
            committedData = serializer.Serialize(dataStore);
        }

        public void Rollback()
        {
            dataStore = serializer.Deserialize<DataStore>(committedData);
        }

        public TPersistable Get<TPersistable>(object id) where TPersistable : class
        {
            PropertyInfo keyProperty = GetKeyProperty<TPersistable>();
            Expression<Func<TPersistable, bool>> lambda = BuildLambdaExpressionForKey<TPersistable>(id, keyProperty);
            IQueryable<TPersistable> collectionQuery = dataStore.GetData<TPersistable>().AsQueryable();

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
            HashSet<TPersistable> collection = dataStore.GetData<TPersistable>();

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
                    keyProperty.SetValue(item, dataStore.GetNextKey<TPersistable>());
                }
            }
            
            collection.Add(item);
            return item;
        }        

        public TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = dataStore.GetData<TPersistable>();

            collection.Remove(item);
            return item;
        }

        public TPersistable Update<TPersistable>(object id, TPersistable item) where TPersistable : class
        {
            var oldItem = Get<TPersistable>(id);
            var collection = dataStore.GetData<TPersistable>();
            collection.Remove(oldItem);
            collection.Add(item);
            return item;
        }
    }

    [Serializable]
    class DataStore
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
            string type = typeof(TPersistable).ToString();

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
    }

    static class SerializeExtensions
    {
        public static byte[] Serialize(this JsonSerializer serializer, object value)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(this JsonSerializer serializer, byte[] serialized)
        {
            serialized = serialized ?? new byte[] { };

            if (serialized.Length == 0)
                return default(T);

            using (var stream = new MemoryStream(serialized))
                return serializer.Deserialize<T>(stream);
        }
    }
}