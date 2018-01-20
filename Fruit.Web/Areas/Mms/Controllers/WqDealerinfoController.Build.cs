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
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class WqDealerinfoController : Controller
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
        public ActionResult Edit(string id)
        {
            wq_dealerInfo form = null;
            if (form == null)
            {
                form = new wq_dealerInfo{ CompCode = id };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class WqDealerinfoApiController : ApiController
    {
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                IQueryable<wq_dealerInfo> query = db.wq_dealerInfo.AsNoTracking();
                int total;
                query = pageReq.ApplyQuery<wq_dealerInfo>(query, "CompCode", "desc", out total);
                dynamic result = new System.Dynamic.ExpandoObject();
                result.rows = query.ToList();
                result.total = total;
                return result;
            }
        }
        public object NewCompCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_dealerInfo", null));
        }

    }
}
