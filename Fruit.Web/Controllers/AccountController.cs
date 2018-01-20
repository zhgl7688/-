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



        
      
        
        


        [HttpGet]
        public JavaScriptResult UserInfo()
        {
            return new JavaScriptResult() { Script = "var userInfo=" + Newtonsoft.Json.JsonConvert.SerializeObject(System.Web.HttpContext.Current.Session["sys_user"], Newtonsoft.Json.Formatting.None) + ";" };
        }
       

    }
}