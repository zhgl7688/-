using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_rolePermissionMap
    {
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string PermissionCode { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public virtual sys_permission sys_permission { get; set; }
        public virtual sys_role sys_role { get; set; }
    }
}
