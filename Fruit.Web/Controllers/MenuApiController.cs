using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fruit.Web.Controllers
{
    public class MenuApiController : ApiController
    {
        [HttpPost]
        public IEnumerable<sys_menu> Get()
        {
            // TODO
            return new SysMenuServices().GetUserMenuList(User.Identity.Name);
        }
    }
}