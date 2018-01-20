using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class UserController : Controller
    {
        // GET: Sys/User
        public ActionResult Index()
        {
            return View();
        }
    }

    public class UserApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        public object Organize()
        {
            using (var db = new SysContext())
            {
                return db.sys_organize.AsNoTracking().OrderBy(r => r.OrganizeSeq).ToList();
            }
        }

        [System.Web.Http.HttpGet]
        public string[] UserOrganize(String UserCode)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<string>("select OrganizeCode from sys_userOrganizeMap where UserCode = @UserCode", new System.Data.SqlClient.SqlParameter("@UserCode", UserCode)).ToArray();
            }
        }

        [System.Web.Http.HttpPost]
        public bool UserOrganize(String UserCode, JArray data)
        {
            using (var db = new SysContext())
            {
                db.sys_userOrganizeMap.Remove(u => u.UserCode == UserCode);
                for (var i = 0; i < data.Count; i++)
                {
                    db.sys_userOrganizeMap.Add(new sys_userOrganizeMap { UserCode = UserCode, OrganizeCode = (string)data[i] });
                }
                db.SaveChanges();
                //return db.Database.SqlQuery<string>("select OrganizeCode from sys_userOrganizeMap where UserCode = @UserCode", new System.Data.SqlClient.SqlParameter("@UserCode", UserCode)).ToArray();
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        public string[] UserRole(String UserCode)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<string>("select RoleCode from sys_userRoleMap where UserCode = @UserCode", new System.Data.SqlClient.SqlParameter("@UserCode", UserCode)).ToArray();
            }
        }

        [System.Web.Http.HttpPost]
        public bool UserRole(String UserCode, JArray data)
        {
            using (var db = new SysContext())
            {
                db.sys_userRoleMap.Remove(u => u.UserCode == UserCode);
                for (var i = 0; i < data.Count; i++)
                {
                    db.sys_userRoleMap.Add(new sys_userRoleMap { UserCode = UserCode, RoleCode = (string)data[i] });
                }
                db.SaveChanges();
                //return db.Database.SqlQuery<string>("select OrganizeCode from sys_userOrganizeMap where UserCode = @UserCode", new System.Data.SqlClient.SqlParameter("@UserCode", UserCode)).ToArray();
            }
            return true;
        }

        public object Get(string id = null, string OrganizeCode = null)
        {
            PageRequest pageReq = new PageRequest();
            using (var db = new SysContext())
            {
                IQueryable<sys_user> query = db.sys_user.AsNoTracking();
                if (OrganizeCode != null)
                {
                    query = db.sys_userOrganizeMap.Include("sys_user").Where(o => o.OrganizeCode == OrganizeCode).Select(o => o.sys_user);
                }
                var rQuery = pageReq.ApplyQuery(query, "UserSeq");
                return rQuery.ToList();
            }
        }

        public bool ResetPassword(JObject data)
        {
            var userCode = (string)data["UserCode"];
            using (var db = new SysContext())
            {
                var user = db.sys_user.Find(userCode);
                if (user != null)
                {
                    user.Password = "1234";
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public object Post(JArray array)
        {
            using (var db = new SysContext())
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var row_state = (RowState)(int)array[i]["_row_state"];
                    sys_user user = array[i].ToObject<sys_user>();
                    switch (row_state)
                    {
                        case RowState.Changed:
                            var oldUser = db.sys_user.Find(user.UserCode);
                            oldUser.UserName = user.UserName;
                            oldUser.Mobile = user.Mobile;
                            oldUser.Description = user.Description;
                            oldUser.IsEnable = user.IsEnable;
                            oldUser.UpdateDate = DateTime.Now;
                            oldUser.UpdatePerson = User.Identity.GetUserInfo().UserName;
                            break;
                        case RowState.New:
                            user.CreateDate = DateTime.Now;
                            user.CreatePerson = User.Identity.GetUserInfo().UserName;
                            user.Password = "1234";
                            db.sys_user.Add(user);
                            break;
                        case RowState.Deleted:
                            db.sys_userRoleMap.Remove(r => r.UserCode == user.UserCode);
                            db.sys_userOrganizeMap.Remove(r => r.UserCode == user.UserCode);
                            db.sys_userSetting.Remove(r => r.UserCode == user.UserCode);
                            db.sys_user.Remove(r => r.UserCode == user.UserCode);
                            break;
                    }
                }
                db.SaveChanges();
            }
            return Get(null);
        }
    }
}