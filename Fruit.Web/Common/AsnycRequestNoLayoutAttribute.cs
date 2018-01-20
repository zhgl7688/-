using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    /// <summary>
    /// 全局过滤器，用于对异步请求去除布局使用
    /// </summary>
    public class AsnycRequestNoLayoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result != null && IsNeedRemoveLayout(filterContext))
            {
                result.MasterName = @"~/Views/Shared/_EmptyLayout.cshtml";
            }
        }

        private bool IsNeedRemoveLayout(ActionExecutedContext filterContext)
        {
            return filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() || filterContext.RequestContext.HttpContext.Request["NOLAYOUT"] == "1";
        }
    }
}