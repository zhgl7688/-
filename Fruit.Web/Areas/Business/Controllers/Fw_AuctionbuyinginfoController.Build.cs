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
    public partial class Fw_AuctionbuyinginfoController : Controller
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
            fw_auctionbuyinginfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_auctionbuyinginfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_auctionbuyinginfo
                {
                    aucid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_AuctionbuyinginfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_auctionbuyinginfoListModel {
            public int aucid { get; set; }
            public int? proid { get; set; }
            public decimal? primePrice { get; set; }
            public decimal? floorPrice { get; set; }
            public int? addPrice { get; set; }
            public decimal? cellingPrice { get; set; }
            public int? counts { get; set; }
            public string memid { get; set; }
            public DateTime? starttime { get; set; }
            public DateTime? endtime { get; set; }
            public int? minUserCount { get; set; }
            public string imgurl { get; set; }
            public string remark { get; set; }
            public int? isactive { get; set; }
            public string auccode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_auctionbuyinginfoListModel>(db.Database, "a.aucid ,a.proid ,a.primePrice ,a.floorPrice ,a.addPrice ,a.cellingPrice ,a.counts ,a.memid ,a.starttime ,a.endtime ,a.minUserCount ,a.imgurl ,a.remark ,a.isactive ,a.auccode ", "fw_auctionbuyinginfo a ", sbCondition.ToString(), "a.aucid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_auctionbuyinginfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_auctionbuyinginfo.Find(form.aucid);
                if (dbForm == null)
                {
                    db.fw_auctionbuyinginfo.Add(form);
                }
                else
                {
                    dbForm.proid = form.proid;
                    dbForm.primePrice = form.primePrice;
                    dbForm.floorPrice = form.floorPrice;
                    dbForm.addPrice = form.addPrice;
                    dbForm.cellingPrice = form.cellingPrice;
                    dbForm.counts = form.counts;
                    dbForm.memid = form.memid;
                    dbForm.starttime = form.starttime;
                    dbForm.endtime = form.endtime;
                    dbForm.minUserCount = form.minUserCount;
                    dbForm.imgurl = form.imgurl;
                    dbForm.remark = form.remark;
                    dbForm.isactive = form.isactive;
                    dbForm.auccode = form.auccode;
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
                    db.fw_auctionbuyinginfo.Remove(r => r.aucid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newaucid()
        {
            return new SysSerialServices().GetNewSerial("fw_auctionbuyinginfo");
        }

    }
}
