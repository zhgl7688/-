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
    
    public partial class tbBdMall
    {
        [Key, Column(Order = 0)]
        public Guid? Code { get; set; }
        public string Name { get; set; }
        public string PinyinCode { get; set; }
        public string PlatformName { get; set; }
        public string appkey { get; set; }
        public string appscret { get; set; }
        public string sessionkey { get; set; }
        public string CodeField { get; set; }
        [Key, Column(Order = 1)]
        public float? Discount { get; set; }
    }
}
