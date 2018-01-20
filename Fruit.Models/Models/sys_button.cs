using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_button
    {
        public sys_button()
        {
            this.sys_menuButtonMap = new List<sys_menuButtonMap>();
            this.sys_roleMenuButtonMap = new List<sys_roleMenuButtonMap>();
        }

        public string ButtonCode { get; set; }
        public string ButtonName { get; set; }
        public Nullable<int> ButtonSeq { get; set; }
        public string Description { get; set; }
        public string ButtonIcon { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_menuButtonMap> sys_menuButtonMap { get; set; }

        [JsonIgnore]
        public virtual ICollection<sys_roleMenuButtonMap> sys_roleMenuButtonMap { get; set; }
    }
}
