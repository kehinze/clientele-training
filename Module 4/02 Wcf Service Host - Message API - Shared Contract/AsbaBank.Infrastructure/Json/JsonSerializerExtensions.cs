using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AsbaBank.Infrastructure.Json
{
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
}