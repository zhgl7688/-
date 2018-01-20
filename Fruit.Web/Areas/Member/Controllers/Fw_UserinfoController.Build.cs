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

namespace Fruit.Web.Areas.Member.Controllers
{
    public partial class Fw_UserinfoController : Controller
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
            fw_userinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_userinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_userinfo
                {
                    userid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_UserinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_userinfoListModel {
            public int userid { get; set; }
            public string username { get; set; }
            public string userpass { get; set; }
            public string regtime { get; set; }
            public string lasttime { get; set; }
            public int? isadmin { get; set; }
            public int? isenabled { get; set; }
            public string usercode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_userinfoListModel>(db.Database, "a.userid ,a.username ,a.userpass ,a.regtime ,a.lasttime ,a.isadmin ,a.isenabled ,a.usercode ", "fw_userinfo a ", sbCondition.ToString(), "a.userid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_userinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_userinfo.Find(form.userid);
                if (dbForm == null)
                {
                    db.fw_userinfo.Add(form);
                }
                else
                {
                    dbForm.username = form.username;
                    dbForm.userpass = form.userpass;
                    dbForm.regtime = form.regtime;
                    dbForm.lasttime = form.lasttime;
                    dbForm.isadmin = form.isadmin;
                    dbForm.isenabled = form.isenabled;
                    dbForm.usercode = form.usercode;
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
                    db.fw_userinfo.Remove(r => r.userid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newuserid()
        {
            return new SysSerialServices().GetNewSerial("fw_userinfo");
        }

    }
}
