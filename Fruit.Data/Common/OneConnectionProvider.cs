using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fruit.Data
{
    public abstract class OneConnectionProvider
    {
        public abstract OneConnection CreateConnection(string connectionString);
    }
}
