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
    public partial class Base_ProductbrandinfoController : Controller
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
            base_ProductBrandInfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.base_ProductBrandInfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new base_ProductBrandInfo
                {
                    BrandCode = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Base_ProductbrandinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class base_ProductBrandInfoListModel {
            public string BrandCode { get; set; }
            public string BrandName { get; set; }
            public string DelFlag { get; set; }
            public string CreatePerson { get; set; }
            public DateTime? CreateDate { get; set; }
            public string UpdatePerson { get; set; }
            public DateTime? UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));
            SerachCondition.TextBox(sbCondition, "BrandName", "a.BrandName", "varchar");
            SerachCondition.Date(sbCondition, "CreateDate", "a.CreateDate", "datetime");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<base_ProductBrandInfoListModel>(db.Database, "a.BrandCode ,a.BrandName ,a.DelFlag ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ", "base_ProductBrandInfo a ", sbCondition.ToString(), "BrandCode", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<base_ProductBrandInfo>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.base_ProductBrandInfo.Find(form.BrandCode);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.base_ProductBrandInfo.Add(form);
                }
                else
                {
                    dbForm.BrandName = form.BrandName;
                    dbForm.DelFlag = form.DelFlag;
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
                db.base_ProductBrandInfo.Remove(r => r.BrandCode == id);
                db.SaveChanges();
            }
            return true;
        }
        public object NewBrandCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("base_ProductBrandInfo", null));
        }

    }
}
