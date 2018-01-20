using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys
{
    public class SysAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Sys";
            }
        }
    }
}