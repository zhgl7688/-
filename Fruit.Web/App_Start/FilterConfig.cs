using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AsnycRequestNoLayoutAttribute());
            filters.Add(new MenuRoleAuthorizeAttribute());
        }
    }
}
