/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;

namespace Fruit.Web.Areas.Psi.Controllers
{
    public partial class ReceiveController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }


    }

    public partial class ReceiveApiController : ApiController
    {
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new MmsContext())
            {
                IQueryable<mms_merchants> query = db.mms_merchants.AsNoTracking();
                int total;
                query = pageReq.ApplyQuery<mms_merchants>(query, "MerchantsCode", "desc", out total);
                dynamic result = new System.Dynamic.ExpandoObject();
                result.rows = query.ToList();
                result.total = total;
                return result;
            }
        }


    }
}
