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
    
    public partial class fw_categoryinfo
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int catid { get; set; }
        public string catname { get; set; }
        public int? parentid { get; set; }
        public string catcode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatePerson { get; set; }
    }
}
