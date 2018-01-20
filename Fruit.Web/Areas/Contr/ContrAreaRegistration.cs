using System.Web.Mvc;

namespace Fruit.Web.Areas.Contr
{
    public class ContrAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Contr";
            }
        }
    }
}