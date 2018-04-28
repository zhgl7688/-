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
                // 提供搜索下拉框数据源
                List<ComboItem> memid, isactive;
                using(var db = new LUOLAI1401Context())
                {
                    memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                }
                return View(new {dataSource = new {memid,isactive}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            fw_teambuying form = null;
            List<ComboItem> memid = null, isactive = null, ispassed = null, cautionFlay = null, ispayback = null, dealFlag = null, isSuccess = null;
            using(var db = new SysContext())
            {
                isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                cautionFlay = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                ispayback = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                dealFlag = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                isSuccess = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                form = db.fw_teambuying.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_teambuying
                {
                    procode = id
                };
            }
            return View(new { form = form, dataSource = new { memid,isactive,ispassed,cautionFlay,ispayback,dealFlag,isSuccess }});
        }

    }
    public partial class Fw_TeambuyinginfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_teambuyingListModel {
            public string procode { get; set; }
            public int? catid { get; set; }
            public string proname { get; set; }
            public string spec { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
            public string sellmode { get; set; }
            public decimal? price { get; set; }
            public decimal? GroupPrice { get; set; }
            public string grade { get; set; }
            public int? minprodcount { get; set; }
            public int? stock { get; set; }
            public int? isactive { get; set; }
            public string isactive_RefText { get; set; }
            public decimal? depositbuyer { get; set; }
            public decimal? cautionAmt { get; set; }
            public DateTime? endtime { get; set; }
            public DateTime? starttime { get; set; }
            public int? ispassed { get; set; }
            public string ispassed_RefText { get; set; }
            public string cautionFlay { get; set; }
            public string cautionFlay_RefText { get; set; }
            public int? ispayback { get; set; }
            public string ispayback_RefText { get; set; }
            public int dealFlag { get; set; }
            public string dealFlag_RefText { get; set; }
            public string dealTimes { get; set; }
            public int isSuccess { get; set; }
            public string isSuccess_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "proname", "a.proname", "");
            SerachCondition.Dropdown(sbCondition, "memid", "a.memid", "");
            SerachCondition.Dropdown(sbCondition, "isactive", "a.isactive", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_teambuyingListModel>(db.Database, "a.procode ,a.catid ,a.proname ,a.spec ,b.realname memid_RefText ,a.memid ,a.sellmode ,a.price ,a.GroupPrice ,a.grade ,a.minprodcount ,a.stock ,c.Text isactive_RefText ,a.isactive ,a.depositbuyer ,a.cautionAmt ,a.endtime ,a.starttime ,d.Text ispassed_RefText ,a.ispassed ,e.Text cautionFlay_RefText ,a.cautionFlay ,f.Text ispayback_RefText ,a.ispayback ,g.Text dealFlag_RefText ,a.dealFlag ,a.dealTimes ,h.Text isSuccess_RefText ,a.isSuccess ", "fw_teambuying a LEFT JOIN fw_memberinfo b ON a.memid = b.memid LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.isactive = c.Value AND (c.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.ispassed = d.Value AND (d.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.cautionFlay = e.Value AND (e.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code f ON a.ispayback = f.Value AND (f.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code g ON a.dealFlag = g.Value AND (g.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code h ON a.isSuccess = h.Value AND (h.CodeType='YN') ", sbCondition.ToString(), "a.procode", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_teambuying>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_teambuying.Find(form.procode);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_teambuying.Add(form);
                }
                else
                {
                    dbForm.catid = form.catid;
                    dbForm.proname = form.proname;
                    dbForm.spec = form.spec;
                    dbForm.memid = form.memid;
                    dbForm.sellmode = form.sellmode;
                    dbForm.price = form.price;
                    dbForm.GroupPrice = form.GroupPrice;
                    dbForm.grade = form.grade;
                    dbForm.minprodcount = form.minprodcount;
                    dbForm.stock = form.stock;
                    dbForm.isactive = form.isactive;
                    dbForm.depositbuyer = form.depositbuyer;
                    dbForm.cautionAmt = form.cautionAmt;
                    dbForm.viewcount = form.viewcount;
                    dbForm.endtime = form.endtime;
                    dbForm.starttime = form.starttime;
                    dbForm.ispassed = form.ispassed;
                    dbForm.cautionFlay = form.cautionFlay;
                    dbForm.ispayback = form.ispayback;
                    dbForm.dealFlag = form.dealFlag;
                    dbForm.dealTimes = form.dealTimes;
                    dbForm.isSuccess = form.isSuccess;
                    dbForm.remark = form.remark;
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
        public object DoTXShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','3','X'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("procode");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoTShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','3','L'";
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
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.fw_teambuying.Remove(r => r.procode == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newprocode()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("fw_teambuying", null));
        }

    }
}
