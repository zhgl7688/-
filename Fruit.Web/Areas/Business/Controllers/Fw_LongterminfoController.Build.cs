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
                // 提供搜索下拉框数据源
                List<ComboItem> memid, mode, isactive, ispassed;
                using(var db = new LUOLAI1401Context())
                {
                    memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    mode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                    isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                    ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
                }
                return View(new {dataSource = new {memid,mode,isactive,ispassed}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_longterminfo form = null;
            List<ComboItem> mode = null, memid = null, isactive = null, ispassed = null;
            using(var db = new SysContext())
            {
                mode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ShState'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
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
            return View(new { form = form, dataSource = new { mode,memid,isactive,ispassed }});
        }

    }
    public partial class Fw_LongterminfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_longterminfoListModel {
            public int longid { get; set; }
            public string longname { get; set; }
            public string mode { get; set; }
            public string mode_RefText { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
            public int? isactive { get; set; }
            public string isactive_RefText { get; set; }
            public int? ispassed { get; set; }
            public string ispassed_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "longname", "a.longname", "");
            SerachCondition.Dropdown(sbCondition, "mode", "a.mode", "");
            SerachCondition.Dropdown(sbCondition, "memid", "a.memid", "");
            SerachCondition.Dropdown(sbCondition, "isactive", "a.isactive", "");
            SerachCondition.Dropdown(sbCondition, "ispassed", "a.ispassed", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_longterminfoListModel>(db.Database, "a.longid ,a.longname ,b.Text mode_RefText ,a.mode ,c.realname memid_RefText ,a.memid ,d.Text isactive_RefText ,a.isactive ,e.Text ispassed_RefText ,a.ispassed ", "fw_longterminfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.mode = b.Value AND (b.CodeType='TradingMode') LEFT JOIN fw_memberinfo c ON a.memid = c.memid LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.isactive = d.Value AND (d.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.ispassed = e.Value AND (e.CodeType='ShState') ", sbCondition.ToString(), "a.longid", "desc");
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
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_longterminfo.Add(form);
                }
                else
                {
                    dbForm.longname = form.longname;
                    dbForm.mode = form.mode;
                    dbForm.memid = form.memid;
                    dbForm.url = form.url;
                    dbForm.isactive = form.isactive;
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
        public object DoCXShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'4'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("longid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoCShenhe(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'4'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("IDS");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteNonQuery();
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
