using System.Web.Mvc;

namespace C.QuanTri.MapRoute
{
    public class QuanTriAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuanTri";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QuanTri_default",
                "QuanTri/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "C.QuanTri.Controllers" }
            );
        }
    }
}