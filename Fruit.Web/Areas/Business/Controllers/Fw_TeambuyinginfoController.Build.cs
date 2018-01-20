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
    public partial class Fw_TeambuyinginfoController : Controller
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
            fw_teambuyinginfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_teambuyinginfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_teambuyinginfo
                {
                    teamid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_TeambuyinginfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_teambuyinginfoListModel {
            public int teamid { get; set; }
            public int? proid { get; set; }
            public decimal? primePrice { get; set; }
            public decimal? teamPrice { get; set; }
            public int? counts { get; set; }
            public string memid { get; set; }
            public DateTime? starttime { get; set; }
            public DateTime? Endtime { get; set; }
            public int? minUserCount { get; set; }
            public decimal? minProdCount { get; set; }
            public string imgurl { get; set; }
            public string remark { get; set; }
            public int? isactive { get; set; }
            public string teamcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_teambuyinginfoListModel>(db.Database, "a.teamid ,a.proid ,a.primePrice ,a.teamPrice ,a.counts ,a.memid ,a.starttime ,a.Endtime ,a.minUserCount ,a.minProdCount ,a.imgurl ,a.remark ,a.isactive ,a.teamcode ", "fw_teambuyinginfo a ", sbCondition.ToString(), "a.teamid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_teambuyinginfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_teambuyinginfo.Find(form.teamid);
                if (dbForm == null)
                {
                    db.fw_teambuyinginfo.Add(form);
                }
                else
                {
                    dbForm.proid = form.proid;
                    dbForm.primePrice = form.primePrice;
                    dbForm.teamPrice = form.teamPrice;
                    dbForm.counts = form.counts;
                    dbForm.memid = form.memid;
                    dbForm.starttime = form.starttime;
                    dbForm.Endtime = form.Endtime;
                    dbForm.minUserCount = form.minUserCount;
                    dbForm.minProdCount = form.minProdCount;
                    dbForm.imgurl = form.imgurl;
                    dbForm.remark = form.remark;
                    dbForm.isactive = form.isactive;
                    dbForm.teamcode = form.teamcode;
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
                    db.fw_teambuyinginfo.Remove(r => r.teamid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newteamid()
        {
            return new SysSerialServices().GetNewSerial("fw_teambuyinginfo");
        }

    }
}
