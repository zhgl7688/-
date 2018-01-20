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

namespace Fruit.Web.Areas.Rols.Controllers
{
    public partial class PractisecertController : Controller
    {

        public ActionResult Edit2(long id)
        {
            ViewBag.id = id;
            HR_PractiseCerts hr;
            using (var db = new LUOLAI1401Context())
            {
                hr = db.HR_PractiseCerts.FirstOrDefault(s => s.FID == id);

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

        public ActionResult Edit3(HR_PractiseCerts hr)
        {

            using (var db = new LUOLAI1401Context())
            {
                var oldhr = db.HR_PractiseCerts.FirstOrDefault(s => s.FID == hr.FID);
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

    public partial class PractisecertApiController : ApiController
    {

    }
}
