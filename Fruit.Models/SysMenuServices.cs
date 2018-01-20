using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fruit.Data;

namespace Fruit.Models
{
    /// <summary>
    /// 菜单信息服务
    /// </summary>
    public class SysMenuServices
    {

        /// <summary>
        /// 获取指定用户的菜单列表
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <param name="includeHidden">是否包括不显示（但启用）的菜单项</param>
        /// <returns></returns>
        public IEnumerable<sys_menu> GetUserMenuList(string userCode, bool includeHidden = false)
        {
            using (var db = new SysContext())
            {
                var sql = @"select * from sys_menu where IsVisible=1 and IsEnable=1 and MenuCode in (select MenuCode from sys_roleMenuMap where RoleCode in (select RoleCode from sys_userRoleMap where UserCode=@userCode)) order by MenuSeq";
                if (includeHidden)
                {
                    sql = @"select * from sys_menu where IsEnable=1 and MenuCode in (select MenuCode from sys_roleMenuMap where RoleCode in (select RoleCode from sys_userRoleMap where UserCode=@userCode)) order by MenuSeq";
                }
                return db.sys_menu.SqlQuery(sql, new SqlParameter("@userCode", userCode)).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// 获取所有的系统菜单列表
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetMenuList()
        {
            using (var db = new SysContext())
            {
                return db.sys_menu.AsNoTracking().LeftJoin(db.sys_menu, a => a.ParentCode, b => b.MenuCode, (a, b) => new { 
                    a.MenuCode,
                    a.MenuName,
                    a.MenuSeq,
                    a.ParentCode,
                    a.CreateDate,
                    a.CreatePerson,
                    a.Description,
                    a.IconClass,
                    a.IconURL,
                    a.IsEnable,
                    a.IsVisible,
                    a.UpdateDate,
                    a.UpdatePerson,
                    a.URL,
                    ParentName=b.MenuName
                }).OrderBy(a => a.MenuSeq).ToList();
            }
        }

        public IEnumerable<dynamic> GetAllButtons()
        {
            using (var db = new SysContext())
            {
                return db.sys_button.SqlQuery("select * from sys_button order by ButtonSeq").ToList();
            }
        }
    }
}
