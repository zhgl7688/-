using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class PermissionController : Controller
    {
        // GET: Sys/Permission
        public ActionResult Index()
        {
            return View();
        }
    }

    public class PermissionApiController : ApiController
    {
        public object Get()
        {

            using (var db = new SysContext())
            {
                return db.sys_permission.AsNoTracking().LeftJoin(db.sys_permission, a => a.ParentCode, b => b.PermissionCode, (a, b) => new
                {
                    a.PermissionCode,
                    a.PermissionName,
                    a.ParentCode,
                    a.CreateDate,
                    a.CreatePerson,
                    a.UpdateDate,
                    a.UpdatePerson,
                    ParentName = b.PermissionName
                }).OrderBy(a => a.PermissionCode).ToList();
            }
        }

        public object Post(JArray array)
        {
            DynamicData.Save<SysContext, sys_permission>(array);
            return Get();
        }
    }
}