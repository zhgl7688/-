/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class Sys_UserController : Controller
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
            sys_user form = null;
            using(var db = new SysContext())
            {
                form = db.sys_user.Find(id);
            }
            if (form == null)
            {
                form = new sys_user
                {
                    UserCode = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Sys_UserApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class sys_userListModel {
            public string UserCode { get; set; }
            public string UserSeq { get; set; }
            public string UserName { get; set; }
            public string Description { get; set; }
            public string Password { get; set; }
            public string RoleName { get; set; }
            public string OrganizeName { get; set; }
            public string ConfigJSON { get; set; }
            public string IsEnable { get; set; }
            public string LoginCount { get; set; }
            public string LastLoginDate { get; set; }
            public string CreatePerson { get; set; }
            public decimal CreateDate { get; set; }
            public string UpdatePerson { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new SysContext())
            {
                return pageReq.ToPageList<sys_userListModel>(db.Database, "a.UserCode ,a.UserSeq ,a.UserName ,a.Description ,a.Password ,a.RoleName ,a.OrganizeName ,a.ConfigJSON ,a.IsEnable ,a.LoginCount ,a.LastLoginDate ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ", "sys_user a ", "", "UserCode", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<sys_user>();
            using (var db = new SysContext())
            {
                var dbForm = db.sys_user.Find(form.UserCode);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.sys_user.Add(form);
                }
                else
                {
                    dbForm.UserSeq = form.UserSeq;
                    dbForm.UserName = form.UserName;
                    dbForm.Description = form.Description;
                    dbForm.Password = form.Password;
                    dbForm.RoleName = form.RoleName;
                    dbForm.OrganizeName = form.OrganizeName;
                    dbForm.ConfigJSON = form.ConfigJSON;
                    dbForm.IsEnable = form.IsEnable;
                    dbForm.LoginCount = form.LoginCount;
                    dbForm.LastLoginDate = form.LastLoginDate;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new SysContext())
            {
                db.sys_user.Remove(r => r.UserCode == id);
                db.SaveChanges();
            }
            return true;
        }
        public object NewUserCode()
        {
            return new SysSerialServices().GetNewSerial("sys_user");
        }

    }
}
