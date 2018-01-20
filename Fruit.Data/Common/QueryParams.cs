using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public class QueryParams : List<QueryParam>
    {
        public QueryParams()
        { }

        public QueryParams(IEnumerable<QueryParam> collection)
            : base(collection)
        { }
    }

    public class QueryParam
    {
        public QueryParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public QueryParam(string name, object value, DbType type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public string Name { get; set; }
        public DbType Type { get; set; }
        public object Value { get; set; }
        public object DbValue { get; set; }
        public bool IsOutput { get; set; }
    }
}
