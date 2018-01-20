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
    public partial class InvfeeController : Controller
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
        public ActionResult Edit(long id)
        {
            BD_InvFees form = null;
            List<ComboItem> CustID = null, invType = null;
            using(var db = new LUOLAI1401Context())
            {
                CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" + i.FID }).ToList();
                invType = db.HR_NewInvoices.Select(i=>new ComboItem{ Text = i.Remarks, Value = "" + i.FID }).ToList();
                form = db.BD_InvFees.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new BD_InvFees
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { CustID,invType }});
        }

    }
    public partial class InvfeeApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class BD_InvFeesListModel {
            public long FID { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string invType { get; set; }
            public string invType_RefText { get; set; }
            public decimal Ratio { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<BD_InvFeesListModel>(db.Database, "a.FID ,b.Contact CustID_RefText ,a.CustID ,c.Remarks invType_RefText ,a.invType ,a.Ratio ,a.Remark ,a.CreatePerson ,a.CreateDate ", "BD_InvFees a LEFT JOIN BD_Customers b ON a.CustID = b.FID LEFT JOIN HR_NewInvoices c ON a.invType = c.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<BD_InvFees>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.BD_InvFees.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.BD_InvFees.Add(form);
                }
                else
                {
                    dbForm.CustID = form.CustID;
                    dbForm.invType = form.invType;
                    dbForm.Ratio = form.Ratio;
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
            var _ids = new List<long>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = long.Parse(_id);
                    db.BD_InvFees.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("BD_InvFees");
        }

    }
}
