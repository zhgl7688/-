using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Fruit.Web.Controllers
{
    /// <summary>
    /// 提供表单“关联”API 处理控制器
    /// </summary>
    public class XtApiController : ApiController
    {
        /// <summary>
        /// 表单请求关联菜单
        /// </summary>
        /// <param name="id">主表 Code</param>
        /// <param name="context">所在连接</param>
        /// <returns>关联菜单项目集合</returns>
        [HttpPost]
        public object Get(int id, string context, JObject data)
        {
            var menuItems = new SysMenuServices().GetUserMenuList(User.Identity.Name, true);
            var menus = new List<XtMenuItem>();
            var tables = new List<XtTable>();
            var connStr = ConfigurationManager.ConnectionStrings[context];
            using (var conn = OneConnectionFactories.GetConnection(connStr.ProviderName, connStr.ConnectionString))
            {

                // 加载菜单
                string sql = string.Format("select t.Code, f.ItemName, SourceTable = (select top 1 MasterTable from xt_djflowm where code = {0}), f.MasterTable, f.MenuCode from Xt_DjTransMain as t join xt_djflowm as f on f.Code = t.TargetTable where t.SourceTable = {0}", id);
                using (var reader = conn.QueryReader(sql))
                {
                    XtTable table;
                    while (reader.Read())
                    {
                        tables.Add(table = new XtTable
                        {
                            Code = reader.GetInt32(0),
                            ItemName = reader.GetString(1),
                            SourceTable = reader.GetString(2),
                            MasterTable = reader.GetString(3)
                        });
                        if (!reader.IsDBNull(4))
                        {
                            table.MenuCode = reader.GetString(4);
                        }
                    }
                }
                // 加载字段关联
                foreach (var table in tables)
                {
                    table.FieldMaps = new Dictionary<string, string>();
                    sql = string.Format("select Sourcefield, Targetfield from Xt_djfield where mainCode = {0} and type='主表'", table.Code);
                    using (var reader = conn.QueryReader(sql))
                    {
                        while (reader.Read())
                        {
                            table.FieldMaps.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }

                //检查当前用户权限 & 启用菜单
                var userServices = new SysUserService();
                XtMenuItem menuItem = null;
                for (var i = 0; i < tables.Count; i++)
                {
                    if (IsUrl(tables[i].MenuCode))
                    {
                        menus.Add(menuItem = new XtMenuItem
                        {
                            Enabled = true,
                            Title = tables[i].ItemName,
                            Url = UrlFormat(tables[i].MenuCode, data),
                            Blank = true
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tables[i].MenuCode))
                        {
                            var menuCodes = tables[i].MenuCode.Split(',');
                            // 测试用户是否有访问权限
                            if (menuItems.Any(mi => mi.MenuCode == menuCodes[0]))
                            {
                                var sysMenuItem = menuItems.FirstOrDefault(mi => mi.MenuCode == tables[i].MenuCode);
                                menus.Add(menuItem = new XtMenuItem
                                {
                                    Enabled = true,
                                    Title = tables[i].ItemName
                                });
                                if (sysMenuItem != null)
                                {
                                    if (!string.IsNullOrEmpty(sysMenuItem.IconClass))
                                    {
                                        menuItem.IconClass = sysMenuItem.IconClass;
                                    }
                                    var dataKey = menuCodes.Length > 1 ? menuCodes[1] : tables[i].FieldMaps.First().Key;
                                    menuItem.Url = sysMenuItem.URL + "/" + data[dataKey];
                                }
                                // 测试数据是否存在
                                var ex = conn.ExecuteScalar(string.Format(
                                    "select 1 where EXISTS(select * from {0} where {1})",
                                    tables[i].MasterTable,
                                    string.Join(" AND ", tables[i].FieldMaps.Select(kv => string.Format("{0}='{1}'", kv.Value, data[kv.Key])))
                                    ));
                                if (ex == null)
                                {
                                    menuItem.Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            return menus;
        }

        private string UrlFormat(string format, JObject data)
        {
            Regex r = new Regex(@"\{([^\}]*)\}", RegexOptions.Compiled);
            return r.Replace(format, (match) =>
            {
                return data[match.Groups[1].Value].ToString();
            });
        }

        private bool IsUrl(string text)
        {
            return text != null && text.IndexOf('/') > -1;
        }

        public class XtTable
        {
            public int Code { get; set; }
            public string ItemName { get; set; }
            public string SourceTable { get; set; }
            public string MasterTable { get; set; }
            public string MenuCode { get; set; }
            public Dictionary<string, string> FieldMaps { get; set; }
        }

        public class XtMenuItem
        {
            /// <summary>
            /// 关联菜单项名称
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 关联菜单项图标
            /// </summary>
            public string IconClass { get; set; }
            /// <summary>
            /// 关联菜单项地址
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 关联菜单项是否可用
            /// </summary>
            public bool Enabled { get; set; }
            public bool Blank { get; set; }
        }
    }
}
