using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class MenuApiController : ApiController
    {
        [HttpPost, ActionName("getall")]
        public IEnumerable<sys_menu> GetAll()
        {
            return new SysMenuServices().GetMenuList();
        }
    }
}