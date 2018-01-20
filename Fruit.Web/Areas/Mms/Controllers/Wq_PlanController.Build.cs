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
    public partial class Wq_PlanController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                // 提供搜索下拉框数据源
                List<ComboItem> Month;
                using(var db = new SysContext())
                {
                    Month = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='MON'")).ToList();
                }
                return View(new {dataSource = new {Month}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_planViewable : wq_plan {
            public string UserCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_plan form = null;
            List<ComboItem> Month = null;
            using(var db = new SysContext())
            {
                Month = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='MON'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_planViewable>("select b.name UserCode_RefText ,a.UserCode ,a.Month ,a.SellAmt ,a.IncomeAmt ,a.CompCode ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ,a.id  from wq_plan a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_plan
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { Month }});
        }

    }
    public partial class Wq_PlanApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_planListModel {
            public string id { get; set; }
            public string UserCode { get; set; }
            public string UserCode_RefText { get; set; }
            public string Month { get; set; }
            public string Month_RefText { get; set; }
            public decimal? SellAmt { get; set; }
            public decimal? IncomeAmt { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "(a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "' or '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super') AND c.CodeType='MON'"));
            SerachCondition.PopupSelect(sbCondition, "UserCode", "a.UserCode", "varchar");
            SerachCondition.Dropdown(sbCondition, "Month", "a.Month", "varchar");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_planListModel>(db.Database, "a.id ,b.name UserCode_RefText ,a.UserCode ,c.Text Month_RefText ,a.Month ,a.SellAmt ,a.IncomeAmt ", "wq_plan a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.Month = c.Value ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_plan>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_plan.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_plan.Add(form);
                }
                else
                {
                    dbForm.UserCode = form.UserCode;
                    dbForm.Month = form.Month;
                    dbForm.SellAmt = form.SellAmt;
                    dbForm.IncomeAmt = form.IncomeAmt;
                    dbForm.Remark = form.Remark;
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
                    db.wq_plan.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_plan", null));
        }

    }
}
