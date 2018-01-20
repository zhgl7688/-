using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_organizeRoleMap
    {
        public int ID { get; set; }
        public string OrganizeCode { get; set; }
        public string RoleCode { get; set; }
        public virtual sys_organize sys_organize { get; set; }
        public virtual sys_role sys_role { get; set; }
    }
}
