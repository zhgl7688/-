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
    
    public partial class fw_helpinfo
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string helpid { get; set; }
        public string helptitle { get; set; }
        public string helpcontent_h { get; set; }
        public string helpcontent { get; set; }
        public int? isenabled { get; set; }
        public int? handleuserid { get; set; }
        public string helpcode { get; set; }
        public int? sort { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatePerson { get; set; }
    }
}
