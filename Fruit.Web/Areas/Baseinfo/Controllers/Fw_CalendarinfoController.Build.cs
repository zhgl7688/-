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
    public partial class Fw_CalendarinfoController : Controller
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
        public ActionResult Edit(DateTime id)
        {
            fw_calendarinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_calendarinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_calendarinfo
                {
                    caldate = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_CalendarinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_calendarinfoListModel {
            public string caldate { get; set; }
            public DateTime? starttime { get; set; }
            public DateTime? endtime { get; set; }
            public int? handleuserid { get; set; }
            public DateTime? handletime { get; set; }
            public string calcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_calendarinfoListModel>(db.Database, "a.caldate ,a.starttime ,a.endtime ,a.handleuserid ,a.handletime ,a.calcode ", "fw_calendarinfo a ", sbCondition.ToString(), "a.caldate", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_calendarinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_calendarinfo.Find(form.caldate);
                if (dbForm == null)
                {
                    db.fw_calendarinfo.Add(form);
                }
                else
                {
                    dbForm.starttime = form.starttime;
                    dbForm.endtime = form.endtime;
                    dbForm.handleuserid = form.handleuserid;
                    dbForm.handletime = form.handletime;
                    dbForm.calcode = form.calcode;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<DateTime>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = DateTime.Parse(_id);
                    db.fw_calendarinfo.Remove(r => r.caldate == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newcaldate()
        {
            return new SysSerialServices().GetNewSerial("fw_calendarinfo");
        }

    }
}
