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
    public partial class PrepaidController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> CustID, Corp;
                using(var db = new LUOLAI1401Context())
                {
                    CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" +  i.FID }).ToList();
                }
                using(var db = new SysContext())
                {
                    Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                }
                return View(new {dataSource = new {CustID,Corp}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PrepaidTaxViewable : PrepaidTax {
            public string Invoice_RefText { get; set; }
        }
        public ActionResult Edit(int id)
        {
            PrepaidTax form = null;
            List<ComboItem> Corp = null, CustID = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" + i.FID }).ToList();
                form = db.Database.SqlQuery<PrepaidTaxViewable>("select a.FID ,a.Code ,a.Corp ,a.CustID ,b.invoiceTitle Invoice_RefText ,a.Invoice ,a.Amt ,a.DeductibleAmt ,a.ReceiptDate ,a.InvoicesRise ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate  from PrepaidTax a LEFT JOIN FA_Invoices b ON a.Invoice = b.invoiceCode  where a.FID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PrepaidTax
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { Corp,CustID }});
        }

    }
    public partial class PrepaidApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PrepaidTaxListModel {
            public int? FID { get; set; }
            public string Code { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public int? CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string Invoice { get; set; }
            public string Invoice_RefText { get; set; }
            public decimal? Amt { get; set; }
            public decimal? DeductibleAmt { get; set; }
            public string ReceiptDate { get; set; }
            public string InvoicesRise { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "Corp", "a.Corp", "");
            SerachCondition.Dropdown(sbCondition, "CustID", "a.CustID", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PrepaidTaxListModel>(db.Database, "a.FID ,a.Code ,b.Text Corp_RefText ,a.Corp ,c.Contact CustID_RefText ,a.CustID ,d.invoiceTitle Invoice_RefText ,a.Invoice ,a.Amt ,a.DeductibleAmt ,a.ReceiptDate ,a.InvoicesRise ", "PrepaidTax a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN BD_Customers c ON a.CustID = c.FID LEFT JOIN FA_Invoices d ON a.Invoice = d.invoiceCode ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PrepaidTax>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PrepaidTax.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PrepaidTax.Add(form);
                }
                else
                {
                    dbForm.Code = form.Code;
                    dbForm.Corp = form.Corp;
                    dbForm.CustID = form.CustID;
                    dbForm.Invoice = form.Invoice;
                    dbForm.Amt = form.Amt;
                    dbForm.DeductibleAmt = form.DeductibleAmt;
                    dbForm.ReceiptDate = form.ReceiptDate;
                    dbForm.InvoicesRise = form.InvoicesRise;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.PrepaidTax.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("PrepaidTax", null));
        }

    }
}
