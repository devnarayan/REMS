using System.Web.Mvc;

namespace REMS.Web.Areas.BI
{
    public class BIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BI_default",
                "BI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}