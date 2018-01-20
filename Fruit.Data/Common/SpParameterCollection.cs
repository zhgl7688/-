using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public class SpParameterCollection : ObservableCollection<SpParameter>
    {
        private ITableMetadataProvider provider;
        private StoredProcedure storedProcedure;

        internal SpParameterCollection(ITableMetadataProvider provider, StoredProcedure storedProcedure)
        {
            this.provider = provider;
            this.storedProcedure = storedProcedure;

            var parameters = provider.GetStoredProcedureParameters(storedProcedure.Name);
            foreach (var parameter in parameters)
            {
                Add(parameter);
            }
        }
    }
}
