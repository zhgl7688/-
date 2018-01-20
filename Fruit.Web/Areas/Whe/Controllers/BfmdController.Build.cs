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

namespace Fruit.Web.Areas.Whe.Controllers
{
    public partial class BfmdController : Controller
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
            base_customField form = null;
            List<ComboItem> TypeCode = null;
            using(var db = new SysContext())
            {
                TypeCode = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='patrolSignIn'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.base_customField.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new base_customField
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { TypeCode }});
        }

    }
    public partial class BfmdApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class base_customFieldListModel {
            public int id { get; set; }
            public string TypeCode { get; set; }
            public string TypeCode_RefText { get; set; }
            public string TypeName { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<base_customFieldListModel>(db.Database, "a.id ,b.Text TypeCode_RefText ,a.TypeCode ,a.TypeName ", "base_customField a LEFT JOIN [Bcp.Sys].dbo.sys_code b ON a.TypeCode = b.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<base_customField>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.base_customField.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    db.base_customField.Add(form);
                }
                else
                {
                    dbForm.TypeCode = form.TypeCode;
                    dbForm.TypeName = form.TypeName;
                }
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
                    db.base_customField.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("base_customField", null));
        }

    }
}
