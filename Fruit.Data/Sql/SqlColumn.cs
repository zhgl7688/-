using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Sql
{
    /// <summary>
    /// 表示 Sql 中的列
    /// </summary>
    public class SqlColumn
    {
        public SqlTable Table { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
