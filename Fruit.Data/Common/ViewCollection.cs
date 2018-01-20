using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据库中的视图集合
    /// </summary>
    public sealed class ViewCollection : ObservableCollection<Table>
    {
        private ITableMetadataProvider provider;

        internal ViewCollection(ITableMetadataProvider provider)
        {
            this.provider = provider;

            var tableNames = provider.GetViewNames();
            foreach (var tableName in tableNames)
            {
                Add(new View(provider, tableName));
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
