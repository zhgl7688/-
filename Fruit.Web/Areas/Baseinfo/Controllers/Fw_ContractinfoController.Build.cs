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
    public partial class Fw_ContractinfoController : Controller
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
            fw_contractinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_contractinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_contractinfo
                {
                    conid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_ContractinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_contractinfoListModel {
            public string conid { get; set; }
            public string conname { get; set; }
            public string linkurl { get; set; }
            public int? handleuserid { get; set; }
            public string createtime { get; set; }
            public string modifytime { get; set; }
            public string concode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_contractinfoListModel>(db.Database, "a.conid ,a.conname ,a.linkurl ,a.handleuserid ,a.createtime ,a.modifytime ,a.concode ", "fw_contractinfo a ", sbCondition.ToString(), "a.conid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_contractinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_contractinfo.Find(form.conid);
                if (dbForm == null)
                {
                    db.fw_contractinfo.Add(form);
                }
                else
                {
                    dbForm.conname = form.conname;
                    dbForm.linkurl = form.linkurl;
                    dbForm.handleuserid = form.handleuserid;
                    dbForm.createtime = form.createtime;
                    dbForm.modifytime = form.modifytime;
                    dbForm.concode = form.concode;
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
                    db.fw_contractinfo.Remove(r => r.conid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newconid()
        {
            return new SysSerialServices().GetNewSerial("fw_contractinfo");
        }

    }
}
