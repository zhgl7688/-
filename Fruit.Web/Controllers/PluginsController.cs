using Fruit.Models;
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web.Controllers
{
    public partial class PluginsController : Controller
    {
        static PluginsController()
        {
            RegisterLookupConfig();
        }

        private static Dictionary<string, LookupConfig> LookupConfigs;

        // GET: Plugins
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchScheme(string pageCode, string method = "list")
        {
            var user = System.Web.HttpContext.Current.Session["sys_user"] as sys_user;
            var sss = new SysSerialServices();
            object result = null;
            using (var db = new SysContext())
            {
                switch(method)
                {
                    case "list":
                        if(user == null)
                        {
                            result = db.sys_searchScheme.Where(s => s.PageCode == pageCode).OrderBy(s => s.Title).Select(s=>new {
                                s.Id,
                                s.Title,
                                s.Data,
                                deleable = false
                            }).ToList();
                        }
                        else
                        {
                            result = db.sys_searchScheme.Where(s => s.PageCode == pageCode && (s.CompCode == null || s.CompCode == user.CompCode)).OrderBy(s => s.Title).Select(s => new {
                                s.Id,
                                s.Title,
                                s.Data,
                                deleable = s.CompCode == user.CompCode && s.UserCode == user.UserCode
                            }).ToList();
                        }
                        break;
                    case "add":
                        if(user != null)
                        {
                            var ss = new sys_searchScheme
                            {
                                Id = sss.GetNewSerial(db, "sys_searchScheme"),
                                CompCode = user.CompCode,
                                UserCode = user.UserCode,
                                PageCode = pageCode,
                                Title = Request["title"],
                                Data = Request["data"]
                            };
                            db.sys_searchScheme.Add(ss);
                            db.SaveChanges();
                            result = new { ss.Id, ss.Title, ss.Data, deleable = true };
                        }
                        break;
                    case "del":
                        if(user != null)
                        {
                            var ss = db.sys_searchScheme.Find(int.Parse(Request["id"]));
                            if(ss != null && ss.CompCode == user.CompCode && ss.UserCode == user.UserCode)
                            {
                                db.Entry(ss).State = EntityState.Deleted;
                                db.SaveChanges();
                                result = true;
                            }
                        }
                        break;
                }
            }
            return new JsonNetResult(result);
        }

        public ActionResult Popup(string id)
        {
            try
            {
                if (!LookupConfigs.ContainsKey(id))
                {
                    throw new Exception(id + " 不是已知的可选择数据源");
                }
                var cfg = LookupConfigs[id];
                return Content(cfg.GetContext());
            }
            catch (Exception e)
            {
                return Content("<div style=color:red>错误：" + e.Message + "</div>");
            }
        }

        public ActionResult GetLookupData(string id)
        {
            if (!LookupConfigs.ContainsKey(id))
            {
                throw new Exception(id + " 不是已知的可选择数据源");
            }

            var cfg = LookupConfigs[id];
            var pr = new PageRequest();

            //return Json(cfg.GetData(pr), JsonRequestBehavior.AllowGet);
            return new JsonNetResult(cfg.GetData(pr));
        }

        public class LookupConfig
        {
            public Func<string> GetContext { get; set; }

            public Func<PageRequest, object> GetData { get; set; }
        }
    }
}