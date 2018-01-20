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
    public partial class TestController : Controller
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
            mms_check form = null;
            List<ComboItem> WarehouseName = null;
            using(var db = new MmsContext())
            {
                WarehouseName = db.mms_warehouse.Select(i=>new ComboItem{ Text = i.WarehouseName, Value = i.WarehouseCode }).ToList();
                form = db.mms_check.Find(id);
            }
            if (form == null)
            {
                form = new mms_check{ BillNo = id };
            }
            return View(new { form = form, dataSource = new { WarehouseName }});
        }

    }
    public partial class TestApiController : ApiController
    {
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new MmsContext())
            {
                IQueryable<mms_check> query = db.mms_check.AsNoTracking();
                int total;
                query = pageReq.ApplyQuery<mms_check>(query, "BillNo", "desc", out total);
                dynamic result = new System.Dynamic.ExpandoObject();
                result.rows = query.ToList();
                result.total = total;
                return result;
            }
        }
        public object NewBillNo()
        {
            return string.Format("{0}{1}{2}{3:D4}", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("D2"), DateTime.Now.Day.ToString("D2"), new SysSerialServices().GetNewSerial("mms_check", int.Parse(DateTime.Now.ToString("yyyyMM"))));
        }

    }
}
