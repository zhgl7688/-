using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_menu
    {
        public sys_menu()
        {
            this.sys_menuButtonMap = new List<sys_menuButtonMap>();
            this.sys_menuButtonMap1 = new List<sys_menuButtonMap>();
            this.sys_roleMenuColumnMap = new List<sys_roleMenuColumnMap>();
            this.sys_roleMenuButtonMap = new List<sys_roleMenuButtonMap>();
            this.sys_roleMenuMap = new List<sys_roleMenuMap>();
        }

        public string MenuCode { get; set; }
        public string ParentCode { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public string IconClass { get; set; }
        public string IconURL { get; set; }
        public string MenuSeq { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsVisible { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_menuButtonMap> sys_menuButtonMap { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_menuButtonMap> sys_menuButtonMap1 { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_roleMenuColumnMap> sys_roleMenuColumnMap { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_roleMenuButtonMap> sys_roleMenuButtonMap { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_roleMenuMap> sys_roleMenuMap { get; set; }
    }
}
