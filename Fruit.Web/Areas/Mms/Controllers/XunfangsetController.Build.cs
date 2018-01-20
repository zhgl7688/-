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
    public partial class XunfangsetController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolSetViewable : wq_patrolSet {
            public string RoleCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_patrolSet form = null;
            List<ComboItem> SignInMode = null, SignOutMode = null, PhotoMarkType = null, PhotoRatio = null;
            using(var db = new SysContext())
            {
                SignInMode = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where CodeType='sfkq'").ToList();
                SignOutMode = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where CodeType='sfkq'").ToList();
                PhotoMarkType = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where CodeType='sfkq'").ToList();
                PhotoRatio = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where CodeType='zpfbl'").ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_patrolSetViewable>("select a.CompCode ,b.RoleName RoleCode_RefText ,a.RoleCode ,a.SignInMode ,a.SignOutMode ,a.SignInRange ,a.SignOutRange ,a.PhotoMarkType ,a.PhotoRatio ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ,a.id  from wq_patrolSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_patrolSet
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { SignInMode,SignOutMode,PhotoMarkType,PhotoRatio }});
        }

    }
    public partial class XunfangsetApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolSetListModel {
            public string id { get; set; }
            public string RoleCode { get; set; }
            public string RoleCode_RefText { get; set; }
            public string SignInMode { get; set; }
            public string SignInMode_RefText { get; set; }
            public string SignOutMode { get; set; }
            public string SignOutMode_RefText { get; set; }
            public int? SignInRange { get; set; }
            public int? SignOutRange { get; set; }
            public string PhotoMarkType { get; set; }
            public string PhotoMarkType_RefText { get; set; }
            public string PhotoRatio { get; set; }
            public string PhotoRatio_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_patrolSetListModel>(db.Database, "a.id ,b.RoleName RoleCode_RefText ,a.RoleCode ,c.Text SignInMode_RefText ,a.SignInMode ,d.Text SignOutMode_RefText ,a.SignOutMode ,a.SignInRange ,a.SignOutRange ,e.Text PhotoMarkType_RefText ,a.PhotoMarkType ,f.Text PhotoRatio_RefText ,a.PhotoRatio ", "wq_patrolSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.SignInMode = c.Text LEFT JOIN [Bcp.Sys].dbo.sys_code d ON a.SignOutMode = d.Text LEFT JOIN [Bcp.Sys].dbo.sys_code e ON a.PhotoMarkType = e.Text LEFT JOIN [Bcp.Sys].dbo.sys_code f ON a.PhotoRatio = f.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_patrolSet>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_patrolSet.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_patrolSet.Add(form);
                }
                else
                {
                    dbForm.RoleCode = form.RoleCode;
                    dbForm.SignInMode = form.SignInMode;
                    dbForm.SignOutMode = form.SignOutMode;
                    dbForm.SignInRange = form.SignInRange;
                    dbForm.SignOutRange = form.SignOutRange;
                    dbForm.PhotoMarkType = form.PhotoMarkType;
                    dbForm.PhotoRatio = form.PhotoRatio;
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
                db.wq_patrolSet.Remove(r => r.id == id);
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return new SysSerialServices().GetNewSerial("wq_patrolSet");
        }

    }
}
