using REMS.Data;
using REMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace MyApp.Uploads
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                using (REMSDBEntities dbcontext = new REMSDBEntities())
                {
                    HttpFileCollection files = context.Request.Files;
                    var userName = context.Request.Form["name"];
                    var saleid = context.Request.Form["saleid"];
                    var adate = context.Request.Form["adate"];
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string ext = Path.GetExtension(file.FileName);
                        string filename = "/Agreement/Upload/" + saleid.ToString() + "" + ext;
                        string fname = context.Server.MapPath(filename);
                        file.SaveAs(fname);

                        DateTimeFormatInfo dtinfo=new DateTimeFormatInfo();
                        dtinfo.ShortDatePattern="dd/MM/yyyy";
                        dtinfo.DateSeparator="/";
                        int sid=Convert.ToInt32(saleid);
                        var ag = dbcontext.Agreements.Where(a => a.SaleID == sid).FirstOrDefault();
                        ag.UploadDate = Convert.ToDateTime(adate, dtinfo);
                        ag.UploadURL = filename;
                        ag.UploadBy = System.Security.Principal.GenericPrincipal.Current.Identity.Name;

                        dbcontext.Agreements.Add(ag);
                        dbcontext.Entry(ag).State = EntityState.Modified;
                        int ii= dbcontext.SaveChanges();

                    }
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Agreement uploaded successfully!");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
