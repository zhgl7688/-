using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class OrganizeController : Controller
    {
        // GET: Sys/Organize
        public ActionResult Index()
        {
            return View();
        }
    }

    public class OrganizeApiController : ApiController
    {
        public ICollection<sys_organize> Get()
        {
            using (var db = new SysContext())
            {
                return db.sys_organize.AsNoTracking().OrderBy(o => o.OrganizeSeq).ToList();
            }
        }

        public object Delete(string id)
        {
            using (var db = new SysContext())
            {
                db.sys_organizeRoleMap.Remove(r => r.OrganizeCode == id);
                db.sys_userOrganizeMap.Remove(u => u.OrganizeCode == id);
                db.sys_organize.Remove(o => o.OrganizeCode == id);
                db.SaveChanges();
                return true;
            }
        }

        public object Post(JObject data)
        {
            string _id = data.Value<string>("_id");
            var org = data.ToObject<sys_organize>();
            using (var db = new SysContext())
            {
                if (string.IsNullOrEmpty(_id))
                {
                    db.sys_organize.Add(org);
                }
                else
                {
                    var organize = db.sys_organize.Find(_id);
                    DynamicData.Copy(org, organize, data);
                }
                db.SaveChanges();
            }
            return true;
        }

        [System.Web.Http.HttpPost]
        public dynamic GetRoles(string id)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<SysOrganizeRoleSelected>("select b.*, convert(bit, (select count(*) from sys_organizeRoleMap m where m.OrganizeCode=@organizeCode and m.RoleCode = b.RoleCode)) Selected from sys_role b order by b.RoleSeq", new SqlParameter("@organizeCode", id)).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public dynamic SaveRoles(string id, JArray data)
        {
            var list = data.ToObject<List<SysOrganizeRoleSelected>>();
            using (var db = new SysContext())
            {
                foreach (var item in list)
                {
                    if (item.Selected)
                    {
                        if (db.sys_organizeRoleMap.Count(m => m.OrganizeCode == id && m.RoleCode == item.RoleCode) == 0)
                        {
                            db.sys_organizeRoleMap.Add(new sys_organizeRoleMap { RoleCode = item.RoleCode, OrganizeCode = id });
                        }
                    }
                    else
                    {
                        db.sys_organizeRoleMap.Remove(m => m.OrganizeCode == id && m.RoleCode == item.RoleCode);
                    }
                }
                db.SaveChanges();
            }
            return true;
        }
    }
}