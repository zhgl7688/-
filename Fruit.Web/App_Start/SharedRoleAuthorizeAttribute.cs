using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web
{
    public class SharedRoleAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// 指定行为共享特定 URL 的页面权限而不用单独设置权限
        /// </summary>
        /// <param name="url">测试权限使用的 URL</param>
        public SharedRoleAuthorizeAttribute(string url)
        {
            this.RoleUrl = url;
        }

        public string RoleUrl { get; private set; }
    }
}