/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class DeltestController : Controller
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
            a form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.a.Find(Guid.Parse(id));
            }
            if (form == null)
            {
                form = new a
                {
                    id = Guid.Parse(id)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class DeltestApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class aListModel {
            public Guid id { get; set; }
            public string NameD { get; set; }
        }
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<aListModel>(db.Database, "a.id ,a.NameD ", "a a ", "", "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<a>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.a.Find(form.id);
                if (dbForm == null)
                {
                    db.a.Add(form);
                }
                else
                {
                    dbForm.NameD = form.NameD;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                db.a.Remove(r => r.id == Guid.Parse(id));
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
