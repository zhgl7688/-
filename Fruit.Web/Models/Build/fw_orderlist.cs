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
    
    public partial class fw_orderlist
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ordlistid { get; set; }
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ordid { get; set; }
        public int? proid { get; set; }
        public string proname { get; set; }
        public string spec { get; set; }
        public decimal? price { get; set; }
        public decimal? counts { get; set; }
        public decimal? subtotal { get; set; }
    }
}
