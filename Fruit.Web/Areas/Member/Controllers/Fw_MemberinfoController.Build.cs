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

namespace Fruit.Web.Areas.Member.Controllers
{
    public partial class Fw_MemberinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> ispassed;
                using(var db = new SysContext())
                {
                    ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='RZZT'")).ToList();
                }
                return View(new {dataSource = new {ispassed}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            fw_memberinfo form = null;
            List<ComboItem> gender = null, ispassed = null, isenabled = null;
            using(var db = new SysContext())
            {
                gender = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Gender'")).ToList();
                ispassed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='RZZT'")).ToList();
                isenabled = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_memberinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_memberinfo
                {
                    Fid = id
                };
            }
            return View(new { form = form, dataSource = new { gender,ispassed,isenabled }});
        }

    }
    public partial class Fw_MemberinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_memberinfoListModel {
            public string Fid { get; set; }
            public string memid { get; set; }
            public string realname { get; set; }
            public int? gender { get; set; }
            public string gender_RefText { get; set; }
            public string email { get; set; }
            public string qq { get; set; }
            public string wechat { get; set; }
            public DateTime? regtime { get; set; }
            public DateTime? lasttime { get; set; }
            public int? ispassed { get; set; }
            public string ispassed_RefText { get; set; }
            public int? isenabled { get; set; }
            public string isenabled_RefText { get; set; }
            public int? Cimg1 { get; set; }
            public int? Cimg2 { get; set; }
            public int? Cimg3 { get; set; }
            public int? Cimgcode1 { get; set; }
            public int? Cimgcode2 { get; set; }
            public int? Xletterurl { get; set; }
            public int? Cletterurl { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "memid", "a.memid", "");
            SerachCondition.TextBox(sbCondition, "realname", "a.realname", "");
            SerachCondition.TextBox(sbCondition, "qq", "a.qq", "");
            SerachCondition.TextBox(sbCondition, "wechat", "a.wechat", "");
            SerachCondition.Dropdown(sbCondition, "ispassed", "a.ispassed", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_memberinfoListModel>(db.Database, "a.Fid ,a.memid ,a.realname ,b.Text gender_RefText ,a.gender ,a.email ,a.qq ,a.wechat ,a.regtime ,a.lasttime ,c.Text ispassed_RefText ,a.ispassed ,d.Text isenabled_RefText ,a.isenabled ,a.Cimg1 ,a.Cimg2 ,a.Cimg3 ,a.Cimgcode1 ,a.Cimgcode2 ,a.Xletterurl ,a.Cletterurl ", "fw_memberinfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.gender = b.Value AND (b.CodeType='Gender') LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.ispassed = c.Value AND (c.CodeType='RZZT') LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.isenabled = d.Value AND (d.CodeType='YN') ", sbCondition.ToString(), "a.regtime", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_memberinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_memberinfo.Find(form.Fid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_memberinfo.Add(form);
                }
                else
                {
                    dbForm.memid = form.memid;
                    dbForm.realname = form.realname;
                    dbForm.gender = form.gender;
                    dbForm.email = form.email;
                    dbForm.qq = form.qq;
                    dbForm.wechat = form.wechat;
                    dbForm.regtime = form.regtime;
                    dbForm.lasttime = form.lasttime;
                    dbForm.ispassed = form.ispassed;
                    dbForm.isenabled = form.isenabled;
                    dbForm.Cimg1 = form.Cimg1;
                    dbForm.Cimg2 = form.Cimg2;
                    dbForm.Cimg3 = form.Cimg3;
                    dbForm.Cimgcode1 = form.Cimgcode1;
                    dbForm.Cimgcode2 = form.Cimgcode2;
                    dbForm.Xletterurl = form.Xletterurl;
                    dbForm.Cletterurl = form.Cletterurl;
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
        public object DoBVIP(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','4','L'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Fid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoBVIPX(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','4','X'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Fid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoBPQX(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','7','L'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("IDS");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoBQXRZ(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','7','X'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Fid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public object DoBXH(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine @FID,'" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','6','L'";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@FID";
                p1.Value = (string)row.GetValue("Fid");
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
                    db.fw_memberinfo.Remove(r => r.Fid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFid()
        {
            return new SysSerialServices().GetNewSerial("fw_memberinfo");
        }

    }
}
