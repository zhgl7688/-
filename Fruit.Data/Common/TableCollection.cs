using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据库中的表集合
    /// </summary>
    public sealed class TableCollection : ObservableCollection<Table>
    {
        private ITableMetadataProvider provider;

        internal TableCollection(ITableMetadataProvider provider)
        {
            this.provider = provider;

            var tableNames = provider.GetTableNames();
            foreach (var tableName in tableNames)
            {
                Add(new Table(provider, tableName));
            }
        }

        public Table this[string name]
        {
            get
            {
                return this.FirstOrDefault(t => t.TableName == name);
            }
        }
    }
}
