using System.Web.Mvc;

namespace Fruit.Web.Areas.Member
{
    public class MemberAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Member";
            }
        }
    }
}