using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Sql
{
    /// <summary>
    /// 表示 Sql 语句的操作类型
    /// </summary>
    public enum SqlOperationKind
    {
        Select,
        Insert,
        Update,
        Delete
    }
}
