using System.Web.Mvc;

namespace Fruit.Web.Areas.Put
{
    public class PutAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Put";
            }
        }
    }
}