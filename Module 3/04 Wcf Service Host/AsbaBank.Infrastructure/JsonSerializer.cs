using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AsbaBank.Infrastructure
{
    [DebuggerNonUserCode, DebuggerStepThrough]
    public class JsonSerializer 
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;
        private readonly Encoding encoding = Encoding.UTF8;

        public JsonSerializer()
        {
            serializer = new Newtonsoft.Json.JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
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



    public static class JsonSerializerExtensions
    {
        public static T DeserializeJson<T>(this string jsonString, IEnumerable<Type> knownTypes = null)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonString,
                                                    new JsonSerializerSettings
                                                    {
                                                        TypeNameHandling = TypeNameHandling.Objects,
                                                        DefaultValueHandling = DefaultValueHandling.Populate,
                                                        NullValueHandling = NullValueHandling.Ignore,
                                                        TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                                                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                                                        Converters = new List<JsonConverter>(
                                                            new JsonConverter[]
                                                            {
                                                               new JsonKnownTypeConverter(knownTypes)
                                                            })
                                                    });
        }

        public static string SerializeJson(this object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Objects,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                });
        }
    }

    public class JsonKnownTypeConverter : JsonConverter
    {
        const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        public IEnumerable<Type> KnownTypes { get; set; }
        public JsonKnownTypeConverter(IEnumerable<Type> knownTypes)
        {
            KnownTypes = knownTypes;
        }

        protected object Create(Type objectType, JObject jObject)
        {
            if (jObject["$type"] != null)
            {
                string typeName = jObject["$type"].ToString();
            
                return CreateInstanceUsingNonPublicConstructor(KnownTypes.First(x => typeName.Contains("." + x.Name + ",")));
            }

            throw new InvalidOperationException("No supported type");
        }

        public static object CreateInstanceUsingNonPublicConstructor(Type type, params object[] parameters)
        {
            Type[] types = parameters.ToList().ConvertAll(input => input.GetType()).ToArray();
            var constructor = type.GetConstructor(Flags, null, types, null);
            
            return constructor.Invoke(parameters);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override bool CanConvert(Type objectType)
        {
            if (KnownTypes == null)
                return false;

            return (objectType.IsInterface || objectType.IsAbstract) && KnownTypes.Any(objectType.IsAssignableFrom);
        }
    }
}