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
    public partial class BasesupperController : Controller
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
            tbBdLitigant form = null;
            using(var db = new SMTERPContext())
            {
                form = db.tbBdLitigant.Find(Guid.Parse(id));
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new tbBdLitigant
                {
                    Code = Guid.Parse(id)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class BasesupperApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class tbBdLitigantListModel {
            public Guid Code { get; set; }
            public Guid? PeopleCode { get; set; }
            public string Name { get; set; }
            public string Remark { get; set; }
            public bool? IfStop { get; set; }
            public int? Sort { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new SMTERPContext())
            {
                return pageReq.ToPageList<tbBdLitigantListModel>(db.Database, "a.Code ,a.PeopleCode ,a.Name ,a.Remark ,a.IfStop ,a.Sort ", "tbBdLitigant a ", sbCondition.ToString(), "Code", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<tbBdLitigant>();
            using (var db = new SMTERPContext())
            {
                var dbForm = db.tbBdLitigant.Find(form.Code);
                if (dbForm == null)
                {
                    db.tbBdLitigant.Add(form);
                }
                else
                {
                    dbForm.PeopleCode = form.PeopleCode;
                    dbForm.Name = form.Name;
                    dbForm.Remark = form.Remark;
                    dbForm.IfStop = form.IfStop;
                    dbForm.Sort = form.Sort;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new SMTERPContext())
            {
                var guid = Guid.Parse(id);
                db.tbBdLitigant.Remove(r => r.Code == guid);
                db.SaveChanges();
            }
            return true;
        }
        public object NewCode()
        {
            return Guid.NewGuid();
        }

    }
}
