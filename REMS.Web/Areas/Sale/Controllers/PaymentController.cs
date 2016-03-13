using AutoMapper;
using REMS.Data;
using REMS.Data.Access;
using REMS.Data.Access.Custo;
using REMS.Data.Access.Sale;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using REMS.Web.Models;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Globalization;
using REMS.Data.CustomModel;
using System.Data.Entity;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class PaymentController : BaseController
    {
        DataFunctions obj = new DataFunctions();
        SaleFlatService saleService;
        CustomerService custService;
        PaymentService payService;
        public PaymentController()
        {
            saleService = new SaleFlatService();
            custService = new CustomerService();
            payService = new PaymentService();
        }
        // GET: Index
        [MyAuthorize]
        public ActionResult Index(int? id)
        {
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult OtherPayments()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult Details(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Details(int id,FormCollection collection)
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult Edit(int id)
        {
            ViewBag.ID = id;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            return View();
        }
        // GET: Payment
        [MyAuthorize]
        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Payment(FormCollection collection)
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult Search()
        {
            Session["PropertyName"] = "Property Name";
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            // Get this month records
            datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datet = datef.AddMonths(1);
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            try
            {
                string proName = collection["newCust.PropertyID"];
                string proType = collection["newCust.PropertyTypeID"];
                string proSize = collection["newCust.PropertySizeID"];
                string protypename = collection["protypename"];
                int pid = 0, ptype = 0, psize = 0;
                if (proName == "? undefined:undefined ?" || proName == "All") proName = "All"; else pid = Convert.ToInt32(proName);
                if (proType == "? undefined:undefined ?" || proType == "All") { proType = "All"; Session["PropertyName"] = "Property Name"; } else { ptype = Convert.ToInt32(proType); Session["PropertyName"] = protypename; }
                if (proSize == "? undefined:undefined ?" || proSize == "All") proSize = "All"; else psize = Convert.ToInt32(proSize);
                REMSDBEntities context = new REMSDBEntities();
                string flat = collection["search1"];
                if (flat == "0") // Customer Search.
                {
                    #region Custom Search
                    string srch = collection["search1"];
                    string search = collection["searchby"];
                    string soryby = collection["sortby"];
                    string datefrom = collection["datefrom"];
                    string dateto = collection["dateto"];
                    string searchtext = collection["searchtext"];


                    DateTime datef = new DateTime();
                    DateTime datet = new DateTime();
                    // Date.
                    if (datefrom != "" && dateto != "")
                    {
                        datef = Convert.ToDateTime(datefrom);
                        datet = Convert.ToDateTime(dateto);
                    }
                    if (search == "All")
                    {
                        if (soryby == "All")
                        {
                            // Get this month records
                            datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            datet = datef.AddMonths(1);
                            var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                        else
                        {
                            var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                    }
                    else if (search == "FlatName")
                    {
                        if (soryby == "All")
                        {
                            var model = context.Payments.Where(p => p.FlatName.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                        else
                        {
                            // Date.
                            var model = context.Payments.Where(p => p.FlatName.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                    }
                    else if (search == "Customer Name")
                    {
                        if (soryby == "All")
                        {
                            var model = context.Payments.Where(p => p.CustomerName.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                        else
                        {
                            // Date.
                            var model = context.Payments.Where(p => p.CustomerName.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                    }
                    else if (search == "Cheque No")
                    {
                        if (soryby == "All")
                        {
                            var model = context.Payments.Where(p => p.ChequeNo.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                        else
                        {
                            // Date.
                            var model = context.Payments.Where(p => p.ChequeNo.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                            return View(model);
                        }
                    }
                    else if (search == "This Month")
                    {
                        datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        datet = datef.AddMonths(1);
                        // Date.
                        var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                        return View(model);
                    }
                    else if (search == "Last 7 Days")
                    {
                        datet = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        datef = datet.AddDays(-7);
                        // Date.
                        var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                        return View(model);
                    }
                    else
                    {
                        // Get this month records
                        datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        datet = datef.AddMonths(1);
                        var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                        return View(model);
                    }
                    #endregion
                }
                else
                {
                    //// Property Search.
                    //if (proName != "All" && proType != "All" && proSize != "All")
                    //{
                    //    // Search by property name, type and size
                    //    var model1 = (from sale in
                    //                      context.SaleFlats
                    //                  join pay in context.Payments on sale.SaleID equals pay.SaleID
                    //                  where sale.PropertyID.Value == pid && sale.PropertyTypeID == ptype && sale.PropertySizeID == psize
                    //                  select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();
                    //    List<Payment> py = new List<Payment>();
                    //    foreach (var v in model1)
                    //    {
                    //        py.Add(new tblSPayment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                    //    }
                    //    return View(py);
                    //}
                    //else if (proName != "All" && proType != "All" && proSize == "All")
                    //{
                    //    // Search by Name, and Type.
                    //    var model1 = (from sale in
                    //                      context.SaleFlats
                    //                  join pay in context.Payments on sale.SaleID equals pay.SaleID
                    //                  where sale.PropertyID.Value == pid && sale.PropertyTypeID == ptype
                    //                  select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();
                    //    List<tblSPayment> py = new List<tblSPayment>();
                    //    foreach (var v in model1)
                    //    {
                    //        py.Add(new tblSPayment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                    //    }
                    //    return View(py);
                    //}
                    //else if (proName != "All" && proType == "All" && proSize == "All")
                    //{
                    //    // Search by name.
                    //    var model1 = (from sale in
                    //                      context.SaleFlats
                    //                  join pay in context.Payments on sale.SaleID equals pay.SaleID
                    //                  where sale.PropertyID.Value == pid
                    //                  select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();
                    //    List<tblSPayment> py = new List<tblSPayment>();
                    //    foreach (var v in model1)
                    //    {
                    //        py.Add(new tblSPayment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                    //    }
                    //    return View(py);
                    //}
                }

                return View();
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return View();
            }
        }

        [MyAuthorize]
        public ActionResult EditPayment()
        {
            Session["PropertyName"] = "Property Name";
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            // Get this month records
            datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datet = datef.AddMonths(1);
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
            return View(model);
        }

        [MyAuthorize]
        public ActionResult Clearance(string id)
        {
            ViewBag.ID = id;
            return View();
        }

        [MyAuthorize]
        public ActionResult ViewFlat()
        {
            return View();
        }

        public ActionResult PaymentCancel(int id)
        {
            ViewBag.TransactionID = id;
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PaymentCancels.Where(p => p.TransactionID == id).FirstOrDefault();
            PaymentCancelModel m = new PaymentCancelModel();
            var pay = context.Payments.Where(py => py.TransactionID == id).FirstOrDefault();
            if (model == null)
            {

                m.TransactionID = id;
                m.Status = "New Payment";
                m.Amount = pay.Amount;
                m.FlatName = pay.FlatName;
                m.CustomerName = pay.CustomerName;
                ViewBag.Status = "Cancel Payment";
            }
            else
            {
                Mapper.CreateMap<PaymentCancel, PaymentCancelModel>();
                m = Mapper.Map<PaymentCancel, PaymentCancelModel>(model);
                m.TransactionID = id;
                if (m.Status != "Canceled")
                {
                    ViewBag.Status = "Cancel Payment";
                    m.Amount = pay.Amount;
                }
                else
                {
                    ViewBag.Status = "Undo Cancel Payment";

                }
                m.FlatName = pay.FlatName;
                m.CustomerName = pay.CustomerName;
            }
            return View(m);
        }

        #region payment Service
        public string GetSaleByFlatID(int flatid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(saleService.GetSaleFlatByFlatID(flatid));
        }
        public string GetCustomerbyFlatID(int flatid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(custService.GetCustomerByFlatID(flatid));
        }
        public string GetPaymentList(int saleid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(payService.GetPaymentListBySaleID(saleid));
        }
        public string GetBank()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(payService.GetBank());
        }
        public string InsertPaymentDetails(string InstallmentNo, string Saleid, string Flatname, string DueAmount, string DueDate, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, string Amtrcvdinwrds, string ReceivedAmount, string IsPrint, string IsEmailSent, string EmailTo, string CustomerName, string CustomerID, bool chkInterest,string PaymentType)
        {
            try
            {
                decimal TotalreceivedAmount = Convert.ToDecimal(ReceivedAmount);
                decimal InterestAmount = 0;
                string PaymentNumber = "0";
                int MaxTransactionID = 0;

                MaxTransactionID = Convert.ToInt32(obj.GetMax("TransactionID", "Payment")) + 1;
                bool IsPrintReceipt = Convert.ToBoolean(IsPrint);
                bool IsSentEmail = Convert.ToBoolean(IsEmailSent);

                bool bl = SavePayment(Convert.ToDecimal(DueAmount), Saleid, Flatname, InstallmentNo, InterestAmount, Convert.ToDecimal(ReceivedAmount), PaymentMode, Amtrcvdinwrds, PaymentNumber, MaxTransactionID, ChequeNo, ChequeDate, BankName, BankBranch, Remarks, PayDate, IsPrintReceipt, IsSentEmail, CustomerName, CustomerID, DueDate, chkInterest,PaymentType);

                PrintReceipt re = new PrintReceipt();
                ReceiptModel model = new ReceiptModel();
                model.TransactionID = MaxTransactionID;
                model.ToEmailID = EmailTo;
                string filename = "";
                if (IsPrintReceipt)
                {
                    filename = re.GenerateReceipt(model);
                }
                if (IsSentEmail && filename != "")
                {
                    string Subject = "Receipt Detail";
                    re.SendMailfinal(ConfigurationManager.AppSettings["email"], Subject, model.ToEmailID, model.ToEmailID, filename);
                    // Send email
                }
                if (bl)
                    return filename.Trim('~');
                else return "No";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "No";
            }

        }
        protected bool SavePayment(decimal DueAmount, string Saleid, string FlatName, string InstallmentNo, decimal InterestAmount, decimal PayAmount, string PaymentMode, String Amtrcvdinwrds, String PaymentNo, int TransactionID, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, bool IsPrint, bool IsEmailSent, string CustomerName, string CustomerID, string DueDate, bool chkInterest,string PaymentType)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                Hashtable htPayment = new Hashtable();
                htPayment.Add("InstallmentNo", InstallmentNo);
                htPayment.Add("SaleID", Convert.ToInt32(Saleid));
                htPayment.Add("PaymentDate", Convert.ToDateTime(PayDate, dtinfo));
                if (DueDate == "") DueDate = DateTime.Now.ToString("dd/MM/yyyy");
                htPayment.Add("DueDate", Convert.ToDateTime(DueDate, dtinfo));
                htPayment.Add("DueAmount", DueAmount);
                htPayment.Add("TotalAmount", DueAmount);
                htPayment.Add("Amount", PayAmount);
                htPayment.Add("PaymentMode", PaymentMode);
                if (PaymentMode == "Cash" || PaymentMode == "Transfer Entry")
                {
                    htPayment.Add("PaymentStatus", "Clear");
                }
                else
                {
                    htPayment.Add("PaymentStatus", "Pending");
                    htPayment.Add("ChequeNo", ChequeNo);
                    htPayment.Add("ChequeDate", Convert.ToDateTime(ChequeDate, dtinfo));
                    htPayment.Add("BankName", BankName);
                    htPayment.Add("BankBranch", BankBranch);
                }
                htPayment.Add("CustomerName", CustomerName);
                htPayment.Add("Remarks", Remarks);
                htPayment.Add("AmtRcvdinWords", Amtrcvdinwrds);
                htPayment.Add("Activity", "Add");
                htPayment.Add("PaymentNo", PaymentNo);
                htPayment.Add("IsReceipt", IsPrint);
                htPayment.Add("FlatName", FlatName);
                htPayment.Add("TransactionID", TransactionID);
                htPayment.Add("CustomerID", Convert.ToInt32(CustomerID));
                htPayment.Add("CreatedBy", User.Identity.Name);
                htPayment.Add("Interest", chkInterest);
                htPayment.Add("PaymentType", PaymentType);
                if (obj.ExecuteProcedure("Insert_Payment", htPayment))
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return false;
            }
        }

        #endregion

        #region SearchPayment 
        public string SearchPayment(string search, string Flatid, string datefrom, string dateto, string searchtext)
        {
            try
            {
                DateTime datef = new DateTime();
                DateTime datet = new DateTime();

                // Date.
                if (datefrom != "" && dateto != "")
                {
                    datef = Convert.ToDateTime(datefrom);
                    datet = Convert.ToDateTime(dateto);
                }
                else
                {
                    datef = DateTime.Now.AddMonths(-1);
                    datet = DateTime.Now;
                }

                REMSDBEntities context = new REMSDBEntities();
                if (search == "All")
                {
                    datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    datet = datef.AddMonths(1);
                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                    // By default showing last one month sales in all properties
                }

                else if (search == "Customer Name")
                {
                    var model = context.Payments.Where(p => p.CustomerName.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }
                else if (search == "ReceiptNo")
                {
                    var model = context.Payments.Where(p => p.PaymentNo.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }                
                else if (search == "PaymentDate")
                {
                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }
                else if (search == "This Month")
                {
                    datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    datet = datef.AddMonths(1);
                    // Date.
                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }
                else if (search == "Last 7 Days")
                {

                    datet = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    datef = datet.AddDays(-7);
                    // Date.
                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }
                else
                {
                    int FId = Convert.ToInt32(Flatid);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    var model = context.Payments.Where(p => p.SaleID == sid).OrderByDescending(o => o.PaymentID);
                    List<PaymentModel> model1 = new List<PaymentModel>();
                    foreach (var v in model)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }
        //public string SearchPayment12(string PropertyID, string PropertyTypeID, string PropertySizeID, string PropertyTypeName, string search1, string searchby, string sortby, string datefrom, string dateto, string searchtext)
        //{
        //    try
        //    {
        //        string proName = PropertyID;
        //        string proType = PropertyTypeID;
        //        string proSize = PropertySizeID;
        //        string protypename = PropertyTypeName;
        //        int pid = 0, ptype = 0, psize = 0;
        //        if (proName == "? undefined:undefined ?" || proName == "All") proName = "All"; else pid = Convert.ToInt32(proName);
        //        if (proType == "? undefined:undefined ?" || proType == "All") { proType = "All"; Session["PropertyName"] = "Property Name"; } else { ptype = Convert.ToInt32(proType); Session["PropertyName"] = protypename; }
        //        if (proSize == "? undefined:undefined ?" || proSize == "All") proSize = "All"; else psize = Convert.ToInt32(proSize);
        //        REMSDBEntities context = new REMSDBEntities();
        //        string flat = search1;
        //        if (flat == "0") // Customer Search.
        //        {
        //            #region Custom Search
        //            string srch = search1;
        //            string search = searchby;
        //            string soryby = sortby;


        //            DateTime datef = new DateTime();
        //            DateTime datet = new DateTime();
        //            // Date.
        //            if (datefrom != "" && dateto != "")
        //            {
        //                datef = Convert.ToDateTime(datefrom);
        //                datet = Convert.ToDateTime(dateto);
        //            }
        //            if (search == "All")
        //            {
        //                if (soryby == "All")
        //                {
        //                    // Get this month records
        //                    datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //                    datet = datef.AddMonths(1);
        //                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //                else
        //                {
        //                    var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //            }
        //            else if (search == "ReceiptNo")
        //            {
        //                if (soryby == "All")
        //                {
        //                    var model = context.Payments.Where(p => p.PaymentNo.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //                else
        //                {
        //                    // Date.
        //                    var model = context.Payments.Where(p => p.PaymentNo.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //            }

        //            else if (search == "FlatName")
        //            {
        //                if (soryby == "All")
        //                {
        //                    var model = context.Payments.Where(p => p.FlatName.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //                else
        //                {
        //                    // Date.
        //                    var model = context.Payments.Where(p => p.FlatName.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //            }
        //            else if (search == "Customer Name")
        //            {
        //                if (soryby == "All")
        //                {
        //                    var model = context.Payments.Where(p => p.CustomerName.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //                else
        //                {
        //                    // Date.
        //                    var model = context.Payments.Where(p => p.CustomerName.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //            }
        //            else if (search == "Cheque No")
        //            {
        //                if (soryby == "All")
        //                {
        //                    var model = context.Payments.Where(p => p.ChequeNo.Contains(searchtext)).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //                else
        //                {
        //                    // Date.
        //                    var model = context.Payments.Where(p => p.ChequeNo.Contains(searchtext) && p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                    List<PaymentModel> model1 = new List<PaymentModel>();
        //                    foreach (var v in model)
        //                    {
        //                        string bdate = "";
        //                        if (v.PaymentDate != null)
        //                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                        model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                    }
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //                }
        //            }
        //            else if (search == "This Month")
        //            {
        //                datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //                datet = datef.AddMonths(1);
        //                // Date.
        //                var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                List<PaymentModel> model1 = new List<PaymentModel>();
        //                foreach (var v in model)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //            }
        //            else if (search == "Last 7 Days")
        //            {
        //                datet = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //                datef = datet.AddDays(-7);
        //                // Date.
        //                var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                List<PaymentModel> model1 = new List<PaymentModel>();
        //                foreach (var v in model)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //            }
        //            else
        //            {
        //                // Get this month records
        //                datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //                datet = datef.AddMonths(1);
        //                var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
        //                List<PaymentModel> model1 = new List<PaymentModel>();
        //                foreach (var v in model)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model1.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.SaleID, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model1);
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            // Property Search.
        //            if (proName != "All" && proType != "All" && proSize != "All")
        //            {
        //                // Search by property name, type and size
        //                var model1 = (from sale in
        //                                  context.SaleFlats
        //                              join cust in context.Customers on sale.SaleID equals cust.SaleID
        //                              join pay in context.Payments on sale.SaleID equals pay.SaleID
        //                              where sale.ProjectID.Value == pid
        //                              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = cust.AppTitle + " " + cust.FName + " " + cust.LName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentNo = pay.PaymentNo, PaymentID = pay.PaymentID, PaymentStatus = pay.PaymentStatus }).AsEnumerable();
        //                List<PaymentModel> model = new List<PaymentModel>();
        //                foreach (var v in model1)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //            }
        //            else if (proName != "All" && proType != "All" && proSize == "All")
        //            {
        //                // Search by Name, and Type.
        //                var model1 = (from sale in
        //                                  context.SaleFlats
        //                              join cust in context.Customers on sale.SaleID equals cust.SaleID
        //                              join pay in context.Payments on sale.SaleID equals pay.SaleID
        //                              where sale.ProjectID.Value == pid
        //                              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = cust.AppTitle + " " + cust.FName + " " + cust.LName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentNo = pay.PaymentNo, PaymentID = pay.PaymentID, PaymentStatus = pay.PaymentStatus }).AsEnumerable();
        //                List<PaymentModel> model = new List<PaymentModel>();
        //                foreach (var v in model1)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //            }
        //            else if (proName != "All" && proType == "All" && proSize == "All")
        //            {
        //                // Search by name.
        //                var model1 = (from sale in
        //                                  context.SaleFlats
        //                              join cust in context.Customers on sale.SaleID equals cust.SaleID
        //                              join pay in context.Payments on sale.SaleID equals pay.SaleID
        //                              where sale.ProjectID.Value == pid
        //                              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = cust.AppTitle + " " + cust.FName + " " + cust.LName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentNo = pay.PaymentNo, PaymentID = pay.PaymentID, PaymentStatus = pay.PaymentStatus }).AsEnumerable();
        //                List<PaymentModel> model = new List<PaymentModel>();
        //                foreach (var v in model1)
        //                {
        //                    string bdate = "";
        //                    if (v.PaymentDate != null)
        //                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
        //                    model.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentNo = v.PaymentNo, PaymentID = v.PaymentID, PaymentStatus = v.PaymentStatus });
        //                }
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //            }
        //        }

        //        return Newtonsoft.Json.JsonConvert.SerializeObject("");
        //    }
        //    catch (Exception ex)
        //    {
        //        REMSDBEntities context = new REMSDBEntities();
        //        Failure = "Invalid search query, please try again.";
        //        Helper h = new Helper();
        //        h.LogException(ex);
        //        return Newtonsoft.Json.JsonConvert.SerializeObject("");
        //    }
        //}
        public string EditSearchPayment(int Flatid)
        {
            try
            {

                int pid = 0, ptype = 0, psize = 0;
                REMSDBEntities context = new REMSDBEntities();
                int proid = Convert.ToInt32(Flatid);
                int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == proid select s.SaleID).FirstOrDefault();
                // Search by name.
                var model1 = (from sale in
                                  context.SaleFlats
                              join cust in context.Customers on sale.SaleID equals cust.SaleID
                              join pay in context.Payments on sale.SaleID equals pay.SaleID
                              where sale.SaleID == sid
                              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID, InstallmentNo = pay.InstallmentNo, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus, ChequeDate = pay.ChequeDate, ChequeNo = pay.ChequeNo, BankName = pay.BankName, BranchName = pay.BankBranch, CreatedBy = pay.CreatedBy }).AsEnumerable();
                List<PaymentModel> model = new List<PaymentModel>();
                foreach (var v in model1)
                {
                    string bdate = "";
                    if (v.PaymentDate != null)
                        bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                    string cdate = "";
                    if (v.ChequeDate != null)
                        cdate = Convert.ToDateTime(v.ChequeDate).ToString("dd/MM/yyyy");
                    // Set Color
                    if (v.InstallmentNo == "Advance Booking Amount")
                        model.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID, PaymentNo = v.PaymentNo, InstallmentNo = "red", PaymentStatus = v.PaymentStatus, ChequeDateSt = cdate, BankName = v.BankName, BankBranch = v.BranchName, ChequeNo = v.ChequeNo, CreatedBy = v.CreatedBy });
                    else
                        model.Add(new PaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID, PaymentNo = v.PaymentNo, InstallmentNo = "", PaymentStatus = v.PaymentStatus, ChequeDateSt = cdate, BankName = v.BankName, BankBranch = v.BranchName, ChequeNo = v.ChequeNo, CreatedBy = v.CreatedBy });
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                REMSDBEntities context = new REMSDBEntities();

                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }
              
        #endregion

        #region Editpayment

        public string GetPaymentbyTIDSt(string transactionid)      
        {
            REMSDBEntities context = new REMSDBEntities();
            int tid = Convert.ToInt32(transactionid);
            var pay = context.Payments.Where(p => p.TransactionID == tid).FirstOrDefault();
            if (pay != null)
            {
                Mapper.CreateMap<Payment, PaymentModel>();
                var model = Mapper.Map<Payment, PaymentModel>(pay);
                if (model.PaymentDate != null)
                    model.PaymentDateSt = Convert.ToDateTime(model.PaymentDate).ToString("dd/MM/yyyy");
                if (model.ChequeDate != null)
                    model.ChequeDateSt = Convert.ToDateTime(model.ChequeDate).ToString("dd/MM/yyyy");
                if (model.BankClearanceDate != null)
                    model.BankClearanceDateSt = Convert.ToDateTime(model.BankClearanceDate).ToString("dd/MM/yyyy");
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public JsonResult GetFlatSaleBySaleID(string saleid)
        {
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            var payment = obj.GetDataTable("Select s.SaleID,s.FlatID,s.Aggrement,s.SaleDate, c.* from saleflat s inner join Customer c on s.SaleID=c.SaleID  where s.saleid='" + saleid + "'");
            var v = payment.AsEnumerable().ToList();
            return Json(new { Sale = (from s in v select new { SaleID = s["SaleID"], SaleDate = s["SaleDate"], CustomerName = s["AppTitle"] + " " + s["FName"] + " " + s["MName"] + " " + s["LName"], PName = s["PName"], Mobile = s["MobileNo"], EmailID = s["EmailID"], CustomerID = s["CustomerID"], FlatID = s["FlatID"] }) }, JsonRequestBehavior.AllowGet);
        }

        // GET Total Payment 
        public JsonResult GetTotalPayment(string saleid)
        {
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            var payment = obj.GetDataTable("select sum(Amount) as TotalAmount from Payment where saleid='" + saleid + "'");
            var v = payment.AsEnumerable().ToList();
            return Json(new { TotalPaid = v[0]["TotalAmount"] }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdatePayment
        public EmptyResult UpdatePaymentDetails(string TransactionID, string InstallmentNo, string Saleid, string Flatname, string DueAmount, string DueDate, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, string Amtrcvdinwrds, string ReceivedAmount, string IsPrint)
        {
            decimal TotalreceivedAmount = Convert.ToDecimal(ReceivedAmount);
            decimal InterestAmount = 0;
            string PaymentNumber = "0";
            bool IsPrintReceipt = false;
            if (IsPrint == "")
                IsPrintReceipt = false;
            else
                IsPrintReceipt = Convert.ToBoolean(IsPrint);
            // bool IsSentEmail = Convert.ToBoolean(IsEmailSent);

            if (!String.IsNullOrEmpty(ReceivedAmount))
            {
                UpdatePayment(Convert.ToDecimal(DueAmount), Saleid, Flatname, InstallmentNo, InterestAmount, Convert.ToDecimal(ReceivedAmount), PaymentMode, Amtrcvdinwrds, PaymentNumber, TransactionID, ChequeNo, ChequeDate, BankName, BankBranch, Remarks, PayDate, IsPrintReceipt);
            }
            return new EmptyResult();
        }

        protected void UpdatePayment(decimal DueAmount, string Saleid, string FlatName, string InstallmentNo, decimal InterestAmount, decimal PayAmount, string PaymentMode, String Amtrcvdinwrds, String PaymentNo, string TransactionID, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, bool IsPrint)
        {
            System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            Hashtable htPayment = new Hashtable();
            htPayment.Add("TransactionID", Convert.ToInt32(TransactionID));
            htPayment.Add("SaleID", Convert.ToInt32(Saleid));
            htPayment.Add("PaymentDate", Convert.ToDateTime(PayDate, dtinfo));
            htPayment.Add("Amount", PayAmount);
            htPayment.Add("PaymentMode", PaymentMode);
            if (PaymentMode == "Cash" || PaymentMode == "Transfer Entry")
            {
                htPayment.Add("PaymentStatus", "Clear");
            }
            else
            {
                htPayment.Add("PaymentStatus", "Pending");
                htPayment.Add("ChequeNo", ChequeNo);
                htPayment.Add("ChequeDate", Convert.ToDateTime(ChequeDate, dtinfo));
                htPayment.Add("BankName", BankName);
                htPayment.Add("BankBranch", BankBranch);
            }
            // htPayment.Add("CustomerName", "");
            htPayment.Add("Remarks", Remarks);
            htPayment.Add("AmtRcvdinWords", Amtrcvdinwrds);
            htPayment.Add("Activity", "Edit");
            htPayment.Add("UserName", User.Identity.Name);
            if (obj.ExecuteProcedure("Update_Payment", htPayment))
            {
                string S = "";
            }
        }

        public string UpdateClearanceDetails(string TransactionID, string ChargeAmount, string Remarks, string ClearanceDate, string IsBounce)
        {
            System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            Hashtable htPayment = new Hashtable();
            htPayment.Add("TransactionID", TransactionID);
            htPayment.Add("ChargeAmount", ChargeAmount);
            htPayment.Add("Remarks", Remarks);
            htPayment.Add("ModifyBy", User.Identity.Name);
            htPayment.Add("ClearanceDate", Convert.ToDateTime(ClearanceDate, dtinfo));
            htPayment.Add("IsBounce", IsBounce);
            string st=obj.GetStringValueFromProcedure("Update_Clearance", htPayment);
            if (st=="1")
            {
                return "Yes";
            }
            return "No";
        }
        #endregion       

        #region CancelPayment
        public string PaymentCancelSave(PaymentCancel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {

                    //var payment= context.tblSPaymentCancels.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                    //if(payment!=null)  // Check it is already Canceled or not.
                    //{
                    if (model.Status == "Canceled") // Undo Cancel
                    {
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        var payment = context.Payments.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                        var cpayment = context.PaymentCancels.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                        cpayment.UnCancelBy = User.Identity.Name;
                        cpayment.UnCancelDate = model.CancelDate;
                        cpayment.UnCancelRemark = model.Remarks;
                        cpayment.Status = "Uncanceled";// Canceled/UnCanceled


                        context.Entry(cpayment).State = EntityState.Modified;
                        int i = context.SaveChanges();
                        if (i > 0)
                        {
                            payment.Amount = cpayment.Amount;
                            payment.Activity = "Uncanceled";
                            context.Entry(payment).State = EntityState.Modified;
                            i = context.SaveChanges();
                            if (i > 0)
                                return "Uncanceled";
                            else return "No";
                        }
                        //}
                        return "No";
                    }
                    else if (model.Status == "Uncanceled") //Cancel Existing Transaction.
                    {
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        var payment = context.Payments.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                        var cpayment = context.PaymentCancels.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                        cpayment.CancelBy = User.Identity.Name;
                        cpayment.CancelDate = model.CancelDate;
                        cpayment.Remarks = model.Remarks;
                        cpayment.Status = "Canceled";// Canceled/UnCanceled

                        context.Entry(cpayment).State = EntityState.Modified;
                        int i = context.SaveChanges();
                        if (i > 0)
                        {
                            payment.Amount = 0;
                            payment.Activity = "Canceled";
                            context.Entry(payment).State = EntityState.Modified;
                            i = context.SaveChanges();
                            if (i > 0)
                                return "Canceled";
                            else return "No";
                        }
                        //}
                        return "No";
                    }
                    else // Add New Installment.
                    {
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        var payment = context.Payments.Where(p => p.TransactionID == model.TransactionID).FirstOrDefault();
                        model.SaleID = payment.SaleID;
                        model.Amount = payment.Amount;
                        model.Status = "Canceled";// Canceled/UnCanceled
                        model.CancelBy = User.Identity.Name;
                        model.TransactionID = payment.TransactionID;
                        context.PaymentCancels.Add(model);
                        int i = context.SaveChanges();
                        if (i > 0)
                        {
                            payment.Amount = 0;
                            payment.Activity = "Canceled";
                            context.Entry(payment).State = EntityState.Modified;
                            i = context.SaveChanges();
                            if (i > 0)
                                return "Canceled";
                            else return "No";
                        }
                        //}
                        return "No";
                    }
                }
                catch (Exception ex)
                {
                    Helper h = new Helper();
                    h.LogExceptionNo(ex, "AIMC1", User.Identity.Name);
                    return "No";
                }
            }
        }
        #endregion
        public JsonResult GetFlatList(string pid)
        {
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            var Flat = obj.GetDataTable("Select * from Flat where FlatID='" + pid + "'");
            var v = Flat.AsEnumerable().ToList();

            return Json(new { Result = (from i in v select new { FlatID = i["FlatID"], FlatName = i["FlatName"] }) }, JsonRequestBehavior.AllowGet);
        }

        //Get proptyname and  Pid
        public JsonResult GetPidProptyname(string saleid)
        {
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            var Flat = obj.GetDataTable("SELECT SaleFlat.SaleID AS Expr1, SaleFlat.FlatID , Flat.FlatName FROM  SaleFlat INNER JOIN Flat ON SaleFlat.FlatID = Flat.FlatID where  SaleFlat.SaleID = " + saleid + "");
            var v = Flat.AsEnumerable().ToList();

            return Json(new { Result = (from i in v select new { FlatID = i["FlatID"], FlatName = i["FlatName"] }) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFlatSale(string flatid)
        {
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            var payment = obj.GetDataTable("Select s.SaleID,s.FlatID,s.Aggrement,s.SaleDate, c.* from saleflat s inner join Customer c on s.SaleID=c.SaleID where s.FlatID='" + flatid + "'");
            var v = payment.AsEnumerable().ToList();
            return Json(new { Sale = (from s in v select new { SaleID = s["SaleID"], SaleDate = s["SaleDate"], CustomerName = s["AppTitle"] + " " + s["FName"] + " " + s["MName"] + " " + s["LName"], PName = s["PName"], Mobile = s["MobileNo"], EmailID = s["EmailID"], CustomerID = s["CustomerID"], FlatID = s["FlatID"] }) }, JsonRequestBehavior.AllowGet);
        }

        public string GetTotalCurrentDueAmount(string FlatId, string date)
        {
            string toduamount = "0";
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            DataTable todue = obj.GetDataTable("select  Sum(TotalAmount) as tol from FlatInstallmentDetail where FlatID='" + FlatId + "' and DueDate<='" + Convert.ToDateTime(date, dtinfo) + "'");
            DataTable dtsale = obj.GetDataTable("select * from SaleFlat where FlatID='" + FlatId + "'");
            int saleid = 0;
            if (dtsale.Rows.Count > 0)
            {
                 saleid = Convert.ToInt32(dtsale.Rows[0]["SaleID"]);
            }
            DataRow rw;
            if (todue.Rows.Count > 0)
            {

                DataTable Tbldue = obj.GetDataTable("select sum(Amount) as paidamount  from dbo.Payment where SaleID='" + saleid + "'");
                toduamount = Convert.ToString((Convert.ToDouble(todue.Rows[0]["tol"]) - Convert.ToDouble(Tbldue.Rows[0]["paidamount"])));
                return toduamount;
            }
            else return "0";
        }

        public string GetInstallmentForDue(string saleid)
        {
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            //List<tblSSaleFlat> lstPropertyDetails = new List<tblSSaleFlat>();
            var installment = obj.GetDataTable("select  * from FlatInstallmentDetail where FlatID='" + saleid + "'");
            var v = installment.AsEnumerable().ToList();
            foreach (var install in v)
            {
                if (install["DueDate"].ToString() == "")
                {
                    model.Add(new FlatSaleModel { InstallmentID = install["InstallmentID"].ToString(), DueDate = null, DueDateST = null, TotalAmount = Convert.ToDecimal(install["TotalAmount"]) });
                }
                else
                {
                    model.Add(new FlatSaleModel { InstallmentID = install["InstallmentID"].ToString(), DueDate = Convert.ToDateTime(install["DueDate"]), DueDateST = Convert.ToDateTime(install["DueDate"]).ToString("dd/MM/yyyy"), TotalAmount = Convert.ToDecimal(install["TotalAmount"]) });
                }

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            //return Json(new { Installments = (from s in v select new { InstallmentID = s["InstallmentID"], InstallmentNo = s["InstallmentNo"], DueDate = s["DueDate"], DueAmount = s["DueAmount"], TotalAmount = s["TotalAmount"] }) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOtherPaymentGroupedBySaleID(string sid)
        {

            var Flat = obj.GetDataTable("Select * from vw_tblsPaymentOthers where SaleID='" + sid + "'");
            var v = Flat.AsEnumerable().ToList();

            return Json(new { GroupPay = (from i in v select new { SaleID = i["SaleID"], TAmount = i["TAmount"], PaymentFor = i["PaymentFor"] }) }, JsonRequestBehavior.AllowGet);
        }
        public string InsertOtherPayments(string InstallmentNo, string Saleid, string Flatname, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, string Amtrcvdinwrds, string ReceivedAmount, string IsPrint, string IsEmailSent, string EmailTo, string CustomerName, string CustomerID)
        {
            try
            {
                decimal TotalreceivedAmount = Convert.ToDecimal(ReceivedAmount);
                string PaymentNumber = "0";
                string Msg = "";
                int MaxTransactionID = 0;
                MaxTransactionID = Convert.ToInt32(obj.GetMax("TransactionID", "PaymentOther")) + 1;
                bool IsPrintReceipt = Convert.ToBoolean(IsPrint);
                bool IsSentEmail = Convert.ToBoolean(IsEmailSent);

                try
                {
                    System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    dtinfo.DateSeparator = "/";
                    Hashtable htPayment = new Hashtable();
                    htPayment.Add("SaleID", Convert.ToInt32(Saleid));
                    htPayment.Add("InstallmentNo", InstallmentNo);
                    htPayment.Add("PaymentDate", Convert.ToDateTime(PayDate, dtinfo));
                    htPayment.Add("Amount", Convert.ToDecimal(ReceivedAmount));
                    htPayment.Add("PaymentMode", PaymentMode);
                    if (PaymentMode == "Cash" || PaymentMode == "Transfer Entry")
                    {
                        htPayment.Add("PaymentStatus", "Clear");
                    }
                    else
                    {
                        htPayment.Add("PaymentStatus", "Pending");
                        htPayment.Add("ChequeNo", ChequeNo);
                        htPayment.Add("ChequeDate", Convert.ToDateTime(ChequeDate, dtinfo));
                        htPayment.Add("BankName", BankName);
                        htPayment.Add("BankBranch", BankBranch);
                    }
                    htPayment.Add("CustomerName", CustomerName);
                    htPayment.Add("Remarks", Remarks);
                    htPayment.Add("AmtRcvdinWords", Amtrcvdinwrds);
                    htPayment.Add("Activity", "Add");
                    htPayment.Add("PaymentNo", PaymentNumber);
                    htPayment.Add("IsReceipt", Convert.ToBoolean(IsPrint));
                    htPayment.Add("FlatName", Flatname);
                    htPayment.Add("TransactionID", MaxTransactionID);
                    htPayment.Add("CustomerID", Convert.ToInt32(CustomerID));
                    htPayment.Add("CreatedBy", User.Identity.Name);
                    if (obj.ExecuteProcedure("Insert_PaymentOther", htPayment))
                    {
                        Msg = "Payment Saved";
                    }
                    else
                        Msg = "No";
                }
                catch (Exception ex)
                {
                    Helper h = new Helper();
                    h.LogException(ex);
                    Msg = "Error in Payment Submission";
                }
                PrintReceipt re = new PrintReceipt();
                ReceiptModel model = new ReceiptModel();
                model.TransactionID = MaxTransactionID;
                model.ToEmailID = EmailTo;
                string filename = "";
                if (IsPrintReceipt)
                {
                    filename = re.GenerateReceiptOtherPayment(model, InstallmentNo);
                }
                if (IsSentEmail && filename != "")
                {
                    string Subject = "Receipt Detail";
                    re.SendMailfinal("info@sbpgroups.in", Subject, model.ToEmailID, model.ToEmailID, filename);
                    // Send email
                }
                return filename.Trim('~');
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "No";
            }

        }

        public string GetPaymentMasterList()
        {
            REMSDBEntities dbContext = new REMSDBEntities();
            var model= dbContext.PaymentMasters.ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
    }
}