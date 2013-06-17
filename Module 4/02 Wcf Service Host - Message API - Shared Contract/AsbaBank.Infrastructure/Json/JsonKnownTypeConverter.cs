using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AsbaBank.Infrastructure.Json
{
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