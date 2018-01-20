using Fruit.Data;
using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fruit.Web
{
    public class SessionExpressionReplacer : IFruitExpressionReplacer
    {
        public bool CanHandle(string name, object context)
        {
            return name.StartsWith("SESSION.", StringComparison.OrdinalIgnoreCase) || name.StartsWith("S.", StringComparison.OrdinalIgnoreCase);
        }

        public string Replace(string name, object context)
        {
            var paths = name.Split('.');
            return (string)HttpContext.Current.Session[paths[1]];
        }
    }

    public class UserExpressionReplacer : IFruitExpressionReplacer
    {
        public bool CanHandle(string name, object context)
        {
            return name.StartsWith("USER.", StringComparison.OrdinalIgnoreCase) || name.StartsWith("U.", StringComparison.OrdinalIgnoreCase);
        }

        public string Replace(string name, object context)
        {
            var sys_user = HttpContext.Current.Session["sys_user"] as sys_user;
            if (sys_user != null)
            {
                return sys_user.GetValue<string>(string.Join(".", name.Split('.').Skip(1)));
            }
            return null;
        }
    }
}
