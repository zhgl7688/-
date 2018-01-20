using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Sql
{
    class SqlConnectionProvider : OneConnectionProvider
    {
        public override OneConnection CreateConnection(string connectionString)
        {
            var conn = new SqlOneConnection();
            conn.Open(connectionString);
            return conn;
        }
    }
}
