using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_organize
    {
        public sys_organize()
        {
            this.sys_userOrganizeMap = new List<sys_userOrganizeMap>();
            this.sys_organizeRoleMap = new List<sys_organizeRoleMap>();
        }

        public string OrganizeCode { get; set; }
        public string ParentCode { get; set; }
        public string OrganizeSeq { get; set; }
        public string OrganizeName { get; set; }
        public string Description { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public string CompCode { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_userOrganizeMap> sys_userOrganizeMap { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_organizeRoleMap> sys_organizeRoleMap { get; set; }
    }
}
