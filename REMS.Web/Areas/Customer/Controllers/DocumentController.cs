using AutoMapper;
using REMS.Data;
using REMS.Web.App_Helpers;
using REMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using REMS.Data.Access.Custo;
using REMS.Data.DataModel;


namespace REMS.Web.Areas.Customer.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Customer/Document/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateAgreementAction(int Id)
        {
            ViewBag.ID = Id;
            return View();
        }

        public ActionResult UploadAgreement(int Id)
        {
            ViewBag.ID = Id;
            return View();
        }
        public string AgreementInfo(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);
            var model = context.Agreements.Where(ag => ag.SaleID == sid).FirstOrDefault();
            if (model != null)
            {
                Mapper.CreateMap<Agreement, AgreementModel>();
                var md = Mapper.Map<Agreement, AgreementModel>(model);
                if (model.CrDate != null)
                    md.CrDateSt = model.CrDate.Value.ToString("dd/MM/yyyy");
                if (model.UploadDate != null)
                    md.UploadDateSt = model.UploadDate.Value.ToString("dd/MM/yyyy");
                if (md.DocURL != null)
                    md.DocURL = md.DocURL.TrimStart('~');
                if (md.HTMLURL != null)
                    md.HTMLURL = md.HTMLURL.TrimStart('~');
                if (md.AssuredDocURL != null)
                    md.AssuredDocURL = md.AssuredDocURL.TrimStart('~');
                if (md.AssuredHTMLURL != null)
                    md.AssuredHTMLURL = md.AssuredHTMLURL.TrimStart('~');
                return Newtonsoft.Json.JsonConvert.SerializeObject(md);
            }
            else
            {
                return "NO";
            }
        }

        public string GenerateAgreement(string atype, string adate, string saleid)
        {
            string sourceFile = "";

            if (atype == "500")
                sourceFile = "~/Content/agreement/CityHeartAgreement500.htm";
            else if (atype == "100")
                sourceFile = "~/Content/agreement/CityHeartAgreement100.htm";
            else
                sourceFile = "~/Content/agreement/CityHeartAgreement50.htm";
            string htmlFile = "~/Agreement/HTML/" + saleid + ".htm";
            string destDocFile = "~/Agreement/Doc/" + saleid + ".doc";

            string AllotmetSoc = "~/Content/agreement/Allotment.htm";
            string Allotmetdec = "~/Agreement/Allotment/" + saleid + ".htm";


            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(Server.MapPath(sourceFile), Server.MapPath(htmlFile), true);
            System.IO.File.Copy(Server.MapPath(AllotmetSoc), Server.MapPath(Allotmetdec), true);
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleid);
                var Sale = context.SaleFlats.Where(se => se.SaleID == sid).FirstOrDefault();
                var project = context.Projects.Where(pro => pro.ProjectID == Sale.ProjectID).FirstOrDefault();
                var cust = context.Customers.Where(cu => cu.SaleID == Sale.SaleID).FirstOrDefault();
                var installment = context.FlatInstallmentDetails.Where(ins => ins.FlatID == Sale.FlatID).ToList();
               // var installment = context.tblSInstallmentDetails.Where(ins => ins.SaleID == sid).ToList();
               // var planty = context.PlanTypeMasters.Where(pl => pl.PlanID == Sale.PlanID).FirstOrDefault();
              //  var plansize = context.tblSPropertySizes.Where(pl => pl.PropertyTypeID == Sale.PropertySizeID).FirstOrDefault();
                var flat = context.Flats.Where(fl => fl.FlatID == Sale.FlatID).FirstOrDefault();
                if (cust.Address1 == null || cust.Address1 == "") cust.Address1 = ".";
                if (cust.CoAddress1 == null || cust.CoAddress1 == "") cust.CoAddress1 = ".";
                // Agreement Letter
                var fileContents = System.IO.File.ReadAllText(Server.MapPath(htmlFile));
                fileContents = fileContents.Replace("<% ProjectName %>", flat.FlatName);
                fileContents = fileContents.Replace("<% OfficeAddress %>", project.OfficeAddress);
                fileContents = fileContents.Replace("<% AgreementDate %>", adate);
                fileContents = fileContents.Replace("<% CompanyName %>", project.CompanyName);
                fileContents = fileContents.Replace("<% PropertyAddress %>", project.Address);
                fileContents = fileContents.Replace("<% AuthoritySign %>", project.AuthoritySign);
                fileContents = fileContents.Replace("<% CustomerFatherName %>", cust.PName);
                fileContents = fileContents.Replace("<% CoAppFatherName %>", cust.SecCoPName);
                fileContents = fileContents.Replace("<% CustomerFullName %>", cust.AppTitle + "" + cust.FName + " " + cust.MName + " " + cust.LName);
                fileContents = fileContents.Replace("<% CoAppFullName %>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName);
                fileContents = fileContents.Replace("<% CoAppAddress %>", cust.CoAddress1 + " " + cust.CoAddress2);
                fileContents = fileContents.Replace("<% CustomerAddress %>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.Country);
                fileContents = fileContents.Replace("<% SaleRate %>", Sale.TotalAmount.Value.ToString());
               // fileContents = fileContents.Replace("<% PlanName %>", planty.PlanTypeName);
                string st = "";
                foreach (var inst in installment)
                {
                    st += @" <tr><td width='321' style='width:240.95pt;border:solid black 1.0pt; padding: 3pt 5.4pt 3pt 5.4pt; font-size:12.0pt;font-family:Arial,sans-serif'><p>";
                    st += inst.InstallmentID.ToString();
                    st += @"</p></td><td width='18' style='width:13.5pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'>
                    <p class='MsoNormal'>:</p>
                </td><td width='150' style='width:125pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'><p class='MsoNormal'>";
                    if (inst.DueDate != null)
                        st += inst.DueDate.Value.ToString("dd/MM/yyyy");
                    st += @"</p></td>
                <td width='301' style='width:225.95pt;border:solid black 1.0pt;  border-left:none;  padding: 3pt 5.4pt 3pt 5.4pt;'>
                    <p class='MsoNormal' style='margin-right:-81.0pt;  font-size:12.0pt;font-family:Arial,sans-serif'> ";
                    st += inst.TotalAmount.Value.ToString();
                    st += @"</p>
                </td>
                </tr>";
                }
                fileContents = fileContents.Replace("<% PlanData %>", st);


                // Allotment Letter
                var AllotmentContents = System.IO.File.ReadAllText(Server.MapPath(Allotmetdec));
                fileContents = fileContents.Replace("<% ProjectName %>", project.PName);
                fileContents = fileContents.Replace("<% OfficeAddress %>", project.OfficeAddress);


                System.IO.File.WriteAllText(Server.MapPath(htmlFile), fileContents); // html aggreement
                System.IO.File.WriteAllText(Server.MapPath(Allotmetdec), AllotmentContents); // Allotment update

                GenerateAgreementDoc(atype, adate, saleid, sid, Sale, project, cust, installment, flat);


                Agreement ag = new Agreement();

                #region Assured Return
             
                #endregion
                // Update Agreement on sale
                Sale.Aggrement = htmlFile;
                context.SaleFlats.Add(Sale);
                context.Entry(Sale).State = EntityState.Modified;
                int i = context.SaveChanges();

                // Save Agreement.

                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                ag.CrDate = Convert.ToDateTime(adate, dtinfo);
                ag.DocURL = destDocFile;
                ag.HTMLURL = htmlFile;
                ag.SaleID = sid;
                ag.AgreementAmount = atype;
                ag.CreatedBy = User.Identity.Name;
                DocumentService dc = new DocumentService();
                i = dc.SaveDocument(ag);
                //<% ProjectName %>
                //<% OfficeAddress %>
                //<% AgreementDate %>
                //<% CompanyName %>
                //<% PropertyAddress %>
                //<% AuthoritySign %>
                //<% CustomerFullName %>
                //<% CoAppFullName %>
                //<% CoAppAddress %>
                //<% CustomerAddress %>
                //<% SaleRate %>

                if (i > 0)
                    return "Yes";
                else return "No";
            }
        }


        public void GenerateAgreementDoc(string atype, string adate, string saleid, int sid, SaleFlat Sale, Project project, REMS.Data.Customer cust, List<FlatInstallmentDetail> installment, Flat flat)
        {
            string sourceFile = "";
            if (atype == "500")
                sourceFile = "~/Content/agreement/CityHeartAgreementDoc500.htm";
            else if (atype == "100")
                sourceFile = "~/Content/agreement/CityHeartAgreementDoc500.htm";
            else
                sourceFile = "~/Content/agreement/CityHeartAgreementDoc500.htm";
            string destDocFile = "~/Agreement/Doc/" + saleid + ".doc";
            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(Server.MapPath(sourceFile), Server.MapPath(destDocFile), true);
            using (REMSDBEntities context = new REMSDBEntities())
            {
                if (cust.Address1 == null || cust.Address1 == "") cust.Address1 = ".";
                if (cust.CoAddress1 == null || cust.CoAddress1 == "") cust.CoAddress1 = ".";
                // Agreement Letter
                var fileContents = System.IO.File.ReadAllText(Server.MapPath(destDocFile));
                fileContents = fileContents.Replace("<% ProjectName %>", project.PName);
                fileContents = fileContents.Replace("<% OfficeAddress %>", project.OfficeAddress);
                fileContents = fileContents.Replace("<% AgreementDate %>", adate);
                fileContents = fileContents.Replace("<% CompanyName %>", project.CompanyName);
                fileContents = fileContents.Replace("<% PropertyAddress %>", project.Address);
                fileContents = fileContents.Replace("<% AuthoritySign %>", project.AuthoritySign);
                fileContents = fileContents.Replace("<% CustomerFatherName %>", cust.PName);
                fileContents = fileContents.Replace("<% CoAppFatherName %>", cust.SecCoPName);
                fileContents = fileContents.Replace("<% CustomerFullName %>", cust.AppTitle + "" + cust.FName + " " + cust.MName + " " + cust.LName);
                fileContents = fileContents.Replace("<% CoAppFullName %>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName);
                fileContents = fileContents.Replace("<% CoAppAddress %>", cust.CoAddress1 + " " + cust.CoAddress2);
                fileContents = fileContents.Replace("<% CustomerAddress %>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.Country);
                fileContents = fileContents.Replace("<% SaleRate %>", Sale.TotalAmount.Value.ToString());
               // fileContents = fileContents.Replace("<% PlanName %>", planty.PlanTypeName);
                string st = "";
                foreach (var inst in installment)
                {
                    st += @" <tr><td width='321' style='width:240.95pt;border:solid black 1.0pt; padding: 3pt 5.4pt 3pt 5.4pt; font-size:12.0pt;font-family:Arial,sans-serif'><p>";
                    st += inst.InstallmentID.ToString();
                    st += @"</p></td><td width='18' style='width:13.5pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'>
                    <p class='MsoNormal'>:</p>
                </td><td width='150' style='width:125pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'><p class='MsoNormal'>";
                    if (inst.DueDate != null)
                        st += inst.DueDate.Value.ToString("dd/MM/yyyy");
                    st += @"</p></td>
                <td width='301' style='width:225.95pt;border:solid black 1.0pt;  border-left:none;  padding: 3pt 5.4pt 3pt 5.4pt;'>
                    <p class='MsoNormal' style='margin-right:-81.0pt;  font-size:12.0pt;font-family:Arial,sans-serif'> ";
                    st += inst.TotalAmount.Value.ToString();
                    st += @"</p>
                </td>
                </tr>";
                }
                fileContents = fileContents.Replace("<% PlanData %>", st);

                System.IO.File.WriteAllText(Server.MapPath(destDocFile), fileContents);
            }
        }
        public string EmailAgreement(string saleid, string name, string email, string url)
        {
            string str = SendMail.AgreementMail(email, name, url);
            // str=OK,Error
            return str;
        }

        
	}
}