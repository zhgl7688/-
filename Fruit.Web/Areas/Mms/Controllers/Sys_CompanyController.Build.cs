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

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class Sys_CompanyController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
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
            wq_appTrans form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_appTrans.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_appTrans
                {
                    id = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Sys_CompanyApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_appTransListModel {
            public string CompCode { get; set; }
            public string iVer { get; set; }
            public string TransCode { get; set; }
            public string Description { get; set; }
            public string appiVer { get; set; }
            public string id { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "'", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='77889'"));

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_appTransListModel>(db.Database, "a.CompCode ,a.iVer ,a.TransCode ,a.Description ,a.appiVer ,a.id ", "wq_appTrans a ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_appTrans>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_appTrans.Find(form.id);
                if (dbForm == null)
                {
                    db.wq_appTrans.Add(form);
                }
                else
                {
                    dbForm.CompCode = form.CompCode;
                    dbForm.iVer = form.iVer;
                    dbForm.TransCode = form.TransCode;
                    dbForm.Description = form.Description;
                    dbForm.appiVer = form.appiVer;
                }
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
                    db.wq_appTrans.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_appTrans", null));
        }

    }
}
