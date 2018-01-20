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
    public partial class Fw_RecordinfoController : Controller
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
            fw_recordinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_recordinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_recordinfo
                {
                    Id = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_RecordinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_recordinfoListModel {
            public int Id { get; set; }
            public int? fromid { get; set; }
            public string frommodule { get; set; }
            public decimal? price { get; set; }
            public decimal? counts { get; set; }
            public string memid { get; set; }
            public DateTime? createdate { get; set; }
            public string fromcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_recordinfoListModel>(db.Database, "a.Id ,a.fromid ,a.frommodule ,a.price ,a.counts ,a.memid ,a.createdate ,a.fromcode ", "fw_recordinfo a ", sbCondition.ToString(), "a.Id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_recordinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_recordinfo.Find(form.Id);
                if (dbForm == null)
                {
                    db.fw_recordinfo.Add(form);
                }
                else
                {
                    dbForm.fromid = form.fromid;
                    dbForm.frommodule = form.frommodule;
                    dbForm.price = form.price;
                    dbForm.counts = form.counts;
                    dbForm.memid = form.memid;
                    dbForm.createdate = form.createdate;
                    dbForm.fromcode = form.fromcode;
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
                    db.fw_recordinfo.Remove(r => r.Id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewId()
        {
            return new SysSerialServices().GetNewSerial("fw_recordinfo");
        }

    }
}
