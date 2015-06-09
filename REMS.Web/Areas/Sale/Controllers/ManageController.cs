using AutoMapper;
using REMS.Data;
using REMS.Data.Access;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class ManageController : BaseController
    {
        DataFunctions obj = new DataFunctions();

        // GET: Installment/Payment
        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        // Get: View and Search Canceled/UnCanceled payments
        [MyAuthorize]
        public ActionResult CancelPayments()
        {
            return View();
        }
        [MyAuthorize]
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
        [MyAuthorize]
        public ActionResult PaymentUnCancel(int id)
        {
            ViewBag.TransactionID = id;
            return View();
        }
        [MyAuthorize]
        public ActionResult RefundPayment()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult BackupReceipt()
        {
            Session["PropertyName"] = "Property Name";
            return View();
        }
        public ActionResult BackupReceiptPrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult BackupReceiptPrintDataAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }

        #region Angular Services
        public string PaymentCancelSave(PaymentCancel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
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
        public string PaymentCancelSearch(string search, string Flatid, string datefrom, string dateto, string searchtext)
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
                    var md = (from pay in context.Payments join can in context.PaymentCancels on pay.TransactionID equals can.TransactionID select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    // By default showing last one month sales in all properties
                }

                else if (search == "Customer Name")
                {
                    var md = (from can in context.PaymentCancels join pay in context.Payments on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.CustomerName.Contains(searchtext) select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "CancelDate")
                {

                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);

                    var md = (from can in context.PaymentCancels join pay in context.Payments on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "PaymentDate")
                {
                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);
                    var md = (from can in context.PaymentCancels join pay in context.Payments on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "This Month")
                {
                    DateTime dtFrom = DateTime.Now.AddMonths(-1);
                    DateTime dtTo = DateTime.Now;

                    var md = (from can in context.PaymentCancels join pay in context.Payments on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "Last 7 Days")
                {

                    DateTime dtFrom = DateTime.Now.AddDays(-7);
                    DateTime dtTo = DateTime.Now;

                    var md = (from can in context.PaymentCancels join pay in context.Payments on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else
                {
                    int FId = Convert.ToInt32(Flatid);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    int pid = Convert.ToInt32(sid);
                    var md = (from pay in context.Payments join can in context.PaymentCancels on pay.TransactionID equals can.TransactionID where can.SaleID == pid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
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
        public string SearchBackupReceiptData(string tids)
        {
            string[] ids = tids.Split(',');
            REMSDBEntities context = new REMSDBEntities();
            List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
            foreach (string id in ids)
            {
                int tid = Convert.ToInt32(id);
                var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.TransactionID == tid select new { pay = pay });
                foreach (var v in md)
                {
                    string bc = "", cd = "";
                    if (v.pay.BankClearanceDate == null) bc = "";
                    else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    if (v.pay.ChequeDate == null) cd = "";
                    else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                    model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        public string SearchBackupReceipt(string search, string FlatId, string datefrom, string dateto, string searchtext)
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
                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentNo != "0" select new { pay = pay });
                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        // By default showing last 7th days in all properties
                    }
                    else if (search == "FlatName")
                    {
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.FlatName.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "ReceiptNo")
                    {
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentNo.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Customer Name")
                    {
                        var md = (from pay in context.Payments join sale in context.Customers on pay.SaleID equals sale.SaleID where pay.CustomerName.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });

                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "PaymentDate")
                    {

                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });

                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "This Month")
                    {
                        datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        datet = datef.AddMonths(1);
                        // Date.
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= datef && pay.PaymentDate <= datet && pay.PaymentNo != "0" select new { pay = pay });

                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Last 7 Days")
                    {

                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;

                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });

                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else
                    {
                        int FId = Convert.ToInt32(FlatId);
                        int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.SaleID == sid && pay.PaymentNo != "0" select new { pay = pay });
                        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                        foreach (var v in md)
                        {
                            string bc = "", cd = "";
                            if (v.pay.BankClearanceDate == null) bc = "";
                            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                            if (v.pay.ChequeDate == null) cd = "";
                            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);

                    }
               // }
               // else // Search by Property id
                //{
                //    if (search == "All")
                //    {
                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.PaymentNo != "0" select new { pay = pay });

                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "FlatName")
                //    {

                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.FlatName.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "ReceiptNo")
                //    {
                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.PaymentNo.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "Customer Name")
                //    {
                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.CustomerName.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });

                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "PaymentDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });
                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "Last 7 Days")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddDays(-7);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from pay in context.Payments join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.ProjectID == pid && pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });
                //        List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //        foreach (var v in md)
                //        {
                //            string bc = "", cd = "";
                //            if (v.pay.BankClearanceDate == null) bc = "";
                //            else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //            if (v.pay.ChequeDate == null) cd = "";
                //            else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, DueAmount = v.pay.DueAmount, FlatName = v.pay.FlatName, InstallmentNo = v.pay.InstallmentNo, InterestAmount = v.pay.InterestAmount, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, ServiceTaxAmount = v.pay.ServiceTaxAmount, TotalAmount = v.pay.TotalAmount, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate, UserID = v.pay.UserID });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //}
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
        public string ExportReceipt(string[] transids)
        {
            try
            {
                string tfile = ExportGrid(transids);
                // Download
                //Response.ContentType = "Application/pdf";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Test_PDF.pdf");
                //Response.TransmitFile(Server.MapPath("~/PDF/Temp/"+dfile));
                //Response.End();
                return tfile;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IMEM", User.Identity.Name);
                return "No";
            }
        }

        public string ExportAndMailReceipt(string[] transids, string emailid)
        {
            // Export and mail excel sheet data.
            try
            {
                DataTable dt = new DataTable();
                string stStr = string.Empty;
                // stStr += stStr + "<table><tr><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>";
                foreach (string tid in transids)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("TransactionID", tid);
                    string l_FileName = "";
                    dt = obj.GetDataTableFromProcedure("Get_Payment", ht);
                    if (dt.Rows.Count > 0)
                    {
                        stStr += @"<tr><td>" + Convert.ToDateTime(dt.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td><td>" + dt.Rows[0]["Amount"] + "</td><td>" + dt.Rows[0]["CustomerName"] + "</td><td>" + dt.Rows[0]["FlatName"] + "</td><td>" + dt.Rows[0]["PaymentNo"] + "</td><td>" + dt.Rows[0]["PaymentMode"] + "</td><td>" + dt.Rows[0]["ChequeNo"] + "</td><td>" + dt.Rows[0]["BankName"] + "</td><td>" + dt.Rows[0]["ChequeDate"] + "</td><td>" + dt.Rows[0]["Activity"] + "</td></tr>";
                    }
                }

                string htmlText = GridBody();
                htmlText = htmlText.Replace("<%Data%>", stStr);
                string filename = "ReceiptData_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + ".xls";
                System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + filename), htmlText);
                //  string tfile = ExportGrid(transids);
                SendMail sm = new SendMail();
                sm.BackupReceiptMailDataFile("Payment Receipt from SBP Groups", "", emailid, filename);
                return "Yes";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IME&M", User.Identity.Name);
                return "No";
            }
        }
        public string BackupReceiptSendMail(string[] transids, string emailid)
        {
            try
            {
                string[] tfiles = GeneratePDFReceipt(transids);
                SendMail sm = new SendMail();
                sm.BackupReceiptMail("Payment Receipt from REMS Property Management", "", emailid, tfiles);
                return "Yes";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IME&M", User.Identity.Name);
                return "No";
            }
        }
        public string BackupReceiptPrint(string transids)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();

                List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                string[] tids = transids.Split(',');
                foreach (string s in tids)
                {
                    int tid = Convert.ToInt32(s);
                    var pay = context.Payments.Where(p => p.TransactionID == tid).FirstOrDefault();
                    var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                    int? pid = context.SaleFlats.Where(ss => ss.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                    var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();

                    string bc = "", cd = "";
                    if (pay.BankClearanceDate == null) bc = "";
                    else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    if (pay.ChequeDate == null) cd = "";
                    else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");

                    model.Add(new PaymentPayDateModel { CompanyName = Property.CompanyName, AuthSign = Property.AuthoritySign, Activity = pay.Activity, Amount = pay.Amount, AmtRcvdinWords = pay.AmtRcvdinWords, BankBranch = pay.BankBranch, BankCharges = pay.BankCharges, BankClearanceDate = bc, BankName = pay.BankName, ChequeDate = cd, ChequeNo = pay.ChequeNo, ClearanceCharge = pay.ClearanceCharge, CrDate = pay.CrDate, CreatedBy = pay.CreatedBy, CustomerName = pay.CustomerName, DueAmount = pay.DueAmount, FlatName = pay.FlatName, InstallmentNo = pay.InstallmentNo, InterestAmount = pay.InterestAmount, IsBounce = pay.IsBounce, IsReceipt = pay.IsReceipt, isrefund = pay.isrefund, ModifyBy = pay.ModifyBy, ModifyDate = pay.ModifyDate, PaymentDate = pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = pay.PaymentID, PaymentMode = pay.PaymentMode, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus, RecordStatus = pay.RecordStatus, RefundRemark = pay.RefundRemark, SaleID = pay.SaleID, ServiceTaxAmount = pay.ServiceTaxAmount, TotalAmount = pay.TotalAmount, TransactionID = pay.TransactionID, TransferDate = pay.TransferDate, UserID = pay.UserID, CustomerAddress = cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode, CoCustomerName = cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName, CoCustomerAddress = cust.CoAddress1 + " " + cust.CoAddress2 + " " + cust.CoCity + " " + cust.CoState + " " + cust.CoCountry + " " + cust.CoPinCode, PropertyAddress = Property.Address, Remarks = pay.Remarks, PropertyName = Property.PName, PropertyLocation = Property.Location });
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IME&M", User.Identity.Name);
                return "No";
            }
        }
        public string BackupReceiptPrintSingle(string tid)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();

                List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                int tids = Convert.ToInt32(tid);
                var pay = context.Payments.Where(p => p.TransactionID == tids).FirstOrDefault();
                //var pop = (from sale in context.tblSSaleFlats join p in context.tblSProperties on sale.ProjectID equals p.PID where sale.SaleID == pay.SaleID select new { PropertyName=p.PName, PID=p.PID, PropertyAddress=p.Address+ " "+p.Village+" "+p.Tehsil }).AsEnumerable();
                var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();

                string bc = "", cd = "";
                if (pay.BankClearanceDate == null) bc = "";
                else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                if (pay.ChequeDate == null) cd = "";
                else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");

                model.Add(new PaymentPayDateModel { Activity = pay.Activity, Amount = pay.Amount, AmtRcvdinWords = pay.AmtRcvdinWords, BankBranch = pay.BankBranch, BankCharges = pay.BankCharges, BankClearanceDate = bc, BankName = pay.BankName, ChequeDate = cd, ChequeNo = pay.ChequeNo, ClearanceCharge = pay.ClearanceCharge, CrDate = pay.CrDate, CreatedBy = pay.CreatedBy, CustomerName = pay.CustomerName, DueAmount = pay.DueAmount, FlatName = pay.FlatName, InstallmentNo = pay.InstallmentNo, InterestAmount = pay.InterestAmount, IsBounce = pay.IsBounce, IsReceipt = pay.IsReceipt, isrefund = pay.isrefund, ModifyBy = pay.ModifyBy, ModifyDate = pay.ModifyDate, PaymentDate = pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = pay.PaymentID, PaymentMode = pay.PaymentMode, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus, RecordStatus = pay.RecordStatus, RefundRemark = pay.RefundRemark, SaleID = pay.SaleID, ServiceTaxAmount = pay.ServiceTaxAmount, TotalAmount = pay.TotalAmount, TransactionID = pay.TransactionID, TransferDate = pay.TransferDate, UserID = pay.UserID, CustomerAddress = cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode, CoCustomerName = cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName, CoCustomerAddress = cust.CoAddress1 + " " + cust.CoAddress2 + " " + cust.CoCity + " " + cust.CoState + " " + cust.CoCountry + " " + cust.CoPinCode, PropertyAddress = Property.Address, Remarks = pay.Remarks, PropertyName = Property.PName, PropertyLocation = Property.Location });
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IME&M", User.Identity.Name);
                return "No";
            }
        }
        #endregion

        #region Helper Methods
        private string ExportGrid(string[] transids)
        {
            DataTable dt = new DataTable();
            string stStr = string.Empty;
            // stStr += stStr + "<table><tr><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>";
            foreach (string tid in transids)
            {
                Hashtable ht = new Hashtable();
                ht.Add("TransactionID", tid);
                string l_FileName = "";
                dt = obj.GetDataTableFromProcedure("Get_Payment", ht);
                if (dt.Rows.Count > 0)
                {
                    stStr += @"<tr><td>" + Convert.ToDateTime(dt.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td><td>" + dt.Rows[0]["Amount"] + "</td><td>" + dt.Rows[0]["CustomerName"] + "</td><td>" + dt.Rows[0]["FlatName"] + "</td><td>" + dt.Rows[0]["PaymentNo"] + "</td><td>" + dt.Rows[0]["PaymentMode"] + "</td><td>" + dt.Rows[0]["ChequeNo"] + "</td><td>" + dt.Rows[0]["BankName"] + "</td><td>" + dt.Rows[0]["ChequeDate"] + "</td><td>" + dt.Rows[0]["Activity"] + "</td></tr>";
                }
            }

            string htmlText = GridBody();
            htmlText = htmlText.Replace("<%Data%>", stStr);
            HtmlToPdfBuilder h2p = new HtmlToPdfBuilder(iTextSharp.text.PageSize.A4_LANDSCAPE);
            string dfile = h2p.HTMLToPdfExportBackupReceiptExcel(htmlText, "PaymentReceipts");
            return dfile;
        }
        private string[] GeneratePDFReceipt(string[] transids)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp\\");
            try
            {
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }
            }
            catch
            { }
            finally
            { }
            DataTable dt = new DataTable();
            string stStr = string.Empty;
            REMSDBEntities context = new REMSDBEntities();
            string[] rid = new string[transids.Length];

            for (int i = 0; i < transids.Length; i++)
            {
                int tids = Convert.ToInt32(transids[i]);
                var pay = context.Payments.Where(p => p.TransactionID == tids).FirstOrDefault();
                //var pop = (from sale in context.tblSSaleFlats join p in context.tblSProperties on sale.ProjectID equals p.PID where sale.SaleID == pay.SaleID select new { PropertyName=p.PName, PID=p.PID, PropertyAddress=p.Address+ " "+p.Village+" "+p.Tehsil }).AsEnumerable();
                var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();
                string bc = "", cd = "";
                if (pay.BankClearanceDate == null) bc = "";
                else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                if (pay.ChequeDate == null) cd = "";
                else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //Property.
                string htmlText = ReceiptBody();
                
                htmlText = htmlText.Replace("<%FName%>", pay.CustomerName).Replace("<%FAddress%>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode).Replace("<%CName%>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName).Replace("<%PropertyUnitAddress%>", pay.FlatName).Replace("<%PaymentDetails%>", pay.PaymentMode + " " + pay.ChequeNo + " " + pay.BankName + " " + pay.BankBranch + " " + pay.ChequeDate).Replace("<%InstallmentNo%>", pay.InstallmentNo).Replace("<%Amount%>", pay.Amount.Value.ToString()).Replace("<%AmountWord%>", pay.AmtRcvdinWords).Replace("<%PaymentNo%>", pay.PaymentNo).Replace("<%PropertyAddress%>", Property.Address).Replace("<%PropertyName%>", Property.PName).Replace("<%PropertyLocation%>", Property.Location).Replace("<%AuthSign%>",Property.AuthoritySign).Replace("<%CompanyName%>",Property.CompanyName);
                stStr += htmlText;
                HtmlToPdfBuilder h2p = new HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
                string filename = "Receipt(" + Property.ReceiptPrefix + "_" + pay.FlatName + ")";
                string dfile = h2p.HTMLToPdfTemp(htmlText, filename);
                rid[i] = filename;
            }
            // TestPDF.HtmlToPdfBuilder h3p = new TestPDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);

            // Merger all pdf.
            //  string tfile = h3p.MergePDFs(transids);

            // delete all pdf.

            // h3p.DeleteAllFiles(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Receipts\\");
            return rid;
        }
        [HttpPost]
        public ActionResult ExportTest(FormCollection collection)
        {
            string ids = collection["hidtid"];
            string[] transids = ids.Split(',');
            DataTable dt = new DataTable();
            string stStr = string.Empty;
            // stStr += stStr + "<table><tr><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>";
            foreach (string tid in transids)
            {
                Hashtable ht = new Hashtable();
                ht.Add("TransactionID", tid);
                string l_FileName = "";
                dt = obj.GetDataTableFromProcedure("Get_Payment", ht);
                if (dt.Rows.Count > 0)
                {
                    stStr += @"<tr><td>" + Convert.ToDateTime(dt.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td><td>" + dt.Rows[0]["Amount"] + "</td><td>" + dt.Rows[0]["CustomerName"] + "</td><td>" + dt.Rows[0]["FlatName"] + "</td><td>" + dt.Rows[0]["PaymentNo"] + "</td><td>" + dt.Rows[0]["PaymentMode"] + "</td><td>" + dt.Rows[0]["ChequeNo"] + "</td><td>" + dt.Rows[0]["BankName"] + "</td><td>" + dt.Rows[0]["ChequeDate"] + "</td><td>" + dt.Rows[0]["Activity"] + "</td></tr>";
                }
            }

            string htmlText = GridBody();
            htmlText = htmlText.Replace("<%Data%>", stStr);
            ExcelExport(htmlText);
            return View();
        }
        public void ExcelExport(string html)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
            "attachment;filename=Receipts.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //GridApprove.AllowPaging = false;
            //GridApprove.DataSource = (DataTable)ViewState["dtapprove"];
            //GridApprove.DataBind();
            //GridApprove.RenderControl(hw);
            hw.Write(html);
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            System.IO.File.WriteAllText(Server.MapPath("~/PDF/df.xls"), html);
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        private string ReceiptBody()
        {
            String htmlText = @"   <div><br />
        <div style='text-align: center'>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
            <h3>RECEIPT: <%PaymentNo%> </h3>
            <br />
        </div>

        Received with thanks from <br />

        First Allottee: <%FName%> <br />
        <%FAddress%>
        <br />

        Co Allottee(s): &nbsp; <%CName%> <br />
        <p>Payment in the respect to Property : <%PropertyUnitAddress%> </p>

        <p> Vide <%PaymentDetails%> </p><br />
        <table style='width:100%;'>
            <tr style='border:1px solid black'><td>Installment Description</td><td>Amount (Rs)</td>
            <tr>
            <tr style='border:1px solid black'><td> <%InstallmentNo%> </td><td> <%Amount%> </td></tr>
            <tr><td> </td> <td>&nbsp;  </td></tr>
        </table>
        Rupees <%AmountWord%><b style='text-align:right; float:right;'>Total Amount: <%Amount%> </b>
        <br />
       <p>Property at: <%PropertyAddress%> , <b> <%PropertyName%> </b><br/>
Location: <%PropertyLocation%>      
</p>
        <table>
            <tr>
                <td>
                    <ul>
                        <li>Receipt is valid subject to realisation of cheque.</li>
                       
                    </ul>
                </td>
                <td>
                   
                </td>
            </tr>
        </table>
 <p><b>for <%CompanyName%>. </b></p>
        <br />
        <br />
        <br />
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <br />
                    (Prepared By)<br />
                    <%AuthSign%>
                </td>
                <td><p><b> </b></p> </td>
            </tr>
        </table>
        <br />

        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
               
";
            return htmlText;
        }
        private string GridBody()
        {
            String htmlText = @"     <br />
                <div style='text-align: center'>
<h2>REMS Property Management</h2>
<h3>RECEIPT</h3>
                    <br />
                </div>
           <table style='border: 2px solid gray;
            width: 100%;'><thead style='border: 1px solid gray;
                background-color: rgb(59, 57, 61);
                padding: 0;
                margin: 0;
                color: rgb(247, 13, 13);'>
<tr style='border-bottom: 1px solid;border-color: gray;'><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>
</thead>
<tbody> 
<%Data%> 
<tbody>
</table>
<br/>
               
";
            return htmlText;
        }
        #endregion
    }
}