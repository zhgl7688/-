using System.Web.Mvc;

namespace Fruit.Web.Areas.Business
{
    public class BusinessAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Business";
            }
        }
    }
}