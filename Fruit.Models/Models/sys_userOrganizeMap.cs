using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_userOrganizeMap
    {
        public int ID { get; set; }
        public string OrganizeCode { get; set; }
        public string UserCode { get; set; }
        public virtual sys_organize sys_organize { get; set; }
        public virtual sys_user sys_user { get; set; }
    }
}
