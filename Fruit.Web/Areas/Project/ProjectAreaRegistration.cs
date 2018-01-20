using System.Web.Mvc;

namespace Fruit.Web.Areas.Project
{
    public class ProjectAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Project";
            }
        }
    }
}