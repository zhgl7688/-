using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    /// <summary>
    /// 提供基于 sys_menu 的权限过滤
    /// </summary>
    public class MenuRoleAuthorizeAttribute : ActionFilterAttribute
    {
        private Diagnostics.TimingScope actionExecuting;
        private Diagnostics.TimingScope resultExecuting;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            actionExecuting.Dispose();
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            resultExecuting = new Fruit.Diagnostics.TimingScope("ResultExecuting");
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            resultExecuting.Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            actionExecuting = new Fruit.Diagnostics.TimingScope("ActionExecuting");
            using (new Fruit.Diagnostics.TimingScope())
            {
                var user = filterContext.HttpContext.User.Identity.IsAuthenticated ? filterContext.HttpContext.Session["sys_user"] as sys_user : null;
                var area = (string)filterContext.RouteData.DataTokens["Area"];
                var routeValues = filterContext.RouteData.Values;
                var controller = (string)routeValues["Controller"];
                var action = (string)routeValues["Action"];
                var pass = false;

                if (string.IsNullOrEmpty(area) && (string)routeValues["Action"] != "Index")
                {
                    return;
                }

                if (controller == "Account" && action == "Login")
                {
                    pass = true;
                }
                else if ( action.ToLower() == "edit2")
                {
                    pass = true;
                }
                else if (action.ToLower() == "edit3")
                {
                    pass = true;
                }
                else if (controller == "Account" && user != null)
                {
                    pass = true;
                }
                else if (user != null)
                {
                    if (controller == "Home" && action == "Index")
                    {
                        pass = true;
                    }
                    else if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SharedRoleAuthorizeAttribute), false).Length > 0)
                    {
                        pass = filterContext.HttpContext.User.Authorized((filterContext.ActionDescriptor.GetCustomAttributes(typeof(SharedRoleAuthorizeAttribute), false).First() as SharedRoleAuthorizeAttribute).RoleUrl);
                    }
                    else
                    {
                        pass = filterContext.HttpContext.User.Authorized(area, routeValues);
                    }
                }

                if (!pass)
                {
                    if (controller == "Home" && action == "Index")
                    {
                        filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl);
                    }
                    //else if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
                    //{
                    //    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl);
                    //}
                    else
                    {
                        filterContext.Result = NoRoleResult.Result;
                    }
                }
            }
        }

        class NoRoleResult : ActionResult
        {

            public static readonly NoRoleResult Result = new NoRoleResult();

            public override void ExecuteResult(ControllerContext context)
            {
                // MARK: 可能被正式配置的 IIS 的错误页重写功能，因此不再使用错误代码。
                // context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.Write("无权使用此功能，请重新登录！");
                context.HttpContext.Response.Write("<script>top.location.href='/Account/Login';</script>");
                context.HttpContext.Response.End();
            }
        }
    }
}