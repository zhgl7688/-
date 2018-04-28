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
                // 提供搜索下拉框数据源
                List<ComboItem> userid;
                using(var db = new SysContext())
                {
                    userid = db.sys_user.Select(i=>new ComboItem{ Text = i.UserName, Value = "" +  i.UserCode }).ToList();
                }
                return View(new {dataSource = new {userid}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_newsinfo form = null;
            List<ComboItem> isslide = null, userid = null;
            using(var db = new SysContext())
            {
                isslide = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                userid = db.sys_user.Select(i=>new ComboItem{ Text = i.UserName, Value = "" + i.UserCode }).ToList();
            }
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
                    newsid = id,
                    userid = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode)
                };
            }
            return View(new { form = form, dataSource = new { isslide,userid }});
        }

    }
    public partial class Fw_NewsinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_newsinfoListModel {
            public int newsid { get; set; }
            public string newstitle { get; set; }
            public string category { get; set; }
            public int? clicks { get; set; }
            public int? isslide { get; set; }
            public string isslide_RefText { get; set; }
            public string userid { get; set; }
            public string userid_RefText { get; set; }
            public DateTime? pubtime { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "newstitle", "a.newstitle", "");
            SerachCondition.Dropdown(sbCondition, "userid", "a.userid", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_newsinfoListModel>(db.Database, "a.newsid ,a.newstitle ,a.category ,a.clicks ,b.Text isslide_RefText ,a.isslide ,c.UserName userid_RefText ,a.userid ,a.pubtime ", "fw_newsinfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.isslide = b.Value AND (b.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_user c ON a.userid = c.UserCode ", sbCondition.ToString(), "a.newsid", "desc");
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
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_newsinfo.Add(form);
                }
                else
                {
                    dbForm.newstitle = form.newstitle;
                    dbForm.category = form.category;
                    dbForm.clicks = form.clicks;
                    dbForm.isslide = form.isslide;
                    dbForm.slidepicurl = form.slidepicurl;
                    dbForm.imgurl = form.imgurl;
                    dbForm.userid = form.userid;
                    dbForm.pubtime = form.pubtime;
                    dbForm.newscontenth = form.newscontenth;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "UPDATE fw_newsinfo SET newscontent=REPLACE(newscontenth,'/upload/','/admin/upload/') WHERE newsid=@newsid";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@newsid";
                p1.Value = form.newsid;
                cmd.Parameters.Add(p1);
                cmd.ExecuteNonQuery();
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
