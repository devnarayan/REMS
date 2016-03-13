using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace REMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string path = System.Web.HttpContext.Current.Server.MapPath("/");//.ApplicationPath.ToLower();
            log4net.GlobalContext.Properties["LogFileName"] = path + "\\App_Log\\MyloggerSite.log";
            log4net.Config.DOMConfigurator.Configure();
        }
    }
}
