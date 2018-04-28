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
                // 提供搜索下拉框数据源
                List<ComboItem> memid, ispassed, isactive;
                using(var db = new LUOLAI1401Context())
                {
                    memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
                    isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='UpdownFrame'")).ToList();
                }
                return View(new {dataSource = new {memid,ispassed,isactive}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_auctionbuyinginfo form = null;
            List<ComboItem> memid = null, ispassed = null, isactive = null;
            using(var db = new SysContext())
            {
                ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
                isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='UpdownFrame'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
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
            return View(new { form = form, dataSource = new { memid,ispassed,isactive }});
        }

    }
    public partial class Fw_AuctionbuyinginfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_auctionbuyinginfoListModel {
            public int aucid { get; set; }
            public string ProName { get; set; }
            public decimal? primePrice { get; set; }
            public decimal? floorPrice { get; set; }
            public int? addPrice { get; set; }
            public decimal? cellingPrice { get; set; }
            public int? counts { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
            public DateTime? starttime { get; set; }
            public DateTime? endtime { get; set; }
            public int? minUserCount { get; set; }
            public int? ispassed { get; set; }
            public string ispassed_RefText { get; set; }
            public int? isactive { get; set; }
            public string isactive_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "ProName", "a.ProName", "");
            SerachCondition.Dropdown(sbCondition, "memid", "a.memid", "");
            SerachCondition.Dropdown(sbCondition, "ispassed", "a.ispassed", "");
            SerachCondition.Dropdown(sbCondition, "isactive", "a.isactive", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_auctionbuyinginfoListModel>(db.Database, "a.aucid ,a.ProName ,a.primePrice ,a.floorPrice ,a.addPrice ,a.cellingPrice ,a.counts ,b.realname memid_RefText ,a.memid ,a.starttime ,a.endtime ,a.minUserCount ,c.Text ispassed_RefText ,a.ispassed ,d.Text isactive_RefText ,a.isactive ", "fw_auctionbuyinginfo a LEFT JOIN fw_memberinfo b ON a.memid = b.memid LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.ispassed = c.Value AND (c.CodeType='ShState') LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.isactive = d.Value AND (d.CodeType='UpdownFrame') ", sbCondition.ToString(), "a.aucid", "desc");
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
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_auctionbuyinginfo.Add(form);
                }
                else
                {
                    dbForm.ProName = form.ProName;
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
                    dbForm.ispassed = form.ispassed;
                    dbForm.remark = form.remark;
                    dbForm.isactive = form.isactive;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        [System.Web.Http.HttpPost]
        public object DoJXShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','2','X'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("aucid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoJShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','2','L'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("IDS");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
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
