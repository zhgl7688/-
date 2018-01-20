using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public interface ITableMetadataProvider
    {
        string[] GetTableNames();
        string[] GetViewNames();
        string[] GetTableTypes();

        string[] GetStoredProcedures();

        Field[] GetTableFields(string table);

        Field[] GetTableTypeFields(string tableType);

        SpParameter[] GetStoredProcedureParameters(string storedProcedure);
        ObjectType GetType(string name);
    }
}
