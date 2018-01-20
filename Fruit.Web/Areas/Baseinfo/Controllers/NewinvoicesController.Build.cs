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

namespace Fruit.Web.Areas.Baseinfo.Controllers
{
    public partial class NewinvoicesController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            HR_NewInvoices form = null;
            List<ComboItem> CustID = null, BillType = null;
            using(var db = new SysContext())
            {
                CustID = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
                BillType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='invoiceType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.HR_NewInvoices.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_NewInvoices
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { CustID,BillType }});
        }

    }
    public partial class NewinvoicesApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_NewInvoicesListModel {
            public string FID { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string TaxRate { get; set; }
            public string BillType { get; set; }
            public string BillType_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_NewInvoicesListModel>(db.Database, "a.FID ,b.Text CustID_RefText ,a.CustID ,a.TaxRate ,c.Text BillType_RefText ,a.BillType ", "HR_NewInvoices a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.CustID = b.Value AND (b.CodeType='FType') LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.BillType = c.Value AND (c.CodeType='invoiceType') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_NewInvoices>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_NewInvoices.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_NewInvoices.Add(form);
                }
                else
                {
                    dbForm.CustID = form.CustID;
                    dbForm.TaxRate = form.TaxRate;
                    dbForm.BillType = form.BillType;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "UPDATE HR_NewInvoices SET Remarks=text FROM [Bcp.Sysy].dbo.sys_code WHERE CodeType='FType' AND CustID=Value AND FID=@FID;UPDATE HR_NewInvoices SET Remarks=Remarks+'--'+text+'--税率 '+TaxRate FROM [Bcp.Sysy].dbo.sys_code WHERE CodeType='invoiceType' AND BillType=Value AND FID=@FID";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = form.FID;
                cmd.Parameters.Add(p1);
                cmd.ExecuteNonQuery();
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
                    db.HR_NewInvoices.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("HR_NewInvoices", null));
        }

    }
}
