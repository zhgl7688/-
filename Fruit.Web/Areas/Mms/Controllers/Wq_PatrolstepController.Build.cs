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
    public partial class Wq_PatrolstepController : Controller
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
        public ActionResult Edit(string id)
        {
            wq_patrolStep form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_patrolStep.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_patrolStep
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_PatrolstepApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolStepListModel {
            public string id { get; set; }
            public string StepCode { get; set; }
            public string StepStatus { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));
            SerachCondition.TextBox(sbCondition, "StepCode", "a.StepCode", "varchar");
            SerachCondition.CheckBox(sbCondition, "StepStatus", "a.StepStatus", "varchar");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_patrolStepListModel>(db.Database, "a.id ,a.StepCode ,a.StepStatus ", "wq_patrolStep a ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_patrolStep>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_patrolStep.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    db.wq_patrolStep.Add(form);
                }
                else
                {
                    dbForm.StepCode = form.StepCode;
                    dbForm.StepStatus = form.StepStatus;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                db.wq_patrolStep.Remove(r => r.id == id);
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_patrolStep", null));
        }

    }
}
