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
    
    public partial class fw_Objection
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objectionid { get; set; }
        public string orderid { get; set; }
        public decimal? orderprice { get; set; }
        public string Objectionstatus { get; set; }
        public decimal? otherprice { get; set; }
        public string otherstatus { get; set; }
        public string explain { get; set; }
        public string MemId { get; set; }
        public string MemIdSeller { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? objState { get; set; }
        public string isShow { get; set; }
        public string IsAgreed { get; set; }
        public string IsOnJudge { get; set; }
        public int? IsContinue { get; set; }
        public string img0 { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public int? Cimg1 { get; set; }
        public int? Cimg2 { get; set; }
        public int? Cimg3 { get; set; }
        public string sysExplain { get; set; }
    }
}