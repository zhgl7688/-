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
    public partial class Fw_CategoryinfoController : Controller
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
            fw_categoryinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_categoryinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_categoryinfo
                {
                    catid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_CategoryinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_categoryinfoListModel {
            public int catid { get; set; }
            public string catname { get; set; }
            public int? parentid { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_categoryinfoListModel>(db.Database, "a.catid ,a.catname ,a.parentid ", "fw_categoryinfo a ", sbCondition.ToString(), "a.catid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_categoryinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_categoryinfo.Find(form.catid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_categoryinfo.Add(form);
                }
                else
                {
                    dbForm.catname = form.catname;
                    dbForm.parentid = form.parentid;
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
                    db.fw_categoryinfo.Remove(r => r.catid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newcatid()
        {
            return new SysSerialServices().GetNewSerial("fw_categoryinfo");
        }

    }
}
