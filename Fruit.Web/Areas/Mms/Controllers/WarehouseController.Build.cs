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
    public partial class WarehouseController : Controller
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
            wq_termPop form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_termPop.Find(id);
            }
            if (form == null)
            {
                form = new wq_termPop{ CompCode = id };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class WarehouseApiController : ApiController
    {
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                IQueryable<wq_termPop> query = db.wq_termPop.AsNoTracking();
                int total;
                query = pageReq.ApplyQuery<wq_termPop>(query, "CompCode", "desc", out total);
                dynamic result = new System.Dynamic.ExpandoObject();
                result.rows = query.ToList();
                result.total = total;
                return result;
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_termPop>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_termPop.Find(form.CompCode);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_termPop.Add(form);
                }
                else
                {
                    dbForm.PopCode = form.PopCode;
                    dbForm.PopName = form.PopName;
                    dbForm.Address = form.Address;
                    dbForm.Contact1 = form.Contact1;
                    dbForm.Tel1 = form.Tel1;
                    dbForm.Mobile1 = form.Mobile1;
                    dbForm.Contact2 = form.Contact2;
                    dbForm.Tel2 = form.Tel2;
                    dbForm.Mobile2 = form.Mobile2;
                    dbForm.Long = form.Long;
                    dbForm.Lat = form.Lat;
                    dbForm.PopType = form.PopType;
                    dbForm.PopStatus = form.PopStatus;
                    dbForm.UserCode = form.UserCode;
                    dbForm.Remark = form.Remark;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new LUOLAI1401Context())
            {
                db.wq_termPop.Remove(r => r.CompCode == id);
                db.SaveChanges();
            }
            return true;
        }
        public object NewCompCode()
        {
            return new SysSerialServices().GetNewSerial("wq_termPop");
        }

    }
}
