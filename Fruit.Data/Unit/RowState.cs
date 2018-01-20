using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public enum RowState : int
    {
        Unchange = 0,
        New = 1,
        Changed = 2,
        Deleted = 3
    }
}
