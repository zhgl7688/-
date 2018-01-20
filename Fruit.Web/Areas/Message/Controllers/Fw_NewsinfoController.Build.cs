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
    public partial class Fw_NewsinfoController : Controller
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
            fw_newsinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_newsinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_newsinfo
                {
                    newsid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_NewsinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_newsinfoListModel {
            public int newsid { get; set; }
            public string newstitle { get; set; }
            public string newscontent { get; set; }
            public string category { get; set; }
            public int? clicks { get; set; }
            public int? isslide { get; set; }
            public string slidepicurl { get; set; }
            public string imgurl { get; set; }
            public int? userid { get; set; }
            public DateTime? pubtime { get; set; }
            public string newscode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_newsinfoListModel>(db.Database, "a.newsid ,a.newstitle ,a.newscontent ,a.category ,a.clicks ,a.isslide ,a.slidepicurl ,a.imgurl ,a.userid ,a.pubtime ,a.newscode ", "fw_newsinfo a ", sbCondition.ToString(), "a.newsid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_newsinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_newsinfo.Find(form.newsid);
                if (dbForm == null)
                {
                    db.fw_newsinfo.Add(form);
                }
                else
                {
                    dbForm.newstitle = form.newstitle;
                    dbForm.newscontent = form.newscontent;
                    dbForm.category = form.category;
                    dbForm.clicks = form.clicks;
                    dbForm.isslide = form.isslide;
                    dbForm.slidepicurl = form.slidepicurl;
                    dbForm.imgurl = form.imgurl;
                    dbForm.userid = form.userid;
                    dbForm.pubtime = form.pubtime;
                    dbForm.newscode = form.newscode;
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
                    db.fw_newsinfo.Remove(r => r.newsid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newnewsid()
        {
            return new SysSerialServices().GetNewSerial("fw_newsinfo");
        }

    }
}
