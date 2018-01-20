using System.Web.Mvc;

namespace Fruit.Web.Areas.Psi
{
    public class PsiAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Psi";
            }
        }
    }
}