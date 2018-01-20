using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    /// <summary>
    /// 全局应用访问对象
    /// </summary>
    public static class WebApp
    {
        private static readonly object lockObj = new object();
        private const string SYSKEYPREIX = "__SYS.";

        /// <summary>
        /// 获取当前登录用户代码
        /// </summary>
        public static string CurrentUserCode
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public static sys_user CurrentUser
        {
            get
            {
                string userCode = CurrentUserCode;
                if (string.IsNullOrEmpty(userCode))
                    return null;

                sys_user user = System.Web.HttpContext.Current.Session[SYSKEYPREIX + "CurrentUser"] as sys_user;
                if (user != null && user.UserCode == userCode)
                    return user;
                // TODO
                //System.Web.HttpContext.Current.Session[SYSKEYPREIX + "CurrentUser"] = user = UserService.Find(userCode);
                return user;
            }
        }

        private static dynamic mParameters;
        public static dynamic Parameters
        {
            get
            {
                if (mParameters == null)
                {
                    lock (lockObj)
                    {
                        if (mParameters == null)
                        {
                            mParameters = new ExpandoObject();
                            using (var db = new SysContext())
                            {
                                db.sys_parameter.ToList().ForEach(p =>
                                {
                                    ((IDictionary<string, object>)mParameters)[p.ParamCode] = p.ParamValue;
                                });
                            }
                        }
                    }
                }
                return mParameters;
            }
        }
    }
}
