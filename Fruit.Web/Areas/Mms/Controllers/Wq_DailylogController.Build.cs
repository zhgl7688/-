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
    public partial class Wq_DailylogController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_dailyLogViewable : wq_dailyLog {
            public string UserCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_dailyLog form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_dailyLogViewable>("select b.name UserCode_RefText ,a.UserCode ,a.AttDateTime ,a.DailySummary ,a.DailyPlan ,a.DailyExper ,a.Pic1 ,a.Pic2 ,a.Pic3 ,a.Pic4 ,a.Pic5 ,a.Pic6 ,a.Pic7 ,a.UpdateDate ,a.UpdatePerson ,a.CreateDate ,a.CreatePerson ,a.CompCode ,a.Pic8 ,a.id ,a.code  from wq_dailyLog a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode  where a.code = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_dailyLog
                {
                    code = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_DailylogApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_dailyLogListModel {
            public string code { get; set; }
            public string UserCode { get; set; }
            public string UserCode_RefText { get; set; }
            public string AttDateTime { get; set; }
            public string DailySummary { get; set; }
            public string DailyPlan { get; set; }
            public string DailyExper { get; set; }
            public DateTime? UpdateDate { get; set; }
            public string UpdatePerson { get; set; }
            public DateTime? CreateDate { get; set; }
            public string CreatePerson { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}{5}{6}", "((a.UserCode IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%') AND a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super ' ))"));
            SerachCondition.PopupSelect(sbCondition, "UserCode", "a.UserCode", "");
            SerachCondition.Date(sbCondition, "AttDateTime", "a.AttDateTime", "");
            SerachCondition.Date(sbCondition, "CreateDate", "a.CreateDate", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_dailyLogListModel>(db.Database, "a.code ,b.name UserCode_RefText ,a.UserCode ,a.AttDateTime ,a.DailySummary ,a.DailyPlan ,a.DailyExper ,a.UpdateDate ,a.UpdatePerson ,a.CreateDate ,a.CreatePerson ", "wq_dailyLog a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode ", sbCondition.ToString(), "code", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_dailyLog>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_dailyLog.Find(form.code);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_dailyLog.Add(form);
                }
                else
                {
                    dbForm.UserCode = form.UserCode;
                    dbForm.AttDateTime = form.AttDateTime;
                    dbForm.DailySummary = form.DailySummary;
                    dbForm.DailyPlan = form.DailyPlan;
                    dbForm.DailyExper = form.DailyExper;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
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
                    db.wq_dailyLog.Remove(r => r.code == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newcode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_dailyLog", null));
        }

    }
}
