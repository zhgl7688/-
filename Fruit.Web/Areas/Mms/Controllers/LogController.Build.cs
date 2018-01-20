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
    public partial class LogController : Controller
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
        public ActionResult Edit(int id)
        {
            log form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.log.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new log
                {
                    id = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class LogApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class logListModel {
            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<logListModel>(db.Database, "a.id ,a.title ,a.description ", "log a ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<log>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.log.Find(form.id);
                if (dbForm == null)
                {
                    db.log.Add(form);
                }
                else
                {
                    dbForm.title = form.title;
                    dbForm.description = form.description;
                    dbForm.content = form.content;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(int id)
        {

            using (var db = new LUOLAI1401Context())
            {
                db.log.Remove(r => r.id == id);
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return new SysSerialServices().GetNewSerial("log");
        }

    }
}
