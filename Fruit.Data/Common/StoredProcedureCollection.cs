using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据库中的存储过程集合
    /// </summary>
    public class StoredProcedureCollection : ObservableCollection<StoredProcedure>
    {
        private ITableMetadataProvider provider;

        internal StoredProcedureCollection(ITableMetadataProvider provider)
        {
            this.provider = provider;

            var storedProcedures = provider.GetStoredProcedures();
            foreach (var storedProcedure in storedProcedures)
            {
                Add(new StoredProcedure(provider, storedProcedure));
            }
        }

        public StoredProcedure this[string name]
        {
            get
            {
                return this.FirstOrDefault(t => t.Name == name);
            }
        }
    }
}
