using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TD
{
    public class Converter : JsonConverter
    {
        private static Assembly _assembly;
        private static Dictionary<string, Type> _mapper;
        
        static Converter()
        {
            _assembly = typeof(Converter).Assembly;
            _mapper = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in _assembly.GetExportedTypes())
            {
                _mapper.Add(type.Name, type);
            }
        }
        
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type type)
        {
            return _mapper.ContainsKey(type.Name);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);

            if (jToken.Type == JTokenType.Object)
            {
                var jObject = (JObject) jToken;
                
                var typeProp = jObject["@type"];
                if (typeProp != null)
                {
                    var typeName = (string)typeProp;
                    if (typeName != null && _mapper.TryGetValue(typeName, out var type))
                    {
                        return jObject.ToObject(type);
                    }
                }
            }

            return jToken.ToObject(objectType);
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public class Int64 : JsonConverter
        {
            public override bool CanRead => true;

            public override bool CanWrite => true;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var token = JToken.ReadFrom(reader);
                if (token.Type == JTokenType.Array)
                {
                    var arr = (JArray) token;
                    var res = new long[arr.Count];

                    for (int i = 0; i < arr.Count; i++)
                    {
                        res[i] = arr[i].Value<long>();
                    }

                    return res;
                }

                if (token.Type == JTokenType.Integer || token.Type == JTokenType.String)
                {
                    return token.Value<long>();
                }

                return null;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(long) 
                    || objectType == typeof(long[]);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is long val)
                {
                    serializer.Serialize(writer, val.ToString());
                }
                else if (value is long[] arr)
                {
                    serializer.Serialize(writer, arr.Select(v => v.ToString()).ToArray());
                }
            }
        }
    }
}