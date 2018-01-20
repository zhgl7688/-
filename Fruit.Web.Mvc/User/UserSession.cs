using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Fruit.Web
{
    public static class UserSession
    {
        private static object lockObj = new object();

        public static T GetSession<T>(this System.Security.Principal.IPrincipal user)
        {
            return (T)GetSession(user, "_CLS:" + typeof(T).Name, default(T));
        }

        public static void SetSession<T>(this System.Security.Principal.IPrincipal user, T value)
        {
            SetSession(user, "_CLS:" + typeof(T).Name, value);
        }

        public static object GetSession(this System.Security.Principal.IPrincipal user, string name, object defaultValue = null)
        {
            if (user.Identity.IsAuthenticated)
            {
                return GetUserSession(user.Identity.Name, name);
            }
            return defaultValue;
        }

        public static void SetSession(this System.Security.Principal.IPrincipal user, string name, object value = null)
        {
            if (user.Identity.IsAuthenticated)
            {
                SetUserSession(user.Identity.Name, name, value);
            }
        }

        public static void RemoveSessions(this System.Security.Principal.IPrincipal user)
        {
            HttpContext.Current.Session.Remove("_F_US");
        }

        public static bool Authorized(this System.Security.Principal.IPrincipal user, string area, RouteValueDictionary routeValues)
        {
            using (new Fruit.Diagnostics.TimingScope())
            {
                StringBuilder sbUrl = new StringBuilder();
                if (!string.IsNullOrEmpty(area))
                {
                    sbUrl.Append("/" + area);
                }
                sbUrl.AppendFormat("/{0}", routeValues["Controller"]);
                if (!"INDEX".Equals((string)routeValues["Action"], StringComparison.OrdinalIgnoreCase))
                {
                    sbUrl.AppendFormat("/{0}", routeValues["Action"]);
                }
                var url = sbUrl.ToString();
                return Authorized(user, url);
            }
        }

        public static bool Authorized(this System.Security.Principal.IPrincipal user, string url)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            string key = url.ToUpperInvariant();
            Dictionary<string, bool> checkRight;
            lock (lockObj)
            {
                checkRight = GetUserSession(user.Identity.Name, "CHK_RIGHT") as Dictionary<string, bool>;
                if (checkRight == null)
                {
                    checkRight = new Dictionary<string, bool>();
                }
                if (checkRight.ContainsKey(key))
                {
                    return checkRight[key];
                }
                using (var db = new SysContext())
                {
                    checkRight[key] = db.Database.SqlQuery<int>(@"select count(*) from sys_roleMenuMap where 
MenuCode in (select MenuCode from sys_menu WHERE URL = @url) and
RoleCode in (select RoleCode from sys_userRoleMap where UserCode = @userCode)", new SqlParameter("@url", key), new SqlParameter("@userCode", user.Identity.Name)).FirstOrDefault() > 0;

                    SetUserSession(user.Identity.Name, "CHK_RIGHT", checkRight);
                }
            }
            return checkRight[key];
        }

        public static void SetUserSession(string user, string name, object value)
        {
            var session = HttpContext.Current.Session["_F_US"] as Dictionary<string, object>;
            if (session == null)
            {
                session = new Dictionary<string,object>();
            }
            session[name.ToUpperInvariant()] = value;
            HttpContext.Current.Session["_F_US"] = session;
        }

        private static object GetUserSession(string user, string name)
        {
            var session = HttpContext.Current.Session["_F_US"] as Dictionary<string, object>;
            if (session == null || !session.ContainsKey(name.ToUpperInvariant()))
            {
                return null;
            }
            return session[name.ToUpperInvariant()];
        }

        public static sys_user GetUserInfo(this System.Security.Principal.IIdentity user)
        {
            return HttpContext.Current.Session["sys_user"] as sys_user;
        }
    }
}
