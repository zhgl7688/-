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

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class SubmittedController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                // 提供搜索下拉框数据源
                List<ComboItem> timingtype;
                using(var db = new SysContext())
                {
                    timingtype = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='sfkq'")).ToList();
                }
                return View(new {dataSource = new {timingtype}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class timingSetViewable : timingSet {
            public string RoleCode_RefText { get; set; }
            public string psncode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            timingSet form = null;
            List<ComboItem> timingtype = null;
            using(var db = new SysContext())
            {
                timingtype = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='sfkq'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<timingSetViewable>("select a.CompCode ,b.RoleName RoleCode_RefText ,a.RoleCode ,c.name psncode_RefText ,a.psncode ,a.timingtype ,a.interval ,a.mobegintime ,a.moendtime ,a.afbegintime ,a.afendtime ,a.id  from timingSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode LEFT JOIN PersonInfo c ON a.psncode = c.psncode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new timingSet
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { timingtype }});
        }

    }
    public partial class SubmittedApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class timingSetListModel {
            public string id { get; set; }
            public string RoleCode { get; set; }
            public string RoleCode_RefText { get; set; }
            public string psncode { get; set; }
            public string psncode_RefText { get; set; }
            public string timingtype { get; set; }
            public string timingtype_RefText { get; set; }
            public string interval { get; set; }
            public string mobegintime { get; set; }
            public string moendtime { get; set; }
            public string afbegintime { get; set; }
            public string afendtime { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));
            SerachCondition.PopupSelect(sbCondition, "RoleCode", "a.RoleCode", "varchar");
            SerachCondition.PopupSelect(sbCondition, "psncode", "a.psncode", "varchar");
            SerachCondition.Dropdown(sbCondition, "timingtype", "a.timingtype", "varchar");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<timingSetListModel>(db.Database, "a.id ,b.RoleName RoleCode_RefText ,a.RoleCode ,c.name psncode_RefText ,a.psncode ,d.Text timingtype_RefText ,a.timingtype ,a.interval ,a.mobegintime ,a.moendtime ,a.afbegintime ,a.afendtime ", "timingSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode LEFT JOIN PersonInfo c ON a.psncode = c.psncode LEFT JOIN [Bcp.Sys].dbo.sys_code d ON a.timingtype = d.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<timingSet>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.timingSet.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    db.timingSet.Add(form);
                }
                else
                {
                    dbForm.RoleCode = form.RoleCode;
                    dbForm.psncode = form.psncode;
                    dbForm.timingtype = form.timingtype;
                    dbForm.interval = form.interval;
                    dbForm.mobegintime = form.mobegintime;
                    dbForm.moendtime = form.moendtime;
                    dbForm.afbegintime = form.afbegintime;
                    dbForm.afendtime = form.afendtime;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.timingSet.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("timingSet", null));
        }

    }
}
