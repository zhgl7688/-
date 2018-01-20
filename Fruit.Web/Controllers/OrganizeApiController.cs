using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fruit.Web.Controllers
{
    public class OrganizeApiController : ApiController
    {
        private static sys_user CurrentUser
        {
            get { return System.Web.HttpContext.Current.Session["sys_user"] as sys_user; }
        }

        internal static List<OrganizeTreeNode> GetOrganizeUsers()
        {
            var user = CurrentUser;
            if (user == null) return null;

            using (var db = new SysContext())
            {
                var list = db.Database.SqlQuery<OrganizeTreeNode>(
                @"select OrganizeCode as id, ParentCode as parentId, compCode, OrganizeName as text, nodeType = 'Organize' from sys_organize where compCode = @p0
union all
select psncode as id, position as parentId, compCode, name as text, nodeType='User' from [LUOLAI1401].[dbo].[PersonInfo] where compCode = @p0",
                    user.CompCode
                ).ToList();

                // 为客户端脚本标记树根节点位置
                var rootOrganize = list.FirstOrDefault(o => o.nodeType == "Organize" && o.id == o.compCode && o.id == user.CompCode);
                if (rootOrganize != null)
                {
                    rootOrganize.parentId = "#root#";
                }

                return list;
            }
        }

        // 获取组织用户树结构需要的数据
        // GET api/<controller>
        public IEnumerable<object> Get()
        {
            return GetOrganizeUsers();
        }

        public class OrganizeTreeNode
        {
            public string id { get; set; }
            public string parentId { get; set; }
            public string text { get; set; }
            public string compCode { get; set; }
            public string nodeType { get; set; }
        }
    }
}