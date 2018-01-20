using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_loginHistory
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string HostIP { get; set; }
        public string LoginCity { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
    }
}
