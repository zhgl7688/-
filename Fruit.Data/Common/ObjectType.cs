using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示连接中特定对象的类型
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// 没有此对象
        /// </summary>
        None,
        /// <summary>
        /// 表
        /// </summary>
        Table,
        /// <summary>
        /// 视图
        /// </summary>
        View,
        /// <summary>
        /// 存储过程
        /// </summary>
        StoredProcedures,
        /// <summary>
        /// 其它类型
        /// </summary>
        Other,
    }
}
