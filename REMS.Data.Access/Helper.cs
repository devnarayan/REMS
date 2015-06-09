using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace REMS.Data.Access
{
   public class Helper
    {
        public void LogException(Exception exception)
        {
            // try-catch because database itself could be down or Request context is unknown.

            try
            {
                int? userId = null;
                try { userId = 0; }
                catch { /* do nothing */ }

                // ** Prototype pattern. the Error object has it default values initialized

                var error = new Error()
                {
                    UserName = userId.Value.ToString(),
                    Exception = exception.GetType().FullName,
                    Message = exception.Message,

                    Everything = exception.ToString(),
                    IpAddress = HttpContext.Current.Request.UserHostAddress,
                    UserAgent = HttpContext.Current.Request.UserAgent,
                    PathAndQuery = HttpContext.Current.Request.Url == null ? "" : HttpContext.Current.Request.Url.PathAndQuery,
                    HttpReferer = HttpContext.Current.Request.UrlReferrer == null ? "" : HttpContext.Current.Request.UrlReferrer.PathAndQuery,
                    CreatedOn = DateTime.Now,
                    ChangedOn = DateTime.Now
                };
                using (REMSDBEntities context = new REMSDBEntities())
                {
                    context.Errors.Add(error);
                    context.SaveChanges();
                }
                //DealSiteContext.Errors.Insert(error);
            }
            catch { /* do nothing, or send email to webmaster*/}
        }
        public void LogExceptionNo(Exception exception, string no, string username)
        {
            // try-catch because database itself could be down or Request context is unknown.

            try
            {
                // ** Prototype pattern. the Error object has it default values initialized
                var error = new Error()
                {
                    UserName = username,
                    ErrorID = no,
                    Exception = exception.GetType().FullName,
                    Message = exception.Message,
                    Everything = exception.ToString(),
                    IpAddress = HttpContext.Current.Request.UserHostAddress,
                    UserAgent = HttpContext.Current.Request.UserAgent,
                    PathAndQuery = HttpContext.Current.Request.Url == null ? "" : HttpContext.Current.Request.Url.PathAndQuery,
                    HttpReferer = HttpContext.Current.Request.UrlReferrer == null ? "" : HttpContext.Current.Request.UrlReferrer.PathAndQuery,
                    CreatedOn = DateTime.Now,
                    ChangedOn = DateTime.Now
                };
                using (REMSDBEntities context = new REMSDBEntities())
                {
                    context.Errors.Add(error);
                    context.SaveChanges();
                }
                //DealSiteContext.Errors.Insert(error);
            }
            catch { /* do nothing, or send email to webmaster*/}
        }
    }
}
