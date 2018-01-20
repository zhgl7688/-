using System.Web.Mvc;

namespace Fruit.Web.Areas.Fa
{
    public class FaAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Fa";
            }
        }
    }
}