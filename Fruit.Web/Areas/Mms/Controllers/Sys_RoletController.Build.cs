/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class Sys_RoletController : Controller
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
            sys_rolet form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.sys_rolet.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new sys_rolet
                {
                    RoleCode = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Sys_RoletApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class sys_roletListModel {
            public string RoleCode { get; set; }
            public string RoleName { get; set; }
            public string Description { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "((a.id='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
            SerachCondition.TextBox(sbCondition, "RoleName", "a.RoleName", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<sys_roletListModel>(db.Database, "a.RoleCode ,a.RoleName ,a.Description ", "sys_rolet a ", sbCondition.ToString(), "RoleCode", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class sys_roleMenuMap_appInsys_roletModel {
            public string RoleCode { get; set; }
            public int id { get; set; }
            public string MenuCode { get; set; }
            public string RMenuName { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var sys_roleMenuMap_app = pageReq.ToPageList<sys_roleMenuMap_appInsys_roletModel>(db.Database, "a.RoleCode ,a.id ,a.MenuCode ,a.RMenuName ", "sys_roleMenuMap_app a ", "a.RoleCode = '" + id + "'", "a.id", "desc").Rows;
                return new {sys_roleMenuMap_app};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<sys_rolet>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.sys_rolet.Find(form.RoleCode);
                if (dbForm == null)
                {
                    db.sys_rolet.Add(form);
                }
                else
                {
                    dbForm.RoleName = form.RoleName;
                    dbForm.Description = form.Description;
                }
                if( post["sys_roleMenuMap_app"] != null )
                {
                    var sub= post["sys_roleMenuMap_app"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<sys_roleMenuMap_app>();
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.RoleCode = form.RoleCode;
                                    model.id = new SysSerialServices().GetNewSerial("sys_roleMenuMap_app." + form.RoleCode, null);
                                    db.sys_roleMenuMap_app.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.sys_roleMenuMap_app.Remove(r => r.RoleCode == did);
                    db.sys_rolet.Remove(r => r.RoleCode == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewRoleCode()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("sys_rolet", null));
        }

    }
}
