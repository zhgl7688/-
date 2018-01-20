using System.Web.Mvc;

namespace Fruit.Web.Areas.Baseinfo
{
    public class BaseinfoAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Baseinfo";
            }
        }
    }
}