using AutoMapper;
using REMS.Data;
using REMS.Data.Access;
using REMS.Data.Access.Custo;
using REMS.Web.App_Helpers;
using REMS.Web.Areas.Admin.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Customer.Controllers
{
     
    public class AssuredReturnController : Controller
    {
        DataFunctions obj = new DataFunctions();
        //
        // GET: /Customer/AssuredReturn/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateAssuredInstallment()
        {
            return View();
        }

        #region
        public string CheckInstallmentStatus(string PID)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                int id = Convert.ToInt32(PID);

                int sids = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == id select s.SaleID).FirstOrDefault();
                var model = context.AssuredReturns.Where(ar => ar.SaleID == sids).FirstOrDefault();
                if (model == null)
                {
                    //int fid = context.Flats.Where(f => f.FlatID == id).FirstOrDefault().FlatID;
                    // PlanID==5 Combo Plan with  Assured Return 
                    var md = context.SaleFlats.Where(s => s.FlatID == id).FirstOrDefault();
                    if (md == null)
                    {
                        string[] st = new string[2];
                        st[0] = "No";
                        st[1] = "No Found";
                        return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Not Found
                    }
                    else
                    {
                        List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
                        DataTable payment = obj.GetDataTable("select sum(Amount) as TotalAmount from Payment where saleid='" + md.SaleID + "'");
                        if (payment.Rows[0]["TotalAmount"].ToString() == "")
                        {
                            string[] st = new string[2];
                            st[0] = "LessPayment";
                            st[1] = "Payment not submitted 50%";
                            return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Not Found
                        }
                        else
                        {

                            decimal tpaid = Convert.ToDecimal(payment.Rows[0]["TotalAmount"].ToString());
                            decimal srate = (decimal)md.TotalAmount;
                            if (tpaid >= srate / 2)
                            {
                                string[] st = new string[2];
                                st[0] = md.SaleID.ToString();
                                int sid = Convert.ToInt32(md.SaleID);
                               // decimal? amt = context.FlatInstallmentDetails.Where(ins => ins.SaleID == sid).FirstOrDefault().TotalAmount;
                                st[1] = tpaid.ToString();
                                return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Not Generated and property found.
                            }
                            else  // Payment not submit 50%
                            {
                                string[] st = new string[2];
                                st[0] = "LessPayment";
                                st[1] = "Payment not submitted 50%";
                                return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Not Found
                            }

                        }
                    }
                }
                else
                {
                    string[] st = new string[2];
                    st[0] = "Yes";
                    st[1] = model.SaleID.Value.ToString();
                    return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Generated
                }
            }
            catch (Exception ex)
            {
                string[] st = new string[2];
                st[0] = "Error";
                st[1] = ex.ToString();
                return Newtonsoft.Json.JsonConvert.SerializeObject(st);// Generated
            }
        }

        public string GetAssuredReturnBySaleID(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(saleid);
            List<AssuredReturnModel> md = new List<AssuredReturnModel>();
            var model = context.AssuredReturns.Where(a => a.SaleID == id).AsEnumerable();
            foreach (var v in model)
            {
                Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                m.Amount = Math.Round(m.Amount.Value, 2);
                if (v.CrDate != null)
                    m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
                if (v.ChequeDate != null)
                    m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                if (v.Status == "Clear")
                    m.PayStatus = "hidden";
                else m.PayStatus = "";
                if (v.Status == "Updated" || v.Status == "Pending")
                {
                    m.PayStatus = "show";
                }
                else
                {
                    m.PayStatus = "hidden";
                }
                if (v.Status == "Clear")
                {
                    m.UnClearStatus = "show";
                }
                else
                {
                    m.UnClearStatus = "hidden";
                }
                md.Add(m);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        public string UpdateAssuredChequeAll(string[] ChequeNos, string[] ChequeDates, string[] asids)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                string[] cno = ChequeNos;
                string[] cdate = ChequeDates;
                string[] asid = asids;
                string rmsg = "No";
                for (int i = 0; i < cno.Length; i++)
                {
                    if (cno[i] != "" && cdate[i] != "")
                    {
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        dtinfo.DateSeparator = "/";
                        dtinfo.ShortDatePattern = "dd/MM/yyyy";
                        int id = Convert.ToInt32(asid[i]);
                        DateTime dt = Convert.ToDateTime(cdate[i], dtinfo);
                        var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                        model.ChequeNo = cno[i];
                        model.ChequeDate = dt;
                        model.Status = "Updated";
                        model.ModifyBy = User.Identity.Name;
                        model.ModifyDate = DateTime.Now;
                        context.Entry(model).State = EntityState.Modified;
                        int ii = context.SaveChanges();
                        if (ii >= 1)
                        {
                            rmsg = model.SaleID.Value.ToString();
                        }
                        // else rmsg= "No";
                    }
                    // else rmsg = "No";
                }
                return rmsg;
            }
        }

        public string GenerateInstallment(string sid, string amt, string sdate, string PDate)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                Hashtable htPayment = new Hashtable();
                htPayment.Add("Amount", Convert.ToDecimal(amt));
                htPayment.Add("SaleID", Convert.ToInt32(sid));
                htPayment.Add("Interest", DataValue.AssuredReturnInterest());
                htPayment.Add("StartDate", Convert.ToDateTime(sdate, dtinfo));
                htPayment.Add("PDate", Convert.ToDateTime(PDate, dtinfo));
                htPayment.Add("UserName", User.Identity.Name);
                htPayment.Add("TDSLimit", Convert.ToDecimal(DataValue.AssuredReturnTDSLimit()));
                htPayment.Add("TDSPer", Convert.ToDecimal(DataValue.AssuredReturnTDS()));

                if (obj.ExecuteProcedure("GenerateAssuredReturn_Installment", htPayment))
                {
                    GenerateAssuredReturnAgreement(sid, sdate);
                    return "Installment Generated Successfully.";
                }
                else return "No";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "Error";
            }
        }

        public int GenerateAssuredReturnAgreement(string saleid, string adate)
        {
            string Allotmetdec = "~/Agreement/Allotment/" + saleid + ".htm";
            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleid);
                var Sale = context.SaleFlats.Where(se => se.SaleID == sid).FirstOrDefault();
                var project = context.Projects.Where(pro => pro.ProjectID == Sale.ProjectID).FirstOrDefault();
                var cust = context.Customers.Where(cu => cu.SaleID == Sale.SaleID).FirstOrDefault();
                var installment = context.FlatInstallmentDetails.Where(ins => ins.FlatID == Sale.FlatID).ToList();
                //var planty = context.PlanTypeMasters.Where(pl => pl.PlanID == Sale.PlanID).FirstOrDefault();
                //var plansize = context.tblSPropertySizes.Where(pl => pl.PropertyTypeID == Sale.PropertySizeID).FirstOrDefault();
                var flat = context.Flats.Where(fl => fl.FlatID == Sale.FlatID).FirstOrDefault();
                if (cust.Address1 == null || cust.Address1 == "") cust.Address1 = ".";
                if (cust.CoAddress1 == null || cust.CoAddress1 == "") cust.CoAddress1 = ".";
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
                var AssuredList = context.AssuredReturns.Where(fl => fl.SaleID == Sale.SaleID).ToList();

                string asamt = "0";
                if (AssuredList.Count > 0)
                {
                    asamt = AssuredList[1].Amount.Value.ToString();
                }

                string socAssured = "~/Content/agreement/AssuredReturn.htm";
                string AssuredhtmlDec = "~/Agreement/Assured/" + saleid + ".htm";
                string AssuredDocDec = "~/Agreement/Assured/" + saleid + ".doc";
                // Agreement Letter
                var AssuredContents = System.IO.File.ReadAllText(Server.MapPath(socAssured));
                AssuredContents = AssuredContents.Replace("<% ProjectName %>", project.PName);
                AssuredContents = AssuredContents.Replace("<% OfficeAddress %>", project.OfficeAddress);
                AssuredContents = AssuredContents.Replace("<% AgreementDate %>", adate);
                AssuredContents = AssuredContents.Replace("<% CompanyName %>", project.CompanyName);
                AssuredContents = AssuredContents.Replace("<% PropertyAddress %>", project.Address);
                AssuredContents = AssuredContents.Replace("<% AuthoritySign %>", project.AuthoritySign);
                AssuredContents = AssuredContents.Replace("<% CustomerFatherName %>", cust.PName);
                AssuredContents = AssuredContents.Replace("<% CoAppFatherName %>", cust.SecCoPName);
                AssuredContents = AssuredContents.Replace("<% CustomerFullName %>", cust.AppTitle + "" + cust.FName + " " + cust.MName + " " + cust.LName);
                AssuredContents = AssuredContents.Replace("<% CoAppFullName %>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName);
                AssuredContents = AssuredContents.Replace("<% CoAppAddress %>", cust.CoAddress1 + " " + cust.CoAddress2);
                AssuredContents = AssuredContents.Replace("<% CustomerAddress %>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.Country);
                AssuredContents = AssuredContents.Replace("<% SaleRate %>", Sale.TotalAmount.Value.ToString());
               // AssuredContents = AssuredContents.Replace("<% PlanName %>", planty.PlanTypeName);
                AssuredContents = AssuredContents.Replace("<% PropertyName %>", flat.FlatName);
                //if (plansize != null)
                //    AssuredContents = AssuredContents.Replace("<% PropertySize %>", plansize.Size.Value.ToString() + " " + plansize.Unit);
                AssuredContents = AssuredContents.Replace("<% AssuredAmt %>", asamt);
                clsNW cl = new clsNW();
                AssuredContents = AssuredContents.Replace("<% AssuredAmtInWords %>", cl.rupeestowords(Convert.ToInt64(Math.Round(Convert.ToDecimal(asamt), 0))));
                AssuredContents = AssuredContents.Replace("<% PlanData %>", st);

                System.IO.File.WriteAllText(Server.MapPath(AssuredhtmlDec), AssuredContents); // html aggreement
                System.IO.File.WriteAllText(Server.MapPath(AssuredDocDec), AssuredContents);  // doc agreement
                Agreement ag = new Agreement();
                ag.AssuredDocURL = AssuredDocDec;
                ag.AssuredHTMLURL = AssuredhtmlDec;
                ag.SaleID = sid;
                DocumentService dc = new DocumentService();
                int i = dc.SaveDocument(ag);
                return i;
            }
        }

        public string SearchAssuredReturn(string PID, string status, string chequeDate, string chequeDateTo)
        {
            if (PID == "? undefined:undefined ?" || PID == "All" || PID == "") PID = "0";
            if (status == "? undefined:undefined ?" || status == "All" || status == "") status = "All";
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(PID);
            List<AssuredReturnModel> md = new List<AssuredReturnModel>();

            if (status == "Date") // with Date
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                DateTime dt = new DateTime();
                DateTime dtTo = new DateTime();
                if (chequeDate != "" && chequeDateTo != "")
                {
                    dt = Convert.ToDateTime(chequeDate, dtinfo);
                    dtTo = Convert.ToDateTime(chequeDateTo, dtinfo);
                }

                var model = context.AssuredReturns.Where(a => a.ChequeDate >= dt && a.ChequeDate <= dtTo).AsEnumerable();
                foreach (var v in model)
                {
                    Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                    var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                    if (v.CrDate != null)
                        m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
                    if (v.ChequeDate != null)
                        m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Status == "Updated" || v.Status == "Pending")
                    {
                        m.PayStatus = "show";
                    }
                    else
                    {
                        m.PayStatus = "hidden";
                    }
                    if (v.Status == "Clear")
                    {
                        m.UnClearStatus = "show";
                    }
                    else
                    {
                        m.UnClearStatus = "hidden";
                    }
                    md.Add(m);
                }
            }
            else if (status == "All") // without status
            {
                var model = context.AssuredReturns.AsEnumerable();
                foreach (var v in model)
                {
                    Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                    var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                    if (v.CrDate != null)
                        m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
                    if (v.ChequeDate != null)
                        m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Status == "Updated" || v.Status == "Pending")
                    {
                        m.PayStatus = "show";
                    }
                    else
                    {
                        m.PayStatus = "hidden";
                    }
                    if (v.Status == "Clear")
                    {
                        m.UnClearStatus = "show";
                    }
                    else
                    {
                        m.UnClearStatus = "hidden";
                    }
                    md.Add(m);
                }
            }
            else if (status == "Paid" || status == "Pending" || status == "Cancel") // without status
            {
                var model = context.AssuredReturns.Where(a => a.Status==status).AsEnumerable();
                foreach (var v in model)
                {
                    Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                    var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                    if (v.CrDate != null)
                        m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
                    if (v.ChequeDate != null)
                        m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Status == "Updated" || v.Status == "Pending")
                    {
                        m.PayStatus = "show";
                    }
                    else
                    {
                        m.PayStatus = "hidden";
                    }
                    if (v.Status == "Clear")
                    {
                        m.UnClearStatus = "show";
                    }
                    else
                    {
                        m.UnClearStatus = "hidden";
                    }
                    md.Add(m);
                }
            }
            else // Search with property id
            {
                int FId = Convert.ToInt32(PID);
                int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                var model = context.AssuredReturns.Where(a => a.SaleID == sid).AsEnumerable();
                foreach (var v in model)
                {
                    Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                    var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                    if (v.CrDate != null)
                        m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
                    if (v.ChequeDate != null)
                        m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Status == "Updated" || v.Status == "Pending")
                    {
                        m.PayStatus = "show";
                    }
                    else
                    {
                        m.PayStatus = "hidden";
                    }
                    if (v.Status == "Clear")
                    {
                        m.UnClearStatus = "show";
                    }
                    else
                    {
                        m.UnClearStatus = "hidden";
                    }
                    md.Add(m);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        public string UpdateAssuredChequeClearance(string asid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                int id = Convert.ToInt32(asid);
                var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                model.Status = "Clear";
                model.ModifyBy = User.Identity.Name;
                model.ModifyDate = DateTime.Now;
                context.Entry(model).State = EntityState.Modified;
                int i = context.SaveChanges();
                if (i >= 1)
                {
                    return model.SaleID.Value.ToString();
                }
                else return "No";
            }
        }

        public string UpdateAssuredChequeUnClearance(string asid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                int id = Convert.ToInt32(asid);
                var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                model.Status = "Updated";
                model.ModifyBy = User.Identity.Name;
                model.ModifyDate = DateTime.Now;
                context.Entry(model).State = EntityState.Modified;
                int i = context.SaveChanges();
                if (i >= 1)
                {
                    return model.SaleID.Value.ToString();
                }
                else return "No";
            }
        }
        #endregion
    }
}