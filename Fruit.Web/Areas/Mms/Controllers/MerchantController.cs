using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public class MerchantController : Controller
    {
        // GET: Mms/Merchant
        public ActionResult Index()
        {
            return View();
        }
    }

    public class MerchantApiController : ApiController
    {
        public object GetNames()
        {
            string q = HttpContext.Current.Request.QueryString["q"];
            using(var db = new MmsContext()) {
                IQueryable<mms_merchants> names = db.mms_merchants.OrderBy(m => m.MerchantsName);
                if (!string.IsNullOrEmpty(q))
                {
                    names = names.Where(m => m.MerchantsName.StartsWith(q));
                }
                return names.Take(10).Select(m => m.MerchantsName).ToList();
            }
        }
    }
}