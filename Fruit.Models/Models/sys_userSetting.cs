using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_userSetting
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string SettingCode { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public virtual sys_user sys_user { get; set; }
    }
}
