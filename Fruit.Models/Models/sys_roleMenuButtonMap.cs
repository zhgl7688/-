using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_roleMenuButtonMap
    {
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string MenuCode { get; set; }
        public string ButtonCode { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual sys_button sys_button { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual sys_menu sys_menu { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual sys_role sys_role { get; set; }
    }
}
