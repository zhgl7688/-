using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 提供针对与 DbDataReader 的扩展方法
    /// </summary>
    public static class DbDataReaderExtension
    {
        /// <summary>
        /// 读取所有数据并返回 Json 数组样式字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="skipNullValue">是否跳过为空值生成属性对。</param>
        /// <returns></returns>
        public static string ToJsonArrayString(this DbDataReader reader, bool skipNullValue = true)
        {
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);
            writer.WriteStartArray();

            while (reader.Read())
            {
                ToJsonObjectString(writer, reader, skipNullValue);
            }

            writer.WriteEndArray();
            return stringWriter.ToString();
        }

        private static void ToJsonObjectString(JsonTextWriter writer, DbDataReader reader, bool skipNullValue)
        {
            writer.WriteStartObject();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var value = reader[i];
                var isNull = (value == DBNull.Value || value == null);
                if (isNull && skipNullValue)
                {
                    continue;
                }
                writer.WritePropertyName(reader.GetName(i));
                if (isNull)
                {
                    writer.WriteNull();
                }
                else
                {
                    WriteValue(writer, value);
                }
            }
            writer.WriteEndObject();
        }

        private static void WriteValue(JsonTextWriter writer, object value)
        {
            var valueType = value.GetType();
            var writeMethod = typeof(JsonTextWriter).GetMethod("WriteValue", System.Reflection.BindingFlags.Instance, null, new Type[] { valueType }, null);
            if (writeMethod == null)
            {
                writer.WriteValue(value.ToString());
            }
            else
            {
                writeMethod.Invoke(writeMethod, new object[] { value });
            }
        }
    }
}
