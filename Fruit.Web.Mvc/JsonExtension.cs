using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    public static class JsonExtension
    {
        static JsonExtension()
        {
            FixJsonSerializer = JsonSerializer.Create();
            FixJsonSerializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            FixJsonSerializer.Converters.Add(new FixJsonConverter());
        }

        public static List<KeyValuePair<Guid, string>> FixTreeParentIdMap(this JArray array, string idField, string parentField)
        {
            var coreMap = new Dictionary<string, Guid>();
            var map = new List<KeyValuePair<Guid, string>>();
            for (var i = 0; i < array.Count; i++)
            {
                var item = array[i];
                var id = (string)item[idField];
                var parent = (string)item[parentField];
                Guid key, value1;
                string value = null;
                if (!Guid.TryParse(id, out key))
                {
                    if (id != null && coreMap.ContainsKey(id))
                    {
                        key = coreMap[id];
                    }
                    else
                    {
                        coreMap[id] = key = Guid.NewGuid();
                    }
                }
                if (!Guid.TryParse(parent, out value1))
                {
                    if (!string.IsNullOrEmpty(parent))
                    {
                        if (coreMap.ContainsKey(parent))
                        {
                            value = coreMap[parent].ToString();
                        }
                        else
                        {
                            coreMap[parent] = key = Guid.NewGuid();
                        }
                    }
                }
                else
                {
                    value = value1.ToString();
                }
                map.Add(new KeyValuePair<Guid, string>(key, value));
            }
            return map;
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 可处理空对象到值类型的工具
        /// </summary>
        public static JsonSerializer FixJsonSerializer { get; private set; }

        class FixJsonConverter : JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Int32) || objectType == typeof(Guid) || objectType == typeof(Guid?) || objectType == typeof(bool) || objectType == typeof(bool?);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(Int32))
                {
                    if (object.Equals(reader.Value, ""))
                    {
                        return existingValue;
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {
                        return int.Parse((string)reader.Value);
                    }
                    else if (reader.ValueType == typeof(Int64))
                    {
                        return (int)(long)reader.Value;
                    }
                    else
                    {
                        return (int)reader.Value;
                    }
                }
                else if(objectType == typeof(Guid))
                {
                    if (object.Equals(reader.Value, ""))
                    {
                        return existingValue;
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {
                        return Guid.Parse((string)reader.Value);
                    }
                }
                else if (objectType == typeof(Guid?))
                {
                    if (object.Equals(reader.Value, "") || object.Equals(reader.Value, "null"))
                    {
                        return (Guid?)null;
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {
                        return Guid.Parse((string)reader.Value);
                    }
                }
                else if (objectType == typeof(bool))
                {
                    if (object.Equals(reader.Value, ""))
                    {
                        return false;
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {
                        return !((string)reader.Value == "False" || (string)reader.Value == "false" || (string)reader.Value == "0" || (string)reader.Value == string.Empty);
                    }
                }
                else if (objectType == typeof(bool?))
                {
                    if (object.Equals(reader.Value, ""))
                    {
                        return null;
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {
                        return !((string)reader.Value == "False" || (string)reader.Value == "false" || (string)reader.Value == "0");
                    }
                }
                throw new ArgumentException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
