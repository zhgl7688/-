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
    public sealed class FieldCollection : ObservableCollection<Field>
    {
        private ITableMetadataProvider provider;
        private Table table;

        internal FieldCollection(ITableMetadataProvider provider, Table table)
        {
            this.provider = provider;
            this.table = table;

            var fields = provider.GetTableFields(table.TableName);
            foreach (var field in fields)
            {
                Add(field);
            }
        }
    }
}
