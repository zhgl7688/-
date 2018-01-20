using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Sql
{
    /// <summary>
    /// 用于生成或创建 SQL 查询
    /// </summary>
    public class SqlBuilder
    {
        private SqlOperationKind operationKind;
        private SqlTable table;

        public SqlBuilder(SqlOperationKind operationKind, SqlTable table)
        {
            this.operationKind = operationKind;
            this.table = table;
            this.PageSize = 10;
        }

        public SqlBuilder(SqlOperationKind operationKind, string table)
            : this(operationKind, new SqlTable { Name = table })
        {
        }

        /// <summary>
        /// 分页状态下每一页的大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码（页码从 1 开始，0 表示不分页）
        /// </summary>
        public int PageNumber { get; set; }
    }
}
