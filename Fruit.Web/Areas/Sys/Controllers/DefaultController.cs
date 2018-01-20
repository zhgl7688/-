using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Sys/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}