using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_permission
    {
        public sys_permission()
        {
            this.sys_rolePermissionMap = new List<sys_rolePermissionMap>();
        }

        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public string ParentCode { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_rolePermissionMap> sys_rolePermissionMap { get; set; }
    }
}
