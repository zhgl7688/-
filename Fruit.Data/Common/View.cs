using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据源中的一个视图
    /// </summary>
    public class View : Table
    {

        internal View(ITableMetadataProvider provider, string viewName)
            : base(provider, viewName)
        {
            
        }
    }
}
