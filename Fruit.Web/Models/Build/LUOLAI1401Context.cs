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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LUOLAI1401Context : DbContext
    {
        public LUOLAI1401Context()
            : base("name=LUOLAI1401Context")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    
        public virtual DbSet<wq_dealerInfo> wq_dealerInfo { get; set; }
        public virtual DbSet<base_productInfo> base_productInfo { get; set; }
        public virtual DbSet<wq_baseRptProduct> wq_baseRptProduct { get; set; }
        public virtual DbSet<base_ProductBrandInfo> base_ProductBrandInfo { get; set; }
        public virtual DbSet<base_productCate> base_productCate { get; set; }
        public virtual DbSet<base_region> base_region { get; set; }
        
        public virtual DbSet<wq_order_bd> wq_order_bd { get; set; }
        public virtual DbSet<wq_termPop> wq_termPop { get; set; }
        public virtual DbSet<a_hd> a_hd { get; set; }
        public virtual DbSet<a_bd> a_bd { get; set; }
        public virtual DbSet<SYS_BSDATA> SYS_BSDATA { get; set; }
        public virtual DbSet<SYS_BSDATATYPE> SYS_BSDATATYPE { get; set; }
        public virtual DbSet<tbCRMCustomerType> tbCRMCustomerType { get; set; }
       
        public virtual DbSet<log> log { get; set; }
        public virtual DbSet<PersonInfo> PersonInfo { get; set; }
        public virtual DbSet<base_customField> base_customField { get; set; }
        public virtual DbSet<wq_attendTrack> wq_attendTrack { get; set; }
        public virtual DbSet<wq_unittype> wq_unittype { get; set; }
        public virtual DbSet<three1> three1 { get; set; }
        public virtual DbSet<three2> three2 { get; set; }
        public virtual DbSet<three3> three3 { get; set; }
       
        public virtual DbSet<wq_Pop_Dealer> wq_Pop_Dealer { get; set; }
        
        
        
        
        
        
        
       
               
        
        
      
        
        
        
        
        
        public virtual DbSet<FA_Invoices> FA_Invoices { get; set; }
        public virtual DbSet<FA_InvAccepts> FA_InvAccepts { get; set; }
        public virtual DbSet<FA_Receipts> FA_Receipts { get; set; }
        
        
        
        
        public virtual DbSet<PrepaidTax> PrepaidTax { get; set; }
        
        
        
        
        public virtual DbSet<fw_productinfo> fw_productinfo { get; set; }
        public virtual DbSet<fw_orderinfo> fw_orderinfo { get; set; }
        public virtual DbSet<fw_orderlist> fw_orderlist { get; set; }
        public virtual DbSet<fw_auctionbuyinginfo> fw_auctionbuyinginfo { get; set; }
        public virtual DbSet<fw_teambuyinginfo> fw_teambuyinginfo { get; set; }
        public virtual DbSet<fw_longterminfo> fw_longterminfo { get; set; }
        public virtual DbSet<fw_messageinfo> fw_messageinfo { get; set; }
        public virtual DbSet<fw_recordinfo> fw_recordinfo { get; set; }
        public virtual DbSet<fw_newsinfo> fw_newsinfo { get; set; }
        public virtual DbSet<fw_slideinfo> fw_slideinfo { get; set; }
        public virtual DbSet<fw_helpinfo> fw_helpinfo { get; set; }
        public virtual DbSet<fw_categoryinfo> fw_categoryinfo { get; set; }
        public virtual DbSet<fw_calendarinfo> fw_calendarinfo { get; set; }
        public virtual DbSet<fw_contractinfo> fw_contractinfo { get; set; }
        public virtual DbSet<fw_memberinfo> fw_memberinfo { get; set; }
        public virtual DbSet<fw_userinfo> fw_userinfo { get; set; }
        public virtual DbSet<fw_disputeinfo> fw_disputeinfo { get; set; }

        public virtual DbSet<fw_depositinfo> fw_depositinfo { get; set; }
        public virtual DbSet<fw_directprice> fw_directprice { get; set; }
        public virtual DbSet<fw_ObjectionResult> fw_ObjectionResult { get; set; }
        public virtual DbSet<fw_Objection> fw_Objection { get; set; }
        public virtual DbSet<fw_directinfo> fw_directinfo { get; set; }
        public virtual DbSet<fw_teambuying> fw_teambuying { get; set; }
        public virtual DbSet<fw_Direct> fw_Direct { get; set; }
        public virtual DbSet<v_ObjectionResult> v_ObjectionResult { get; set; }
        public virtual DbSet<v_ObjectionResult1> v_ObjectionResult1 { get; set; }
        public virtual DbSet<v_Orderinfo> v_Orderinfo { get; set; }
        public virtual DbSet<v_MemberAll> v_MemberAll { get; set; }
        public virtual DbSet<v_objection> v_objection { get; set; }
        // -- INSERT POINT --
    }
}