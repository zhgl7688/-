using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class LogController : Controller
    {
        // GET: Sys/Log
        public ActionResult Index()
        {
            return View();
        }
    }

    public class LogApiController: ApiController
    {


        [System.Web.Http.HttpGet]
        public object Get()
        {
            using (var db = new SysContext())
            {
                var req = new PageRequest();
                var date = HttpContext.Current.Request.QueryString["date"];

                return req.ToPageList(db.sys_log.AsNoTracking().DateRange("Date", date), "ID", "desc");
            }
        }

        [System.Web.Http.HttpGet]
        public object Login()
        {
            using (var db = new SysContext())
            {
                var req = new PageRequest();
                var date = HttpContext.Current.Request.QueryString["date"];

                return req.ToPageList(db.sys_loginHistory.AsNoTracking().DateRange("LoginDate", date), "ID", "desc");
            }
        }
    }
}