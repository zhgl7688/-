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
    public partial class Wq_PatrolplanController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolPlanViewable : wq_patrolPlan {
            public string DealerCode_RefText { get; set; }
            public string UserCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_patrolPlan form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_patrolPlanViewable>("select b.PopName DealerCode_RefText ,a.DealerCode ,a.PlanDate ,c.name UserCode_RefText ,a.UserCode ,a.Purpose ,a.CompCode ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.id ,a.UpdateDate  from wq_patrolPlan a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode LEFT JOIN PersonInfo c ON a.UserCode = c.psncode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_patrolPlan
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_PatrolplanApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolPlanListModel {
            public string id { get; set; }
            public string DealerCode { get; set; }
            public string DealerCode_RefText { get; set; }
            public DateTime? PlanDate { get; set; }
            public string UserCode { get; set; }
            public string UserCode_RefText { get; set; }
            public string Purpose { get; set; }
            public string CreatePerson { get; set; }
            public DateTime? CreateDate { get; set; }
            public string UpdatePerson { get; set; }
            public DateTime? UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}{5}{6}", "((a.UserCode IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%') AND a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super ' ))"));
            SerachCondition.PopupSelect(sbCondition, "DealerCode", "a.DealerCode", "");
            SerachCondition.Date(sbCondition, "PlanDate", "a.PlanDate", "");
            SerachCondition.PopupSelect(sbCondition, "UserCode", "a.UserCode", "");
            SerachCondition.Date(sbCondition, "CreateDate", "a.CreateDate", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_patrolPlanListModel>(db.Database, "a.id ,b.PopName DealerCode_RefText ,a.DealerCode ,a.PlanDate ,c.name UserCode_RefText ,a.UserCode ,a.Purpose ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ", "wq_patrolPlan a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode LEFT JOIN PersonInfo c ON a.UserCode = c.psncode ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_patrolPlan>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_patrolPlan.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_patrolPlan.Add(form);
                }
                else
                {
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.PlanDate = form.PlanDate;
                    dbForm.UserCode = form.UserCode;
                    dbForm.Purpose = form.Purpose;
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
                    db.wq_patrolPlan.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_patrolPlan", null));
        }

    }
}
