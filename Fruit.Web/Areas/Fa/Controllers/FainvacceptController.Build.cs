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
    public partial class FainvacceptController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_InvAcceptsViewable : FA_InvAccepts {
            public string CustID_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            FA_InvAccepts form = null;
            List<ComboItem> Corp = null, invoiceType = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                invoiceType = db.HR_NewInvoices.Select(i=>new ComboItem{ Text = i.Remarks, Value = "" + i.FID }).ToList();
                form = db.Database.SqlQuery<FA_InvAcceptsViewable>("select a.FID ,a.Corp ,b.Contact CustID_RefText ,a.CustID ,a.PID ,a.Fdate ,a.Amt ,a.invoiceTitle ,a.invoiceType ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate  from FA_InvAccepts a LEFT JOIN BD_Customers b ON a.CustID = b.FID  where a.FID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new FA_InvAccepts
                {
                    FID = id,
                    invoiceType = string.Format("{0}", "5")
                };
            }
            return View(new { form = form, dataSource = new { Corp,invoiceType }});
        }

    }
    public partial class FainvacceptApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_InvAcceptsListModel {
            public string FID { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string PID { get; set; }
            public DateTime Fdate { get; set; }
            public decimal Amt { get; set; }
            public string invoiceTitle { get; set; }
            public string invoiceType { get; set; }
            public string invoiceType_RefText { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "Corp", "a.Corp", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<FA_InvAcceptsListModel>(db.Database, "a.FID ,b.Text Corp_RefText ,a.Corp ,c.Contact CustID_RefText ,a.CustID ,a.PID ,a.Fdate ,a.Amt ,a.invoiceTitle ,d.Remarks invoiceType_RefText ,a.invoiceType ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ", "FA_InvAccepts a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN BD_Customers c ON a.CustID = c.FID LEFT JOIN HR_NewInvoices d ON a.invoiceType = d.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<FA_InvAccepts>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.FA_InvAccepts.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.FA_InvAccepts.Add(form);
                }
                else
                {
                    dbForm.Corp = form.Corp;
                    dbForm.CustID = form.CustID;
                    dbForm.PID = form.PID;
                    dbForm.Fdate = form.Fdate;
                    dbForm.Amt = form.Amt;
                    dbForm.invoiceTitle = form.invoiceTitle;
                    dbForm.invoiceType = form.invoiceType;
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
                    db.FA_InvAccepts.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("FA_InvAccepts", null));
        }

    }
}
