using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_log
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}
