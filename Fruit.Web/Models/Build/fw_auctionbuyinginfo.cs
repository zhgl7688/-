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
    
    public partial class fw_auctionbuyinginfo
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int aucid { get; set; }
        public int? proid { get; set; }
        public decimal? primePrice { get; set; }
        public decimal? floorPrice { get; set; }
        public int? addPrice { get; set; }
        public decimal? cellingPrice { get; set; }
        public int? counts { get; set; }
        public string memid { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public int? minUserCount { get; set; }
        public string imgurl { get; set; }
        public string remark { get; set; }
        public int? isactive { get; set; }
        public int? ispassed { get; set; }
        public string auccode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatePerson { get; set; }
        public int? CatId { get; set; }
        public string ProName { get; set; }
        public string Spec { get; set; }
        public string Grade { get; set; }
        public decimal? DepositBuyer { get; set; }
        public string PayType { get; set; }
        public int? IsSplit { get; set; }
        public decimal cautionAmt { get; set; }
        public string cautionFlay { get; set; }
        public int ispayback { get; set; }
        public int dealFlag { get; set; }
        public int dealTimes { get; set; }
        public int isSuccess { get; set; }
    }
}
