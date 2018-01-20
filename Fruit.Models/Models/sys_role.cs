using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_role
    {
        public sys_role()
        {
            this.sys_organizeRoleMap = new List<sys_organizeRoleMap>();
            this.sys_rolePermissionMap = new List<sys_rolePermissionMap>();
            this.sys_roleMenuColumnMap = new List<sys_roleMenuColumnMap>();
            this.sys_roleMenuButtonMap = new List<sys_roleMenuButtonMap>();
            this.sys_roleMenuMap = new List<sys_roleMenuMap>();
            this.sys_userRoleMap = new List<sys_userRoleMap>();
        }

        public string RoleCode { get; set; }
        public string RoleSeq { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_organizeRoleMap> sys_organizeRoleMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_rolePermissionMap> sys_rolePermissionMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_roleMenuColumnMap> sys_roleMenuColumnMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_roleMenuButtonMap> sys_roleMenuButtonMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_roleMenuMap> sys_roleMenuMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_userRoleMap> sys_userRoleMap { get; set; }
    }
}
