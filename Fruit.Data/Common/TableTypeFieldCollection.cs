using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据库中的表字段集合
    /// </summary>
    public sealed class TableTypeFieldCollection : ObservableCollection<Field>
    {
        private ITableMetadataProvider provider;
        private TableType tableType;

        internal TableTypeFieldCollection(ITableMetadataProvider provider, TableType tableType)
        {
            this.provider = provider;
            this.tableType = tableType;

            var fields = provider.GetTableTypeFields(tableType.TypeName);
            foreach (var field in fields)
            {
                Add(field);
            }
        }
    }
}
