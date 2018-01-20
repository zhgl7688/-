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
    public partial class Fw_HelpinfoController : Controller
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
            fw_helpinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_helpinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_helpinfo
                {
                    helpid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_HelpinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_helpinfoListModel {
            public string helpid { get; set; }
            public string helptitle { get; set; }
            public string helpcontent { get; set; }
            public int? isenabled { get; set; }
            public int? handleuserid { get; set; }
            public string createtime { get; set; }
            public string modifytime { get; set; }
            public string helpcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_helpinfoListModel>(db.Database, "a.helpid ,a.helptitle ,a.helpcontent ,a.isenabled ,a.handleuserid ,a.createtime ,a.modifytime ,a.helpcode ", "fw_helpinfo a ", sbCondition.ToString(), "a.helpid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_helpinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_helpinfo.Find(form.helpid);
                if (dbForm == null)
                {
                    db.fw_helpinfo.Add(form);
                }
                else
                {
                    dbForm.helptitle = form.helptitle;
                    dbForm.helpcontent = form.helpcontent;
                    dbForm.isenabled = form.isenabled;
                    dbForm.handleuserid = form.handleuserid;
                    dbForm.createtime = form.createtime;
                    dbForm.modifytime = form.modifytime;
                    dbForm.helpcode = form.helpcode;
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
                    db.fw_helpinfo.Remove(r => r.helpid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newhelpid()
        {
            return new SysSerialServices().GetNewSerial("fw_helpinfo");
        }

    }
}
