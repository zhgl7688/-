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
    public partial class A_GuidController : Controller
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
            a_guid form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.a_guid.Find(Guid.Parse(id));
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new a_guid
                {
                    id = Guid.Parse(id)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class A_GuidApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class a_guidListModel {
            public Guid id { get; set; }
            public string name { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<a_guidListModel>(db.Database, "a.id ,a.name ", "a_guid a ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<a_guid>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.a_guid.Find(form.id);
                if (dbForm == null)
                {
                    db.a_guid.Add(form);
                }
                else
                {
                    dbForm.name = form.name;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                var guid = Guid.Parse(id);
                db.a_guid.Remove(r => r.id == guid);
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return Guid.NewGuid();
        }

    }
}
