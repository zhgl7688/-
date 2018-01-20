using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Fruit.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            config.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(config));

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { 
                    id = RouteParameter.Optional,
                    namespaceName = new string[] { "Fruit.Web.Controllers" }
                }
            );

            config.Routes.MapHttpRoute(
                name: "PublicApi",
                routeTemplate: "wapi/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    namespaceName = new string[] { "Fruit.Web.Generated" }
                }
            );
            JsonConvert.DefaultSettings = () =>
            {
                var jss = new JsonSerializerSettings();
                jss.Converters.Add(new BooleanJsonConverter());
                jss.Converters.Add(new DateTimeJsonConverter());
                return jss;
            };
            
        }

        [JsonConverter(typeof(bool))]
        public class BooleanJsonConverter : JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return typeof(bool) == objectType || typeof(bool?) == objectType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (typeof(bool?) == objectType)
                {
                    if (reader.TokenType == JsonToken.Null) return null;
                }
                if (typeof(bool?) == objectType || typeof(bool) == objectType)
                {
                    if (reader.TokenType == JsonToken.Null) return false;
                    if (reader.TokenType == JsonToken.Boolean) return reader.Value;
                    if (reader.TokenType == JsonToken.String) return reader.Value.Equals("True") || reader.Value.Equals("true") || reader.Value.Equals("1");
                    if (reader.TokenType == JsonToken.Integer) return reader.Value.Equals(1);
                }
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    writer.WriteValue((bool)value);
                }
            }
        }

        [JsonConverter(typeof(DateTime))]
        public class DateTimeJsonConverter : JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return typeof(DateTime) == objectType || typeof(DateTime?) == objectType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (typeof(bool?) == objectType)
                {
                    if (reader.TokenType == JsonToken.Null) return null;
                }
                if (typeof(DateTime?) == objectType || typeof(DateTime) == objectType)
                {
                    if (reader.TokenType == JsonToken.Null) return null;
                    if (reader.TokenType == JsonToken.String)
                    {
                        var strVal = (string)reader.Value;
                        if (!string.IsNullOrEmpty(strVal))
                        {
                            DateTime outTime;
                            if (DateTime.TryParse(strVal, out outTime))
                            {
                                return outTime;
                            }
                            if (DateTime.TryParseExact(strVal, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out outTime))
                            {
                                return outTime;
                            }
                            if (DateTime.TryParseExact(strVal, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out outTime))
                            {
                                return outTime;
                            }
                            if (DateTime.TryParseExact(strVal, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out outTime))
                            {
                                return outTime;
                            }
                        }
                    }
                    return null;
                }
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
        }
    }
}
