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
    public partial class Fw_ProductinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> catid, memid, sellmode, isactive, issale, ispassed;
                using(var db = new LUOLAI1401Context())
                {
                    catid = db.Database.SqlQuery<ComboItem>("select catname Text, cast(catid as varchar(10)) Value from fw_categoryinfo where " + string.Format("{0}", "/*TABLEALIAS*/parentid !='0'")).ToList();
                    memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    sellmode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                    isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                    issale = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='forseek'")).ToList();
                    ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
                }
                return View(new {dataSource = new {catid,memid,sellmode,isactive,issale,ispassed}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_productinfo form = null;
            List<ComboItem> catid = null, spec = null, memid = null, sellmode = null, isactive = null, issale = null, ispassed = null;
            using(var db = new SysContext())
            {
                spec = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='MeasureUnit'")).ToList();
                sellmode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                issale = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='forseek'")).ToList();
                ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                catid = db.Database.SqlQuery<ComboItem>("select catname Text, cast(catid as varchar(10)) Value from fw_categoryinfo where " + string.Format("{0}", "/*TABLEALIAS*/parentid !='0'")).ToList();
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                form = db.fw_productinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_productinfo
                {
                    Fid = id
                };
            }
            return View(new { form = form, dataSource = new { catid,spec,memid,sellmode,isactive,issale,ispassed }});
        }

    }
    public partial class Fw_ProductinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_productinfoListModel {
            public int Fid { get; set; }
            public int? catid { get; set; }
            public string catid_RefText { get; set; }
            public string proname { get; set; }
            public string spec { get; set; }
            public string spec_RefText { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
            public decimal? price { get; set; }
            public string sellmode { get; set; }
            public string sellmode_RefText { get; set; }
            public string grade { get; set; }
            public int? minprodcount { get; set; }
            public int? stock { get; set; }
            public int? isactive { get; set; }
            public string isactive_RefText { get; set; }
            public decimal? depositseller { get; set; }
            public decimal? depositbuyer { get; set; }
            public int? issale { get; set; }
            public string issale_RefText { get; set; }
            public int? viewcount { get; set; }
            public DateTime? CreateDate { get; set; }
            public int? ispassed { get; set; }
            public string ispassed_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "catid", "a.catid", "");
            SerachCondition.TextBox(sbCondition, "proname", "a.proname", "");
            SerachCondition.Dropdown(sbCondition, "memid", "a.memid", "");
            SerachCondition.Dropdown(sbCondition, "sellmode", "a.sellmode", "");
            SerachCondition.Dropdown(sbCondition, "isactive", "a.isactive", "");
            SerachCondition.Dropdown(sbCondition, "issale", "a.issale", "");
            SerachCondition.Date(sbCondition, "CreateDate", "a.CreateDate", "");
            SerachCondition.Dropdown(sbCondition, "ispassed", "a.ispassed", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_productinfoListModel>(db.Database, "a.Fid ,b.catname catid_RefText ,a.catid ,a.proname ,c.Text spec_RefText ,a.spec ,d.realname memid_RefText ,a.memid ,a.price ,e.Text sellmode_RefText ,a.sellmode ,a.grade ,a.minprodcount ,a.stock ,f.Text isactive_RefText ,a.isactive ,a.depositseller ,a.depositbuyer ,g.Text issale_RefText ,a.issale ,a.viewcount ,a.CreateDate ,h.Text ispassed_RefText ,a.ispassed ", "fw_productinfo a LEFT JOIN fw_categoryinfo b ON a.catid = b.catid AND (b.parentid !='0') LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.spec = c.Value AND (c.CodeType='MeasureUnit') LEFT JOIN fw_memberinfo d ON a.memid = d.memid LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.sellmode = e.Value AND (e.CodeType='TradingMode') LEFT JOIN [SYS_YLW].dbo.sys_code f ON a.isactive = f.Value AND (f.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code g ON a.issale = g.Value AND (g.CodeType='forseek') LEFT JOIN [SYS_YLW].dbo.sys_code h ON a.ispassed = h.Value AND (h.CodeType='ShState') ", sbCondition.ToString(), "a.Fid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_productinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_productinfo.Find(form.Fid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_productinfo.Add(form);
                }
                else
                {
                    dbForm.catid = form.catid;
                    dbForm.proname = form.proname;
                    dbForm.spec = form.spec;
                    dbForm.memid = form.memid;
                    dbForm.price = form.price;
                    dbForm.sellmode = form.sellmode;
                    dbForm.grade = form.grade;
                    dbForm.minprodcount = form.minprodcount;
                    dbForm.stock = form.stock;
                    dbForm.isactive = form.isactive;
                    dbForm.depositseller = form.depositseller;
                    dbForm.depositbuyer = form.depositbuyer;
                    dbForm.issale = form.issale;
                    dbForm.viewcount = form.viewcount;
                    dbForm.remark = form.remark;
                    dbForm.ispassed = form.ispassed;
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
        public object DoGXshenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','1','X'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Fid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoGshenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','1','L'";
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
                    db.fw_productinfo.Remove(r => r.Fid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFid()
        {
            return new SysSerialServices().GetNewSerial("fw_productinfo");
        }

    }
}
