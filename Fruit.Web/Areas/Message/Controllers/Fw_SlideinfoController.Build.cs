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

namespace Fruit.Web.Areas.Message.Controllers
{
    public partial class Fw_SlideinfoController : Controller
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
            fw_slideinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_slideinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_slideinfo
                {
                    id = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_SlideinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_slideinfoListModel {
            public int id { get; set; }
            public string title { get; set; }
            public int? clickcounts { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "title", "a.title", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_slideinfoListModel>(db.Database, "a.id ,a.title ,a.clickcounts ", "fw_slideinfo a ", sbCondition.ToString(), "a.id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_slideinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_slideinfo.Find(form.id);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_slideinfo.Add(form);
                }
                else
                {
                    dbForm.title = form.title;
                    dbForm.imgurl = form.imgurl;
                    dbForm.linkurl = form.linkurl;
                    dbForm.clickcounts = form.clickcounts;
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
                    db.fw_slideinfo.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return new SysSerialServices().GetNewSerial("fw_slideinfo");
        }

    }
}
