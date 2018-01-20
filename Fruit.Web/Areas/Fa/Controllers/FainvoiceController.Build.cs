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

namespace Fruit.Web.Areas.Fa.Controllers
{
    public partial class FainvoiceController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> Corp;
                using(var db = new SysContext())
                {
                    Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                }
                return View(new {dataSource = new {Corp}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_InvoicesViewable : FA_Invoices {
            public string CustID_RefText { get; set; }
            public string PID_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            FA_Invoices form = null;
            List<ComboItem> Corp = null, taxType = null, invoiceType = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                taxType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='taxType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                invoiceType = db.HR_NewInvoices.Select(i=>new ComboItem{ Text = i.Remarks, Value = "" + i.FID }).ToList();
                form = db.Database.SqlQuery<FA_InvoicesViewable>("select a.FID ,a.InvoicesID ,a.invoiceCode ,a.Corp ,b.Contact CustID_RefText ,a.CustID ,c.Code PID_RefText ,a.PID ,a.FDate ,a.Amt ,a.invoiceTitle ,a.taxNumber ,a.taxType ,a.invoiceType ,a.invoiceContent ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate  from FA_Invoices a LEFT JOIN BD_Customers b ON a.CustID = b.FID LEFT JOIN PM_Contracts c ON a.PID = c.Code  where a.InvoicesID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new FA_Invoices
                {
                    InvoicesID = id,
                    taxType = string.Format("{0}", "1"),
                    invoiceType = string.Format("{0}", "5")
                };
            }
            return View(new { form = form, dataSource = new { Corp,taxType,invoiceType }});
        }

    }
    public partial class FainvoiceApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_InvoicesListModel {
            public string InvoicesID { get; set; }
            public string invoiceCode { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string PID { get; set; }
            public string PID_RefText { get; set; }
            public DateTime FDate { get; set; }
            public decimal Amt { get; set; }
            public string invoiceTitle { get; set; }
            public string taxNumber { get; set; }
            public string taxType { get; set; }
            public string taxType_RefText { get; set; }
            public string invoiceType { get; set; }
            public string invoiceType_RefText { get; set; }
            public string invoiceContent { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "Corp", "a.Corp", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<FA_InvoicesListModel>(db.Database, "a.InvoicesID ,a.invoiceCode ,b.Text Corp_RefText ,a.Corp ,c.Contact CustID_RefText ,a.CustID ,d.Code PID_RefText ,a.PID ,a.FDate ,a.Amt ,a.invoiceTitle ,a.taxNumber ,e.Text taxType_RefText ,a.taxType ,f.Remarks invoiceType_RefText ,a.invoiceType ,a.invoiceContent ,a.Remark ", "FA_Invoices a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN BD_Customers c ON a.CustID = c.FID LEFT JOIN PM_Contracts d ON a.PID = d.Code LEFT JOIN [Bcp.Sysy].dbo.sys_code e ON a.taxType = e.Value AND (e.CodeType='taxType') LEFT JOIN HR_NewInvoices f ON a.invoiceType = f.FID ", sbCondition.ToString(), "a.InvoicesID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<FA_Invoices>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.FA_Invoices.Find(form.InvoicesID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.FA_Invoices.Add(form);
                }
                else
                {
                    dbForm.invoiceCode = form.invoiceCode;
                    dbForm.Corp = form.Corp;
                    dbForm.CustID = form.CustID;
                    dbForm.PID = form.PID;
                    dbForm.FDate = form.FDate;
                    dbForm.Amt = form.Amt;
                    dbForm.invoiceTitle = form.invoiceTitle;
                    dbForm.taxNumber = form.taxNumber;
                    dbForm.taxType = form.taxType;
                    dbForm.invoiceType = form.invoiceType;
                    dbForm.invoiceContent = form.invoiceContent;
                    dbForm.Remark = form.Remark;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.FA_Invoices.Remove(r => r.InvoicesID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewInvoicesID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("FA_Invoices", null));
        }

    }
}
