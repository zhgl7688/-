using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_user
    {
        public sys_user()
        {
            this.sys_userSetting = new List<sys_userSetting>();
            this.sys_userOrganizeMap = new List<sys_userOrganizeMap>();
            this.sys_userRoleMap = new List<sys_userRoleMap>();
        }

        public string UserCode { get; set; }
        public string UserSeq { get; set; }
        public string UserName { get; set; }
        public string CompCode { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string Mobile { get; set; }
        public string OrganizeName { get; set; }
        public string ConfigJSON { get; set; }
        public bool IsEnable { get; set; }
        public Nullable<int> LoginCount { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_userSetting> sys_userSetting { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_userOrganizeMap> sys_userOrganizeMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_userRoleMap> sys_userRoleMap { get; set; }
    }
}
