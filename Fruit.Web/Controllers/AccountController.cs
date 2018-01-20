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



        
        public void remindGreen()
        {

            using (var db = new LUOLAI1401Context())
            { 
                var getdate = DateTime.Today.AddDays(db.BD_SysParas.FirstOrDefault().certEx); 
                var remindGreen = db.HR_PractiseCerts.Where(a => a.expireDate.CompareTo(getdate) < 0).
                    Join(db.HR_Employees, a => a.CustID, b => b.CustID, (a, b) => new { a.FID, a.CreateDate, a.CreatePerson, a.CustID, a.Remark, a.Scan1, a.Scan2, a.Scan3, a.Scan4, a.UpdateDate, a.bUsed, a.certDate, a.certNo, a.certOrgan, a.certType, a.empID, a.expireDate, a.expireDate2, a.onProject, a.regNo, a.trainStatus, a.trainStatus2, b.FName }).ToList();
                Response.Write(JsonConvert.SerializeObject(remindGreen));
                Response.End();
            }
        }
        
        public void remindRed()
        { 
            using (var db = new LUOLAI1401Context())
            { 
                var getdate = DateTime.Today.AddDays(db.BD_SysParas.FirstOrDefault().bidEx);
                var remindRed = db.PM_ProjectInfoBids.ToList().Where(a => Convert.ToDateTime(a.bidDate).CompareTo(getdate) < 0);
                Response.Write(JsonConvert.SerializeObject(remindRed));
                Response.End();
            }
        }


        [HttpGet]
        public JavaScriptResult UserInfo()
        {
            return new JavaScriptResult() { Script = "var userInfo=" + Newtonsoft.Json.JsonConvert.SerializeObject(System.Web.HttpContext.Current.Session["sys_user"], Newtonsoft.Json.Formatting.None) + ";" };
        }
        //根据单位选择关联合同
        public string PIDSelect(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                var results = db.PM_Contracts.Where(s => s.CustID == id).Select(j => new { value = j.Code, Text = j.Code });

                return results.ToJson();
            }
        }

    }
}