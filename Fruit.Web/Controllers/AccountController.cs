using Fruit.Models;
using Fruit.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fruit.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var userService = new SysUserService();
            sys_user user = userService.Login(model.Username, model.Password);
            if (user != null)
            {
                System.Web.HttpContext.Current.Session.Clear();
                // 登录成功
                FormsAuthentication.SetAuthCookie(user.UserCode, true);
                // 缓存用户信息
                System.Web.HttpContext.Current.Session["sys_user"] = user;

                userService.OnLogined(user);

                return Json(new { success = true, url = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult ModifyPassword(ModifyPasswordModel model)
        {
            string msg = "";bool success;
            using (var db = new SysContext())
            {
                 var oldUser = db.sys_user.Find(model.UserCode);
               if (oldUser!=null&& model.oldPassword== oldUser.Password)
                {
                    oldUser.Password = model.newPassword;
                    db.SaveChanges();
                    msg = "修改成功";
                    success = true;
                }
                else
                {
                    success = false;
                    msg = "原密码不正确";
                }
                        
                
            }
            return Json(new {   success, msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JavaScriptResult UserInfo()
        {
            return new JavaScriptResult() { Script = "var userInfo=" + Newtonsoft.Json.JsonConvert.SerializeObject(System.Web.HttpContext.Current.Session["sys_user"], Newtonsoft.Json.Formatting.None) + ";" };
        }


        public void remindGreen()
        { 
            using (var db = new LUOLAI1401Context())
            { 
                //var getdate = DateTime.Today.AddYears(1);
                var remindGreen = db.fw_productinfo.Where(a => a.ispassed == 0).
                Join(db.fw_memberinfo, a => a.memid, b => b.memid, (a, b) => new { a.Fid, a.CreateDate, a.CreatePerson, a.memid, b.realname }).ToList();
                Response.Write(JsonConvert.SerializeObject(remindGreen));
                Response.End();
            }
        }

        public void remindRed()
        {
            using (var db = new LUOLAI1401Context())
            {
                //var getdate = DateTime.Today.AddYears(1);
                var remindGreen = db.fw_auctionbuyinginfo.Where(a => a.ispassed == 0).
                Join(db.fw_memberinfo, a => a.memid, b => b.memid, (a, b) => new { a.aucid, a.CreateDate, a.CreatePerson, b.realname }).ToList();
                Response.Write(JsonConvert.SerializeObject(remindGreen));
                Response.End();
            }
        }

        public void remindYellow()
        {
            using (var db = new LUOLAI1401Context())
            {
                //var getdate = DateTime.Today.AddYears(1);
                var remindGreen = db.fw_teambuying.Where(a => a.ispassed == 0).
                Join(db.fw_memberinfo, a => a.memid, b => b.memid, (a, b) => new { a.procode, a.CreateDate, a.CreatePerson, b.realname }).ToList();
                Response.Write(JsonConvert.SerializeObject(remindGreen));
                Response.End();
            }
        }

        public void remindBlue()
        {
            using (var db = new LUOLAI1401Context())
            {
                //var getdate = DateTime.Today.AddYears(1);
                var remindGreen = db.fw_memberinfo.Where(a => a.ispassed == 0 && a.Cimg1 != null && a.Cimg2 != null && a.Cimg3 != null && a.Cimgcode1 != null && a.Cimgcode2 != null && a.Cletterurl != null).ToList();
                //Join(db.fw_memberinfo, a => a.memid, b => b.memid, (a, b) => new { a.longid, a.CreateDate, a.CreatePerson, b.realname }).;
                Response.Write(JsonConvert.SerializeObject(remindGreen));
                Response.End();
            }
        }


        

            

            





    }
}