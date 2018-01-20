using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_parameter
    {
        public string ParamCode { get; set; }
        public string ParamValue { get; set; }
        public Nullable<bool> IsUserEditable { get; set; }
        public string Description { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
