using REMS.Data.Access.Sale;
using REMS.Data.CustomModel;
using REMS.Web.App_Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class PaymentDiscountController : Controller
    {
        private PaymentDiscountService pdService;
        private DataFunctions dbContext;
        public PaymentDiscountController()
        {
            pdService = new PaymentDiscountService();
            dbContext = new DataFunctions();
        }
        // GET: Sale/PaymentDiscount
        public ActionResult Index(int id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult AddDiscount(int id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult History()
        {
            return View();
        }

        #region Services
        public string AddPaymentDiscount(PaymentDiscountModel model)
        {
            model.CreatedBy = User.Identity.Name;
            int i= pdService.AddPaymentDiscount(model);
            // Send mail.
            Hashtable ht = new Hashtable();
            ht.Add("PaymentDiscountid", i);
            DataTable dt = dbContext.GetDataTableFromProcedure("spGetEmailByPaymentDiscountID", ht);
            string emails = "";
            foreach(DataRow row in dt.Rows)
            {
                emails = emails + "," + row["Email"].ToString();
            }
            SendMail email = new SendMail();
            string st= email.SendPaymentDiscount(emails, "Admin", model.CustomerName, model.Amount.Value.ToString());
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string AddPaymentDiscountApprove(PaymentDiscountApprovalModel model)
        {
            int i = pdService.AddPaymentDiscountApprove(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetPaymentDiscount(int paymentDiscountid)
        {
            var i = pdService.GetPaymentDiscount(paymentDiscountid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetPaymentDiscountApproveList(int paymentDiscountid)
        {
            var i = pdService.GetPaymentDiscountApproveList(paymentDiscountid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetPaymentDiscountApprove(int paymentDiscountApprovalid)
        {
            var i = pdService.GetPaymentDiscountApprove(paymentDiscountApprovalid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string UpdateStatus(int paymentdiscountAppid, bool status, string remarks, DateTime updated)
        {
            var i = pdService.UpdateStatus(paymentdiscountAppid,status,remarks,updated);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetPDByUserName(string userName,bool isApproved)
        {
            var list = pdService.GetPaymentDisByAdmin(userName, isApproved);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        public string ApproveDiscountPayment(int PaymentDiscountID)
        {
            var i = pdService.ApprovePaymentDiscount(PaymentDiscountID, User.Identity.Name);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string ApproveDiscountPaymentApprove(int PaymentDiscountID,string Remarks,bool IsLast)
        {
            var i = pdService.ApprovePaymentDiscountApprove(PaymentDiscountID, Remarks, User.Identity.Name);
            if (IsLast)
            {
                var model = pdService.ApprovePaymentDiscount(PaymentDiscountID, User.Identity.Name);
                // Update to discount and ledger.
                Hashtable HT = new Hashtable();
                HT.Add("SaleID", model.SaleID);
                HT.Add("InstallmentNo", model.PaymentDiscountID);
                HT.Add("PaymentDate", DateTime.Now);
                HT.Add("Amount", model.Amount);
                HT.Add("PaymentMode", "Discount");
                HT.Add("PaymentStatus", "Clear");
                HT.Add("CustomerName", model.CustomerName);
                HT.Add("Remarks", "Discount");
                HT.Add("AmtRcvdinwords", model.Amount);
                HT.Add("PaymentNo", model.PaymentDiscountID);
                HT.Add("IsReceipt", 0);
                HT.Add("Activity", "Add");
                HT.Add("FlatName", model.FlatNo);
                HT.Add("TransactionID", 0);
                HT.Add("CustomerID", 0);
                HT.Add("CreatedBy", User.Identity.Name);
                HT.Add("PaymentFor", model.PaymentType);

                bool bl= dbContext.ExecuteProcedure("Insert_PaymentDiscount", HT);
                // Send mail to customer.
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }

        public string SearchDiscont(string SearchType,string FlatNo,string DateFrom,string DateTo,string UserName,bool IsApproved)
        {
            if (SearchType == "Flat")
            {
                var model= pdService.GetPaymentDiscountByFlat(FlatNo, IsApproved);
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            else if(SearchType== "RequestDate")
            {
                try
                {
                    DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                    dtinfo.DateSeparator = "/";
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime from = Convert.ToDateTime(DateFrom, dtinfo);
                    DateTime to = Convert.ToDateTime(DateTo, dtinfo);
                    var model = pdService.GetPaymentDiscounByReqDate(from,to, IsApproved);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            else if (SearchType == "ApproveDate")
            {
                try
                {
                    DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                    dtinfo.DateSeparator = "/";
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime from = Convert.ToDateTime(DateFrom, dtinfo);
                    DateTime to = Convert.ToDateTime(DateTo, dtinfo);
                    var model = pdService.GetPaymentDiscountByApproveDate(from, to, IsApproved);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else if(SearchType== "UserName")
            {
                var model = pdService.GetPaymentDisByAdmin(UserName, IsApproved);
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            return "";
        }
        #endregion
    }
}