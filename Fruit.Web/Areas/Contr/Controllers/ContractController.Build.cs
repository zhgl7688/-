/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Contr.Controllers
{
    public partial class ContractController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> CustID, Corp, Type, Region, Scan1;
                using(var db = new LUOLAI1401Context())
                {
                    CustID = db.BD_Customers.Select(i => new ComboItem { Text = i.Contact, Value = "" + i.FID }).ToList();
                }
                using(var db = new SysContext())
                {
                    Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                    Type = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtType'")).ToList();
                    Region = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Region'")).ToList();
                    Scan1 = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yes/no'")).ToList();
                }
                return View(new {dataSource = new {CustID,Corp,Type,Region,Scan1}});
            }
            else
            {
                return View("Edit", id);
            }
        }

        public ActionResult Edit(string id)
        {
            PM_Contracts form = null;
            List<ComboItem> Corp = null, Type = null, CustID = null, Region = null, Scan1 = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                Type = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtType'")).ToList();
                Region = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Region'")).ToList();
                Scan1 = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='GCLB'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                CustID = db.BD_Customers.Select(i => new ComboItem { Text = i.Contact, Value = "" + i.FID }).ToList();
                form = db.PM_Contracts.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_Contracts
                {
                    Code = id,
                    Type = string.Format("{0}", "1"),
                    Region = string.Format("{0}", "1"),
                    Scan1 = string.Format("{0}", "n")
                };
            }
            return View(new { form = form, dataSource = new { Corp,Type,CustID,Region,Scan1 }});
        }

    }
    public partial class ContractApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ContractsListModel {
            public string Code { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public string Type { get; set; }
            public string Type_RefText { get; set; }
            public decimal Amt { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string Owner { get; set; }
            public string Situation { get; set; }
            public decimal taxAmt { get; set; }
            public string Region { get; set; }
            public string Region_RefText { get; set; }
            public string FDate { get; set; }
            public string Remark { get; set; }
            public string Scan1 { get; set; }
            public string Scan2 { get; set; }
            public string ArchStatus { get; set; }
            public string Scan1_RefText { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public string affiliatedPM { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "Corp", "a.Corp", "");
            SerachCondition.Dropdown(sbCondition, "Type", "a.Type", "");
            SerachCondition.Dropdown(sbCondition, "CustID", "a.CustID", "");
            SerachCondition.TextBox(sbCondition, "Owner", "a.Owner", "");
            SerachCondition.Dropdown(sbCondition, "Region", "a.Region", "");
            SerachCondition.Date(sbCondition, "FDate", "a.FDate", "");
            SerachCondition.Dropdown(sbCondition, "Scan1", "a.Scan1", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_ContractsListModel>(db.Database, "a.Code ,b.Text Corp_RefText ,a.Corp ,c.Text Type_RefText ,a.Type ,a.Amt ,d.Contact CustID_RefText ,a.CustID ,a.Owner ,a.Situation ,a.taxAmt ,e.Text Region_RefText ,a.Region ,a.FDate ,a.Remark ,f.Text Scan1_RefText ,a.Scan1 ,a.Scan2,a.ArchStatus ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ,a.affiliatedPM ", "PM_Contracts a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.Type = c.Value AND (c.CodeType='HtType') LEFT JOIN BD_Customers d ON a.CustID = d.FID LEFT JOIN [Bcp.Sysy].dbo.sys_code e ON a.Region = e.Value AND (e.CodeType='Region') LEFT JOIN [Bcp.Sysy].dbo.sys_code f ON a.Scan1 = f.Value AND (f.CodeType='GCLB') ", sbCondition.ToString(), "a.Code", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PM_Contracts>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PM_Contracts.Find(form.Code);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PM_Contracts.Add(form);
                }
                else
                {
                    dbForm.Corp = form.Corp;
                    dbForm.Type = form.Type;
                    dbForm.Amt = form.Amt;
                    dbForm.CustID = form.CustID;
                    dbForm.Owner = form.Owner;
                    dbForm.Situation = form.Situation;
                    dbForm.taxAmt = form.taxAmt;
                    dbForm.Region = form.Region;
                    dbForm.FDate = form.FDate;
                    dbForm.Remark = form.Remark;
                    dbForm.Scan1 = form.Scan1;
                    dbForm.FID = form.FID;
                    dbForm.affiliatedPM = form.affiliatedPM;
                    dbForm.Receiver = form.Receiver;
                    dbForm.Scan1 = form.Scan1;
                    dbForm.Scan2 = form.Scan2;
                    dbForm.ArchStatus = form.ArchStatus;
                    dbForm.Scan3 = form.Scan3;
                    dbForm.Scan4 = form.Scan4;
                    dbForm.EDate = form.EDate;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        [System.Web.Http.HttpPost]
        public object DoBtGuiDang(JObject row)
        {
            object result = null;
            using (var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec PM_GuiD @FID";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Code");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.PM_Contracts.Remove(r => r.Code == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewCode()
        {
            return string.Format("{0}{1}{2:D4}", System.Web.HttpContext.Current.Session["UserName"], DateTime.Now.Year.ToString(), new SysSerialServices().GetNewSerial("PM_Contracts", null));
        }

    }
}
