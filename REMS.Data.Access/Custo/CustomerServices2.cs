using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
    public interface ICustomerServices
    {
      string[] CheckInstallmentStatus(int pID, string pName);
    }
    public class CustomerServices2
    {
        public string[] CheckInstallmentStatus(int pID, string pName)
        {
            try
            {
                using (REMSDBEntities context = new REMSDBEntities())
                {
                    var model = context.AssuredReturns.Where(ar => ar.PropertyID == pID && ar.FlatName == pName).FirstOrDefault();
                    if (model == null)
                    {
                        var fId = from ft in context.Flats
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tr in context.Towers on fr.TowerID equals tr.TowerID
                                  where (tr.ProjectID == pID && ft.FlatName == pName)
                                  select new { ft.FlatID };
                        var md = context.SaleFlats.Where(s => s.FlatID == fId.ToList()[0].FlatID ).FirstOrDefault();//&& s.PlanID == 5
                        if (md == null)
                        {
                            string[] st = new string[2];
                            st[0] = "No";
                            st[1] = "No Found";
                            return st;
                        }
                        else
                        {
                            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
                            var amount = context.Payments.Where(p => p.SaleID == md.SaleID).ToList();
                            if (string.IsNullOrEmpty(Convert.ToString(amount.ToList()[0].Amount)))
                            {
                                string[] st = new string[2];
                                st[0] = "LessPayment";
                                st[1] = "Payment not submitted 50%";
                                return st;
                            }
                            else
                            {
                                decimal sRate = (decimal)md.TotalAmount;
                                if (amount.ToList()[0].Amount >= sRate / 2)
                                {
                                    string[] st = new string[2];
                                    st[0] = md.SaleID.ToString();
                                    int sid = Convert.ToInt32(md.SaleID);
                                    decimal? amt = context.FlatInstallmentDetails.Where(ins => ins.SaleID == sid).OrderBy(o => o.InstallmentID).FirstOrDefault().TotalAmount;
                                    st[1] = amount.ToList()[0].Amount.ToString();
                                    return st;
                                }
                                else
                                {
                                    string[] st = new string[2];
                                    st[0] = "LessPayment";
                                    st[1] = "Payment not submitted 50%";
                                    return st;
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] st = new string[2];
                        st[0] = "Yes";
                        st[1] = model.SaleID.Value.ToString();
                        return st;
                    }
                }
            }
            catch (Exception ex)
            {
                string[] st = new string[2];
                st[0] = "Error";
                st[1] = ex.ToString();
                return st;
            }
        }
        public string GenerateInstallment(int sId, decimal amt, string sDate, string pDate,string userName)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                if (context.GenerateAssuredReturn_Installment(sId, amt, DataValue.AssuredReturnInterest(), Convert.ToDateTime(sDate), Convert.ToDateTime(pDate), userName, DataValue.AssuredReturnTDSLimit(), DataValue.AssuredReturnTDS()) == 1)
                {
                    return "Installment Generated Successfully.";
                }
                else
                {
                    return "No";
                }
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "Error";
            }
        }
        //will be done when change on controller.
//        public int GenerateAssuredReturnAgreement(string saleid, string adate)
//        {
//            string Allotmetdec = "~/Agreement/Allotment/" + saleid + ".htm";
//            // To copy a file to another location and 
//            // overwrite the destination file if it already exists.
//            using (dbSBPEntities2 context = new dbSBPEntities2())
//            {
//                int sid = Convert.ToInt32(saleid);
//                var Sale = context.tblSSaleFlats.Where(se => se.SaleID == sid).FirstOrDefault();
//                var project = context.tblSProperties.Where(pro => pro.PID == Sale.PropertyID).FirstOrDefault();
//                var cust = context.Customers.Where(cu => cu.CustomerID == Sale.CustomerID).FirstOrDefault();
//                var installment = context.tblSInstallmentDetails.Where(ins => ins.SaleID == sid).ToList();
//                var planty = context.PlanTypes.Where(pl => pl.PlanID == Sale.PlanID).FirstOrDefault();
//                var plansize = context.tblSPropertySizes.Where(pl => pl.PropertyTypeID == Sale.PropertySizeID).FirstOrDefault();
//                var flat = context.tblSFlats.Where(fl => fl.FlatID == Sale.FlatID).FirstOrDefault();
//                if (cust.Address1 == null || cust.Address1 == "") cust.Address1 = ".";
//                if (cust.CoAddress1 == null || cust.CoAddress1 == "") cust.CoAddress1 = ".";
//                string st = "";
//                foreach (var inst in installment)
//                {
//                    st += @" <tr><td width='321' style='width:240.95pt;border:solid black 1.0pt; padding: 3pt 5.4pt 3pt 5.4pt; font-size:12.0pt;font-family:Arial,sans-serif'><p>";
//                    st += inst.InstallmentNo.ToString();
//                    st += @"</p></td><td width='18' style='width:13.5pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'>
//                    <p class='MsoNormal'>:</p>
//                </td><td width='150' style='width:125pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; padding:0in 5.4pt 0in 5.4pt;'><p class='MsoNormal'>";
//                    if (inst.DueDate != null)
//                        st += inst.DueDate.Value.ToString("dd/MM/yyyy");
//                    st += @"</p></td>
//                <td width='301' style='width:225.95pt;border:solid black 1.0pt;  border-left:none;  padding: 3pt 5.4pt 3pt 5.4pt;'>
//                    <p class='MsoNormal' style='margin-right:-81.0pt;  font-size:12.0pt;font-family:Arial,sans-serif'> ";
//                    st += inst.TotalAmount.Value.ToString();
//                    st += @"</p>
//                </td>
//                </tr>";
//                }
//                var AssuredList = context.AssuredReturns.Where(fl => fl.SaleID == Sale.SaleID).ToList();

//                string asamt = "0";
//                if (AssuredList.Count > 0)
//                {
//                    asamt = AssuredList[1].Amount.Value.ToString();
//                }

//                string socAssured = "~/Content/agreement/AssuredReturn.htm";
//                string AssuredhtmlDec = "~/Agreement/Assured/" + saleid + ".htm";
//                string AssuredDocDec = "~/Agreement/Assured/" + saleid + ".doc";
//                // Agreement Letter
//                var AssuredContents = System.IO.File.ReadAllText(Server.MapPath(socAssured));
//                AssuredContents = AssuredContents.Replace("<% ProjectName %>", project.PName);
//                AssuredContents = AssuredContents.Replace("<% OfficeAddress %>", project.OfficeAddress);
//                AssuredContents = AssuredContents.Replace("<% AgreementDate %>", adate);
//                AssuredContents = AssuredContents.Replace("<% CompanyName %>", project.CompanyName);
//                AssuredContents = AssuredContents.Replace("<% PropertyAddress %>", project.Address);
//                AssuredContents = AssuredContents.Replace("<% AuthoritySign %>", project.AuthoritySign);
//                AssuredContents = AssuredContents.Replace("<% CustomerFatherName %>", cust.PName);
//                AssuredContents = AssuredContents.Replace("<% CoAppFatherName %>", cust.SecCoPName);
//                AssuredContents = AssuredContents.Replace("<% CustomerFullName %>", cust.AppTitle + "" + cust.FName + " " + cust.MName + " " + cust.LName);
//                AssuredContents = AssuredContents.Replace("<% CoAppFullName %>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName);
//                AssuredContents = AssuredContents.Replace("<% CoAppAddress %>", cust.CoAddress1 + " " + cust.CoAddress2);
//                AssuredContents = AssuredContents.Replace("<% CustomerAddress %>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.Country);
//                AssuredContents = AssuredContents.Replace("<% SaleRate %>", Sale.SaleRate.Value.ToString());
//                AssuredContents = AssuredContents.Replace("<% PlanName %>", planty.PlanTypeName);
//                AssuredContents = AssuredContents.Replace("<% PropertyName %>", flat.FlatName);
//                if (plansize != null)
//                    AssuredContents = AssuredContents.Replace("<% PropertySize %>", plansize.Size.Value.ToString() + " " + plansize.Unit);
//                AssuredContents = AssuredContents.Replace("<% AssuredAmt %>", asamt);
//                AssuredContents = AssuredContents.Replace("<% AssuredAmtInWords %>", cl.rupeestowords(Convert.ToInt64(Math.Round(Convert.ToDecimal(asamt), 0))));
//                AssuredContents = AssuredContents.Replace("<% PlanData %>", st);

//                System.IO.File.WriteAllText(Server.MapPath(AssuredhtmlDec), AssuredContents); // html aggreement
//                System.IO.File.WriteAllText(Server.MapPath(AssuredDocDec), AssuredContents);  // doc agreement
//                Agreement ag = new Agreement();
//                ag.AssuredDocURL = AssuredDocDec;
//                ag.AssuredHTMLURL = AssuredhtmlDec;
//                ag.SaleID = sid;
//                DocumentService dc = new DocumentService();
//                int i = dc.SaveDocument(ag);
//                return i;
//            }
//        }
        public List<AssuredReturnModel> SearchAssuredReturn(string PId, string propertyName, string status, string chequeDate, string chequeDateTo)
        {
            if (PId == "? undefined:undefined ?" || PId == "All" || PId == "") PId = "0";
            if (status == "? undefined:undefined ?" || status == "All" || status == "") status = "All";
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(PId);
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
                var model = context.AssuredReturns.Where(a => a.FlatName.Contains(propertyName) && a.PropertyID == id).AsEnumerable();
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
                var model = context.AssuredReturns.Where(a => a.PropertyID == id && a.Status.Contains(status) && a.FlatName.Contains(propertyName)).AsEnumerable();
                foreach (var v in model)
                {
                    Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
                    var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(v);
                    if (v.CrDate != null)
                        m.CrDateSt = v.CrDate.Value.ToString("dd/MM/yyyy");
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
            return md;
        }
        public List<AssuredReturnModel> GetAssuredReturnBySaleID(string saleid)
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
            return md;
        }

        public string UpdateAssuredCheque(string asid, string chequeNo, string chequeDate,string userName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                if (chequeDate != null && chequeDate != "")
                {
                    DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                    dtinfo.DateSeparator = "/";
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    int id = Convert.ToInt32(asid);
                    DateTime dt = Convert.ToDateTime(chequeDate, dtinfo);
                    var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                    model.ChequeNo = chequeNo;
                    model.ChequeDate = dt;
                    model.Status = "Updated";
                    model.ModifyBy = userName;
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
            return "No";
        }
        public string UpdateAssuredChequeAll(string[] chequeNos, string[] chequeDates, string[] aSids,string userName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                string[] cno = chequeNos;
                string[] cdate = chequeDates;
                string[] asid = aSids;
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
                        model.ModifyBy = userName;
                        model.ModifyDate = DateTime.Now;
                        context.Entry(model).State = EntityState.Modified;
                        int ii = context.SaveChanges();
                        if (ii >= 1)
                        {
                            rmsg = model.SaleID.Value.ToString();
                        }
                        
                    }
                    
                }
                return rmsg;
            }
        }
        public string UpdateAssuredChequeClearance(string asId,string userName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                int id = Convert.ToInt32(asId);
                var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                model.Status = "Clear";
                model.ModifyBy = userName;
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
        public string UpdateAssuredChequeUnClearance(string asId,string userName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                int id = Convert.ToInt32(asId);
                var model = context.AssuredReturns.Where(rtn => rtn.AssuredReturnID == id).FirstOrDefault();
                model.Status = "Updated";
                model.ModifyBy = userName;
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
        public List<AssuredReturnPayment> SearchAssuredPayment(string PID, string propertyname, string status)
        {
            if (PID == "? undefined:undefined ?" || PID == "All" || PID == "") PID = "0";
            if (status == "? undefined:undefined ?" || status == "All" || status == "") status = "All";
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(PID);
            List<AssuredReturnPayment> md = new List<AssuredReturnPayment>();

            if (status == "All") // without status
            {
                var mdl = (from pay in context.AssuredReturnPayments join asd in context.AssuredReturns on pay.SaleID equals asd.SaleID where pay.FlatName.Contains(propertyname) select new { Pay = pay, Sale = asd }).AsEnumerable();
                foreach (var v in mdl)
                {
                    AssuredReturnPaymentModel model = new AssuredReturnPaymentModel();
                    Mapper.CreateMap<AssuredReturnPayment, AssuredReturnPaymentModel>();
                    model = Mapper.Map<AssuredReturnPayment, AssuredReturnPaymentModel>(v.Pay);
                    model.Amount = Math.Round(model.Amount.Value, 2);

                    if (v.Pay.PaymentDate != null)
                        model.PaymentDateSt = v.Pay.PaymentDate.Value.ToString("dd/MM/yyyy");
                    if (v.Pay.ChequeDate != null)
                        model.ChequeDateSt = v.Pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Pay.BankClearanceDate != null)
                        model.BankClearanceDateSt = v.Pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    //md.Add(model);
                }
            }
            else // Search with property id
            {
                var mdl = (from pay in context.AssuredReturnPayments join asd in context.AssuredReturns on pay.SaleID equals asd.SaleID where pay.FlatName.Contains(propertyname) && asd.PropertyID == id select new { Pay = pay, Sale = asd }).AsEnumerable();
                foreach (var v in mdl)
                {
                    AssuredReturnPaymentModel model = new AssuredReturnPaymentModel();
                    Mapper.CreateMap<AssuredReturnPayment, AssuredReturnPaymentModel>();
                    model = Mapper.Map<AssuredReturnPayment, AssuredReturnPaymentModel>(v.Pay);
                    model.Amount = Math.Round(model.Amount.Value, 2);
                    if (v.Pay.PaymentDate != null)
                        model.PaymentDateSt = v.Pay.PaymentDate.Value.ToString("dd/MM/yyyy");
                    if (v.Pay.ChequeDate != null)
                        model.ChequeDateSt = v.Pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                    if (v.Pay.BankClearanceDate != null)
                        model.BankClearanceDateSt = v.Pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    //md.Add(model);
                }
            }
            return md;
        }

        public string SaveAssuredReturnPayment(string saleId, string flatName, string paymentMode, string chequeNo, string chequeDate, string bankName, string bankBranch, string remarks, string payDate, string amtrcvdInWrds, string receivedAmount, string isPrint, string isEmailSent, string emailTo, string customerName, string customerID, string assuredID,string userName)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                int sId = Convert.ToInt32(saleId);
                int asId = Convert.ToInt32(assuredID);
                var mdl = context.AssuredReturnPayments.Where(pay => pay.SaleID == sId && pay.AssuredReturnID == asId).FirstOrDefault();
                if (mdl == null)
                {
                    decimal TotalreceivedAmount = Convert.ToDecimal(receivedAmount);
                    string paymentNumber = "0";
                    int MaxTransactionID = 0;
                    string Msg = "";
                    MaxTransactionID = context.AssuredReturnPayments.Max(p => p.TransactionID).Value+1;
                    bool isPrintReceipt = Convert.ToBoolean(isPrint);
                    bool isSentEmail = Convert.ToBoolean(isEmailSent);
                    string paymentStatus;
                   if (paymentMode == "Cash" || paymentMode == "Transfer Entry")
                        {
                           paymentStatus= "Clear";
                        }
                        else
                        {
                        paymentStatus= "Pending";
                   }
                   try
                   {
                       System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
                       dtinfo.ShortDatePattern = "dd/MM/yyyy";
                       if (context.Insert_PaymentAssuredReturn(sId, Convert.ToDateTime(payDate, dtinfo), TotalreceivedAmount, paymentMode, chequeNo, Convert.ToDateTime(chequeDate), bankName, paymentStatus, customerName, remarks, bankBranch, amtrcvdInWrds, paymentNumber, isPrintReceipt, flatName, MaxTransactionID, Convert.ToInt32(customerID), userName, asId) == 1)
                       {
                           Msg = "Yes";
                       }
                       else
                       {
                           Msg = "Error: Payment not saved";
                       }
                   }
                   catch (Exception ex)
                   {
                       Helper h = new Helper();
                       h.LogException(ex);
                       Msg = "Error in Payment Submission";
                   }
                    //--if (Msg == "Yes") this code will go on controller
                    //{

                    //    PrintReceipt re = new PrintReceipt();
                    //    ReceiptModel model = new ReceiptModel();
                    //    model.ToEmailID = EmailTo;
                    //    string filename = "";
                    //    if (IsPrintReceipt)
                    //    {
                    //        //filename = re.GenerateReceiptOtherPayment(model);
                    //    }
                    //    if (IsSentEmail && filename != "")
                    //    {
                    //        string Subject = "Receipt Detail";
                    //        re.SendMailfinal("info@sbpgroups.in", Subject, model.ToEmailID, model.ToEmailID, filename);
                    //        // Send email
                    //    }
                    //    return filename.Trim('~');
                    //}
                    //else return "No";
                    return Msg;
                }
                else
                {
                    return "Found";
                }
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "No";
            }
        }
        public AssuredReturnModel GetAssuredReturnByID(string id)
        {
            REMSDBEntities context = new REMSDBEntities();
            int asId = Convert.ToInt32(id);
            var model = context.AssuredReturns.Where(a => a.AssuredReturnID == asId).FirstOrDefault();
            Mapper.CreateMap<AssuredReturn, AssuredReturnModel>();
            var m = Mapper.Map<AssuredReturn, AssuredReturnModel>(model);
            if (model.CrDate != null)
                m.CrDateSt = model.CrDate.Value.ToString("dd/MM/yyyy");
            return m;
        }
        public List<AssuredReturnPaymentModel> GetPayment(string saleId)
        {
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(saleId);
            List<AssuredReturnPaymentModel> md = new List<AssuredReturnPaymentModel>();
            var model = context.AssuredReturnPayments.Where(a => a.SaleID == id).AsEnumerable();
            foreach (var v in model)
            {
                Mapper.CreateMap<AssuredReturnPayment, AssuredReturnPaymentModel>();
                var m = Mapper.Map<AssuredReturnPayment, AssuredReturnPaymentModel>(v);
                if (v.PaymentDate != null)
                    m.PaymentDateSt = v.PaymentDate.Value.ToString("dd/MM/yyyy");
                if (v.ChequeDate != null)
                    m.ChequeDateSt = v.ChequeDate.Value.ToString("dd/MM/yyyy");
                if (v.BankClearanceDate != null)
                    m.BankClearanceDateSt = v.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                md.Add(m);
            }
            return md;
        }
    }
}
