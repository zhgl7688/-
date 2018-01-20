using System.Web.Mvc;

namespace Fruit.Web.Areas.Whe
{
    public class WheAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Whe";
            }
        }
    }
}