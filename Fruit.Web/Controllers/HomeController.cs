using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fruit.Models;

namespace Fruit.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            sys_user user = (sys_user)System.Web.HttpContext.Current.Session["sys_user"];
            ViewBag.UserName = user.UserName;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Download()
        {
            return new Exporter();
        }
    }
}