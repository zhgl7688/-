﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已由 FruitBuilder 生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fruit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class fw_longterminfo
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int longid { get; set; }
        public string longname { get; set; }
        public string mode { get; set; }
        public string remark { get; set; }
        public string memid { get; set; }
        public string url { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? modifydate { get; set; }
        public int? isactive { get; set; }
        public string longcode { get; set; }
    }
}