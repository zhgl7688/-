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
    public partial class Fw_MessageinfoController : Controller
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
            fw_messageinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_messageinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_messageinfo
                {
                    messid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_MessageinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_messageinfoListModel {
            public int messid { get; set; }
            public string fromid { get; set; }
            public string toid { get; set; }
            public string title { get; set; }
            public string messcontent { get; set; }
            public string attachmenturl { get; set; }
            public DateTime? createtime { get; set; }
            public int? isread { get; set; }
            public int? isdelete { get; set; }
            public string messcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_messageinfoListModel>(db.Database, "a.messid ,a.fromid ,a.toid ,a.title ,a.messcontent ,a.attachmenturl ,a.createtime ,a.isread ,a.isdelete ,a.messcode ", "fw_messageinfo a ", sbCondition.ToString(), "a.messid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_messageinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_messageinfo.Find(form.messid);
                if (dbForm == null)
                {
                    db.fw_messageinfo.Add(form);
                }
                else
                {
                    dbForm.fromid = form.fromid;
                    dbForm.toid = form.toid;
                    dbForm.title = form.title;
                    dbForm.messcontent = form.messcontent;
                    dbForm.attachmenturl = form.attachmenturl;
                    dbForm.createtime = form.createtime;
                    dbForm.isread = form.isread;
                    dbForm.isdelete = form.isdelete;
                    dbForm.messcode = form.messcode;
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
                    db.fw_messageinfo.Remove(r => r.messid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newmessid()
        {
            return new SysSerialServices().GetNewSerial("fw_messageinfo");
        }

    }
}
