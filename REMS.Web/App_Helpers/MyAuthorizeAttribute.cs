using REMS.Data.Access.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.App_Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            string controller = request.RequestContext.RouteData.Values["controller"].ToString();
            string action = request.RequestContext.RouteData.Values["action"].ToString();
            string username = System.Security.Principal.GenericPrincipal.Current.Identity.Name;

            AuthorizeService aservice = new AuthorizeService();
        bool bl=  aservice.IsUserAuthorize(username, action, controller, "");
            //dbSBPEntities2 context = new dbSBPEntities2();
            //var model = context.UserAccesses.Where(md => md.ControllerName == controller && md.ActionName == action && md.UserName == username).ToList();
            if (!bl)
            {
               // httpContext.Response.Redirect("~/Home/AccessDenied");
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}