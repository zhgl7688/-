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
    public partial class Wq_BaserptproductController : Controller
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
            wq_baseRptProduct form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_baseRptProduct.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_baseRptProduct
                {
                    RPdtCode = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_BaserptproductApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_baseRptProductListModel {
            public string RPdtCode { get; set; }
            public string RPdtName { get; set; }
            public string Description { get; set; }
            public string CreatePerson { get; set; }
            public DateTime? CreateDate { get; set; }
            public string UpdatePerson { get; set; }
            public DateTime? UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "RPdtName", "a.RPdtName", "varchar");
            SerachCondition.Date(sbCondition, "CreateDate", "a.CreateDate", "datetime");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_baseRptProductListModel>(db.Database, "a.RPdtCode ,a.RPdtName ,a.Description ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ", "wq_baseRptProduct a ", sbCondition.ToString(), "RPdtCode", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_baseRptProduct>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_baseRptProduct.Find(form.RPdtCode);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_baseRptProduct.Add(form);
                }
                else
                {
                    dbForm.RPdtName = form.RPdtName;
                    dbForm.Description = form.Description;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                db.wq_baseRptProduct.Remove(r => r.RPdtCode == id);
                db.SaveChanges();
            }
            return true;
        }
        public object NewRPdtCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_baseRptProduct", null));
        }

    }
}
