using System.Web.Mvc;

namespace Fruit.Web.Areas.Wage
{
    public class WageAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Wage";
            }
        }
    }
}