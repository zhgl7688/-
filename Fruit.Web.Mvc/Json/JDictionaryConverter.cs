using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web.Json
{
    /// <summary>
    /// JSON 一般对象与 .Net 字典转换器
    /// </summary>
    public class JDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary<string, object>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IDictionary<string, object> dict = (IDictionary<string, object>)Activator.CreateInstance(objectType);
            string key = null;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        key = (string)reader.Value;
                        break;
                    case JsonToken.String:
                        dict[key] = (string)reader.Value;
                        break;
                    case JsonToken.Float:
                        dict[key] = (float)reader.Value;
                        break;
                    case JsonToken.Integer:
                        if (reader.ValueType == typeof(Int64))
                        {
                            dict[key] = (Int64)reader.Value;
                        }
                        else
                        {
                            dict[key] = (int)reader.Value;
                        }
                        break;
                    case JsonToken.Boolean:
                        dict[key] = (bool)reader.Value;
                        break;
                    case JsonToken.Null:
                        dict[key] = null;
                        break;
                    case JsonToken.EndObject:
                        goto end;
                    default:
                        throw new Exception("JDictionaryConverter 不能解析 " + reader.TokenType);
                }
            }
            end:
            return dict;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
