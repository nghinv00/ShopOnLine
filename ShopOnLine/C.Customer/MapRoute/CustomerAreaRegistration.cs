using System.Web.Mvc;
using System.Web.Routing;

namespace ShopOnLine.Areas.DanhMuc
{
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "ShopOnLine.Controllers" }
            );
        }

    }
}