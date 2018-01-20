using Fruit.Data.Generate;
using Fruit.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public static class OneConnectionFactories
    {
        private static Dictionary<string, OneConnectionProvider> providers = new Dictionary<string, OneConnectionProvider>();

        static OneConnectionFactories()
        {
            providers.Add("System.Data.SqlClient", new SqlConnectionProvider());
        }

        public static OneConnection GetConnection(string providerName, string connectionString)
        {
            if ("System.Data.EntityClient" == providerName)
            {
                ConvertEntityProvider(ref providerName, ref connectionString);
            }
            if (providers.ContainsKey(providerName))
            {
                return providers[providerName].CreateConnection(connectionString);
            }
            throw new NotImplementedException();
        }

        private static void ConvertEntityProvider(ref string providerName, ref string connectionString)
        {
            ConnectionStringBuilder sb = new ConnectionStringBuilder(connectionString);
            providerName = sb["provider"];
            connectionString = sb["provider connection string"];
        }
    }
}
