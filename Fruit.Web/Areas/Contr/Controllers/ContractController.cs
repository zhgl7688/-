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

namespace Fruit.Web.Areas.Contr.Controllers
{
    public partial class ContractController : Controller
    {
        //Contract
        public ActionResult Edit2(string id)
        {
            ViewBag.id = id;
            PM_Contracts hr;
            using (var db = new LUOLAI1401Context())
            {
                hr = db.PM_Contracts.FirstOrDefault(s => s.Code == id);

            }
            if (hr != null)
            {
                ViewBag.Scan1 = hr.Scan1;
                ViewBag.Scan2 = hr.Scan2;
                ViewBag.Scan3 = hr.Scan3;
                ViewBag.Scan4 = hr.Scan4;

            }
            else
            {
                ViewBag.Scan1 = "";
                ViewBag.Scan2 = "";
                ViewBag.Scan3 = "";
                ViewBag.Scan4 = "";
            }

            return View(hr);
        }

        public ActionResult Edit3(PM_Contracts hr)
        {

            using (var db = new LUOLAI1401Context())
            {
                var oldhr = db.PM_Contracts.FirstOrDefault(s => s.Code == hr.Code);
                if (oldhr != null)
                {
                    oldhr.Scan1 = hr.Scan1;
                    oldhr.Scan2 = hr.Scan2;
                    oldhr.Scan3 = hr.Scan3;
                    oldhr.Scan4 = hr.Scan4;
                }
                db.SaveChanges();
            }

            return Content("<script>alert('更新成功');window.close();</script>");

            //return Content("<script>alert('更新成功');location='" + Url.Action("Practisecert", "Rols") + "'</script>");



        }
    }

    public partial class ContractApiController: ApiController
    {

    }
}
