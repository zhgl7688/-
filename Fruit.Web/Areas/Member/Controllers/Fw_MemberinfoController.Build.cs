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
    public partial class Fw_MemberinfoController : Controller
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
        public ActionResult Edit(string id)
        {
            fw_memberinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_memberinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_memberinfo
                {
                    memid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_MemberinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_memberinfoListModel {
            public string memid { get; set; }
            public string mempass { get; set; }
            public string realname { get; set; }
            public int? gender { get; set; }
            public string email { get; set; }
            public string qq { get; set; }
            public string wechat { get; set; }
            public DateTime? regtime { get; set; }
            public DateTime? lasttime { get; set; }
            public int? ispassed { get; set; }
            public int? isenabled { get; set; }
            public int? corpid { get; set; }
            public string memcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_memberinfoListModel>(db.Database, "a.memid ,a.mempass ,a.realname ,a.gender ,a.email ,a.qq ,a.wechat ,a.regtime ,a.lasttime ,a.ispassed ,a.isenabled ,a.corpid ,a.memcode ", "fw_memberinfo a ", sbCondition.ToString(), "a.memid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_memberinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_memberinfo.Find(form.memid);
                if (dbForm == null)
                {
                    db.fw_memberinfo.Add(form);
                }
                else
                {
                    dbForm.mempass = form.mempass;
                    dbForm.realname = form.realname;
                    dbForm.gender = form.gender;
                    dbForm.email = form.email;
                    dbForm.qq = form.qq;
                    dbForm.wechat = form.wechat;
                    dbForm.regtime = form.regtime;
                    dbForm.lasttime = form.lasttime;
                    dbForm.ispassed = form.ispassed;
                    dbForm.isenabled = form.isenabled;
                    dbForm.corpid = form.corpid;
                    dbForm.memcode = form.memcode;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
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
                    db.fw_memberinfo.Remove(r => r.memid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newmemid()
        {
            return new SysSerialServices().GetNewSerial("fw_memberinfo");
        }

    }
}
