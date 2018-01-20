using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web
{
    /// <summary>
    /// 提供在 ASP.NET MVC 应用程序内注册一个或多个区域的方式。包括自动注册 Api 控制器选项
    /// </summary>
    public abstract class BaseAreaRegistration : AreaRegistration
    {

        protected virtual bool NeedConfigurationApiRoute { get { return true; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { controller="Default", action = "Index", id = UrlParameter.Optional }
            );

            if (NeedConfigurationApiRoute)
            {
                GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                    this.AreaName + "Api",
                    "api/" + this.AreaName + "/{controller}/{action}/{id}",
                    new { area = this.AreaName, controller = "default", action = RouteParameter.Optional, id = RouteParameter.Optional, namespaceName = new string[] { string.Format("Fruit.Web.Areas.{0}.Controllers", this.AreaName) } },
                    new { action = new ApiActionConstraint() }
                );
            }
        }
    }
}