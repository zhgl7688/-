using Fruit.Data;
using Fruit.Models;
using Fruit.Web.Areas.Sys.Models;
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
    public class RoleController : Controller
    {
        // GET: Sys/Role
        public ActionResult Index()
        {
            return View();
        }
    }

    public class RoleApiController : ApiController
    {
        public object Get()
        {
            using (var db = new SysContext())
            {
                return db.sys_role.AsNoTracking().OrderBy(r => r.RoleSeq).ToList();
            }
        }

        public object Post(JArray array)
        {
            using (var db = new SysContext())
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var row_state = (RowState)(int)array[i]["_row_state"];
                    sys_role role = array[i].ToObject<sys_role>();
                    switch (row_state)
                    {
                        case RowState.Changed:
                            var oldRole = db.sys_role.Find(role.RoleCode);
                            oldRole.RoleName = role.RoleName;
                            oldRole.RoleCode = role.RoleCode;
                            oldRole.RoleSeq = role.RoleSeq;
                            oldRole.Description = role.Description;
                            oldRole.UpdateDate = DateTime.Now;
                            oldRole.UpdatePerson = User.Identity.GetUserInfo().UserName;
                            break;
                        case RowState.New:
                            role.CreateDate = DateTime.Now;
                            role.CreatePerson = User.Identity.GetUserInfo().UserName;
                            db.sys_role.Add(role);
                            break;
                        case RowState.Deleted:
                            db.sys_organizeRoleMap.Remove(r => r.RoleCode == role.RoleCode);
                            db.sys_userRoleMap.Remove(r => r.RoleCode == role.RoleCode);
                            db.sys_roleMenuMap.Remove(r => r.RoleCode == role.RoleCode);
                            db.sys_role.Remove(db.sys_role.Find(role.RoleCode));
                            break;
                    }
                }
                db.SaveChanges();
            }
            return Get();
        }

        [System.Web.Http.HttpGet]
        public List<SelectListItem> Members(string id)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<SelectListItem>(@"select 'user.'+u.UserCode value, '[用户] '+u.UserName + ' | ' + u.UserCode text from sys_user u
where u.UserCode in (select UserCode from sys_userRoleMap where RoleCode=@roleCode)
union
select 'organize.'+u.OrganizeCode value, '[机构] '+u.OrganizeName + ' | ' + u.OrganizeCode text from sys_organize u
where u.OrganizeCode in (select OrganizeCode from sys_organizeRoleMap where RoleCode=@roleCode)", new SqlParameter("roleCode", id)).ToList();
            }
        }

        public void SaveMembers(string id, JArray data)
        {
            var oldList = Members(id);
            using (var db = new SysContext())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    string item = (string)data[i];

                    if (!oldList.Remove(s => s.Value == item))
                    {
                        var d = item.Split('.');
                        if (d[0] == "user")
                        {
                            db.sys_userRoleMap.Add(new sys_userRoleMap {
                                RoleCode = id,
                                UserCode = d[1]
                            });
                        }
                        else
                        {
                            db.sys_organizeRoleMap.Add(new sys_organizeRoleMap
                            {
                                RoleCode = id,
                                OrganizeCode = d[1]
                            });
                        }
                        // New Item
                    }
                }

                foreach (var delItem in oldList)
                {
                    var d = delItem.Value.Split('.');
                    var code = d[1];
                    if (d[0] == "user")
                    {
                        db.sys_userRoleMap.Remove(u => u.RoleCode == id && u.UserCode == code);
                    }
                    else
                    {
                        db.sys_organizeRoleMap.Remove(u => u.RoleCode == id && u.OrganizeCode == code);
                    }
                }

                db.SaveChanges();

            }
        }

        [System.Web.Http.HttpGet]
        public object AllMembers()
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            using (var db = new SysContext())
            {
                result.organizes = db.Database.SqlQuery<SelectListItem>(@"select 'organize.'+u.OrganizeCode value, u.OrganizeName text from sys_organize u").ToList();
                result.users = db.Database.SqlQuery<SelectListItem>(@"select 'user.'+u.UserCode value, u.UserName text from sys_user u").ToList();
            }
            return result;
        }

        public object Menus(string id)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<SysMenuSelected>(@"select m.MenuName, m.MenuCode, m.ParentCode, m.Description, convert(bit, (select count(*) from sys_roleMenuMap r where r.MenuCode = m.MenuCode and r.RoleCode=@roleCode)) Selected from sys_menu m", new SqlParameter("@roleCode", id)).ToList();
            }
        }

        [System.Web.Http.HttpGet]
        public object Buttons(string id)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            using (var db = new SysContext())
            {
                result.buttons = db.sys_button.AsNoTracking().OrderBy(b => b.ButtonSeq).ToList();
                result.menuButtons = db.sys_menuButtonMap.AsNoTracking().ToList();
                result.roleMenuButtons = db.sys_roleMenuButtonMap.AsNoTracking().Where(r => r.RoleCode == id).ToList();
            }
            return result;
        }

        public object SaveButtons(string id, JObject data)
        {
            using (var db = new SysContext())
            {
                db.sys_roleMenuMap.Remove(r => r.RoleCode == id);
                JArray menuArray = data["menus"] as JArray;
                for (int i = 0; i < menuArray.Count; i++)
                {
                    db.sys_roleMenuMap.Add(new sys_roleMenuMap
                    {
                        RoleCode = id,
                        MenuCode = (string)menuArray[i]
                    });
                }

                JArray dataArray = data["datas"] as JArray;
                if (dataArray != null)
                {
                    db.sys_rolePermissionMap.Remove(r => r.RoleCode == id);
                    for (int i = 0; i < dataArray.Count; i++)
                    {
                        var PermissionCode = (string)dataArray[i];
                        db.sys_rolePermissionMap.Add(new sys_rolePermissionMap
                        {
                            RoleCode = id,
                            PermissionCode = PermissionCode
                        });
                    }
                }
                
                JArray buttonArray = data["buttons"] as JArray;
                if(buttonArray != null) {
                    db.sys_roleMenuButtonMap.Remove(r => r.RoleCode == id);
                    for (int i = 0; i < buttonArray.Count; i++)
                    {
                        var item = (JObject)buttonArray[i];
                        var ButtonCode = (string)item["ButtonCode"];
                        var MenuCode = (string)item["MenuCode"];
                        db.sys_roleMenuButtonMap.Add(new sys_roleMenuButtonMap
                        {
                            RoleCode = id,
                            ButtonCode = ButtonCode,
                            MenuCode = MenuCode
                        });
                    }
                }

                db.SaveChanges();
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        public object Datas(string id)
        {
            using (var db = new SysContext())
            {
                return db.Database.SqlQuery<SysPermissionSelected>(@"select m.*, convert(bit, (select count(*) from sys_rolePermissionMap r where r.PermissionCode = m.PermissionCode and r.RoleCode=@roleCode)) Selected from sys_permission m", new SqlParameter("@roleCode", id)).ToList();
            }
        }
    }
}