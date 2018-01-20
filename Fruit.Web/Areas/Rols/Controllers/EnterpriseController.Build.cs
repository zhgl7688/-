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

namespace Fruit.Web.Areas.Rols.Controllers
{
    public partial class EnterpriseController : Controller
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
        public ActionResult Edit(int id)
        {
            EnterpriseCertificate form = null;
            List<ComboItem> Company = null;
            using(var db = new SysContext())
            {
                Company = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.EnterpriseCertificate.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new EnterpriseCertificate
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { Company }});
        }

    }
    public partial class EnterpriseApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class EnterpriseCertificateListModel {
            public int? FID { get; set; }
            public string Company { get; set; }
            public string Company_RefText { get; set; }
            public string Name { get; set; }
            public string EffectiveDate { get; set; }
            public int? Qty { get; set; }
            public string onProject { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<EnterpriseCertificateListModel>(db.Database, "a.FID ,b.Text Company_RefText ,a.Company ,a.Name ,a.EffectiveDate ,a.Qty ,a.onProject ", "EnterpriseCertificate a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Company = b.Value AND (b.CodeType='HtCorp') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<EnterpriseCertificate>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.EnterpriseCertificate.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.EnterpriseCertificate.Add(form);
                }
                else
                {
                    dbForm.Company = form.Company;
                    dbForm.Name = form.Name;
                    dbForm.EffectiveDate = form.EffectiveDate;
                    dbForm.Qty = form.Qty;
                    dbForm.onProject = form.onProject;
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
                    db.EnterpriseCertificate.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("EnterpriseCertificate", null));
        }

    }
}
