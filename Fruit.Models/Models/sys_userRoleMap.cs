using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_userRoleMap
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string RoleCode { get; set; }
        public virtual sys_role sys_role { get; set; }
        public virtual sys_user sys_user { get; set; }
    }
}
