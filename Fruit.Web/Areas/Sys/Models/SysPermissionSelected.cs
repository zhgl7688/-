using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web.Areas.Sys.Models
{
    public class SysPermissionSelected : sys_permission
    {
        public bool Selected { get; set; }
    }
}