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
    public sealed class TableTypeCollection : ObservableCollection<TableType>
    {
        private ITableMetadataProvider provider;

        internal TableTypeCollection(ITableMetadataProvider provider)
        {
            this.provider = provider;

            var typeNames = provider.GetTableTypes();
            foreach (var typeName in typeNames)
            {
                Add(new TableType(provider, typeName));
            }
        }

        public TableType this[string name]
        {
            get
            {
                return this.FirstOrDefault(t => t.TypeName == name);
            }
        }
    }
}
