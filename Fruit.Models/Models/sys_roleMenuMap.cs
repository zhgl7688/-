using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_roleMenuMap
    {
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string MenuCode { get; set; }
        public virtual sys_menu sys_menu { get; set; }
        public virtual sys_role sys_role { get; set; }
    }
}
