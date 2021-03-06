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
    
    public partial class HR_PractiseCerts
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public string CustID { get; set; }
        public string Remark { get; set; }
        public string Scan1 { get; set; }
        public string Scan2 { get; set; }
        public string Scan3 { get; set; }
        public string Scan4 { get; set; }
        public DateTime UpdateDate { get; set; }
        public string bUsed { get; set; }
        public DateTime? certDate { get; set; }
        public string certNo { get; set; }
        public string certName { get; set; }
        public string certOrgan { get; set; }
        public string certType { get; set; }
        public string empID { get; set; }
        public DateTime expireDate { get; set; }
        public DateTime expireDate2 { get; set; }
        public string onProject { get; set; }
        public string regNo { get; set; }
        public string trainStatus { get; set; }
        public string trainStatus2 { get; set; }
        public string onProjectName { get; set; }
        public string idCard { get; set; }
        public string qualificationsCertNo { get; set; }
        public string isBorrow { get; set; }
        public string isSecurity { get; set; }
        public string CodeID { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public decimal? GAmt { get; set; }
        public string Corp { get; set; }
    }
}
