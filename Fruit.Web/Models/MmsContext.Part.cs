using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web.Models
{
    public partial class MmsContext
    {
        public MmsContext(bool readOnly = false)
            : base(readOnly ? "name=MmsMysqlContext" : "name=MmsContext")
        {
        }
    }
}