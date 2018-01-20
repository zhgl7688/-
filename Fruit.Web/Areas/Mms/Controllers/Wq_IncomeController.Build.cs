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
    public partial class Wq_IncomeController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_incomeViewable : wq_income {
            public string PopCode_RefText { get; set; }
            public string UserCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_income form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_incomeViewable>("select a.id ,b.PopName PopCode_RefText ,a.PopCode ,a.Month ,a.IncomeAmt ,c.name UserCode_RefText ,a.UserCode ,a.CompCode ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate  from wq_income a LEFT JOIN wq_termPop b ON a.PopCode = b.PopCode LEFT JOIN PersonInfo c ON a.UserCode = c.psncode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_income
                {
                    id = id,
                    Month = DateTime.Today,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode),
                    CreatePerson = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserName)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_IncomeApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_incomeListModel {
            public string id { get; set; }
            public string PopCode { get; set; }
            public string PopCode_RefText { get; set; }
            public DateTime? Month { get; set; }
            public decimal? IncomeAmt { get; set; }
            public string UserCode { get; set; }
            public string UserCode_RefText { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}{5}{6}", "((a.UserCode IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%') AND a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super ' ))"));
            SerachCondition.Date(sbCondition, "Month", "a.Month", "datetime");
            SerachCondition.PopupSelect(sbCondition, "UserCode", "a.UserCode", "varchar");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_incomeListModel>(db.Database, "a.id ,b.PopName PopCode_RefText ,a.PopCode ,a.Month ,a.IncomeAmt ,c.name UserCode_RefText ,a.UserCode ,a.Remark ", "wq_income a LEFT JOIN wq_termPop b ON a.PopCode = b.PopCode LEFT JOIN PersonInfo c ON a.UserCode = c.psncode ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_income>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_income.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_income.Add(form);
                }
                else
                {
                    dbForm.PopCode = form.PopCode;
                    dbForm.Month = form.Month;
                    dbForm.IncomeAmt = form.IncomeAmt;
                    dbForm.UserCode = form.UserCode;
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
                    db.wq_income.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0}{1}{2:D3}", "PT", DateTime.Now.ToString("yyyyMM"), new SysSerialServices().GetNewSerial("wq_income", null));
        }

    }
}
