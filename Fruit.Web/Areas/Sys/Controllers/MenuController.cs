using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHttp = System.Web.Http;
using System.Web.Mvc;
using System.Net;
using Fruit.Data;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class MenuController : Controller
    {
        // GET: Sys/Menu
        public ActionResult Index()
        {
            return View();
        }
    }

    public class MenuApiController : WebHttp.ApiController
    {
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        [WebHttp.HttpPost]
        public IEnumerable<dynamic> GetAll()
        {
            return new SysMenuServices().GetMenuList();
        }

        [WebHttp.HttpPost]
        public dynamic Save(JArray data)
        {
            DynamicData.Save<SysContext, sys_menu>(data);
            // 当保存简单的数据，不需要从数据库中重新刷新 UI 数据时，可以直接返回 true 表示保存成功
            // 否则，返回加载相同的数据集来自动更新客户端。
            // return true;
            return GetAll();
        }

        [WebHttp.HttpPost]
        public IEnumerable<dynamic> GetButtons(string id)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<SysButtonSelected>("select b.*, convert(bit, (select count(*) from sys_menuButtonMap m where m.MenuCode=@menuCode and m.ButtonCode = b.ButtonCode)) Selected from sys_button b", new SqlParameter("@menuCode", id)).ToList();
            }
        }

        public IEnumerable<dynamic> Buttons()
        {
            return new SysMenuServices().GetAllButtons();
        }

        public dynamic SaveButtons(string id, JArray data)
        {
            var list = data.ToObject<List<SysButtonSelected>>();
            using (var db = new SysContext())
            {
                foreach (var item in list)
                {
                    if (item.Selected)
                    {
                        if (db.sys_menuButtonMap.Count(m => m.MenuCode == id && m.ButtonCode == item.ButtonCode) == 0)
                        {
                            db.sys_menuButtonMap.Add(new sys_menuButtonMap { ButtonCode = item.ButtonCode, MenuCode = id });
                        }
                    }
                    else
                    {
                        db.sys_menuButtonMap.Remove(m => m.MenuCode == id && m.ButtonCode == item.ButtonCode);
                    }
                }
                db.SaveChanges();
            }
            // 当保存简单的数据，不需要从数据库中重新刷新 UI 数据时，可以直接返回 true 表示保存成功
            // 否则，返回加载相同的数据集来自动更新客户端。
            return true;
        }

        public dynamic SaveAllButtons(JArray data)
        {
            DynamicData.Save<SysContext, sys_button>(data);
            return true;
        }
    }
}