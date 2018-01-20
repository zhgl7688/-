using System.Web.Mvc;

namespace Fruit.Web.Areas.Mms
{
    public class MmsAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mms";
            }
        }
    }
}