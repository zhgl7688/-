using System.Web.Mvc;

namespace Fruit.Web.Areas.Message
{
    public class MessageAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Message";
            }
        }
    }
}