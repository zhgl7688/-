using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 提供 JObject, JArray 等对象扩展功能
    /// </summary>
    public static class JsonExtension
    {
        public static void CopyToTable(this JArray array, DataTable dataTable, Dictionary<string, Func<JToken, object>> setter = null, Action<DataRow> rowSetter = null)
        {
            foreach (var item in array)
            {
                CopyToTable(item, dataTable, setter, rowSetter);
            }
        }
        public static void CopyToTable(this JToken item, DataTable dataTable, Dictionary<string, Func<JToken, object>> setter = null, Action<DataRow> rowSetter = null)
        {
            var datarow = dataTable.NewRow();
            foreach (var jToken in item)
            {
                var jproperty = jToken as JProperty;
                if (jproperty == null) continue;
                if (setter != null && setter.ContainsKey(jproperty.Name))
                {
                    datarow[jproperty.Name] = setter[jproperty.Name](jproperty.Value);
                }
                else if(dataTable.Columns[jproperty.Name] != null)
                {
                    datarow[jproperty.Name] = jproperty.Value.ToString();
                }
            }
            if (rowSetter != null)
            {
                rowSetter(datarow);
            }
            dataTable.Rows.Add(datarow);
        }

        public static object DateTime(this JToken token)
        {
            string val = token.ToString();
            if (string.IsNullOrEmpty(val) || val == "null")
            {
                return DBNull.Value;
            }
            return System.DateTime.Parse(val);
        }

        public static object Int(this JToken token)
        {
            string val = token.ToString();
            if (string.IsNullOrEmpty(val) || val == "null")
            {
                return DBNull.Value;
            }
            return Int32.Parse(val);
        }
    }
}
