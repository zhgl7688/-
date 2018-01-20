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

namespace Fruit.Web.Areas.Business.Controllers
{
    public partial class Fw_LongterminfoController : Controller
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
            fw_longterminfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_longterminfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_longterminfo
                {
                    longid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_LongterminfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_longterminfoListModel {
            public int longid { get; set; }
            public string longname { get; set; }
            public string mode { get; set; }
            public string remark { get; set; }
            public string memid { get; set; }
            public string url { get; set; }
            public DateTime? createdate { get; set; }
            public DateTime? modifydate { get; set; }
            public int? isactive { get; set; }
            public string longcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_longterminfoListModel>(db.Database, "a.longid ,a.longname ,a.mode ,a.remark ,a.memid ,a.url ,a.createdate ,a.modifydate ,a.isactive ,a.longcode ", "fw_longterminfo a ", sbCondition.ToString(), "a.longid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_longterminfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_longterminfo.Find(form.longid);
                if (dbForm == null)
                {
                    db.fw_longterminfo.Add(form);
                }
                else
                {
                    dbForm.longname = form.longname;
                    dbForm.mode = form.mode;
                    dbForm.remark = form.remark;
                    dbForm.memid = form.memid;
                    dbForm.url = form.url;
                    dbForm.createdate = form.createdate;
                    dbForm.modifydate = form.modifydate;
                    dbForm.isactive = form.isactive;
                    dbForm.longcode = form.longcode;
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
                    db.fw_longterminfo.Remove(r => r.longid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newlongid()
        {
            return new SysSerialServices().GetNewSerial("fw_longterminfo");
        }

    }
}
