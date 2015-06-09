using AutoMapper;
using REMS.Data;
using REMS.Data.Access;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class OtherPaymentController : BaseController
    {
        DataFunctions obj = new DataFunctions();
        //
        // GET: /Sale/OtherPayment/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MakeOPayment(int id)
        {
            ViewBag.SaleID = id;
            return View();
        }
        public ActionResult ViewCancelPayments()
        {
            return View();
        }
        public ActionResult BackupReceipt()
        {
            Session["propertyName"] = "Property Name";
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

        public string EditSearchPayment(string FlatId, string paymentfor)
        {
            try
            {
                //string proName = PropertyID;

                //int pid = 0, ptype = 0, psize = 0;
                //if (proName == "? undefined:undefined ?" || proName == "All") proName = "All"; else pid = Convert.ToInt32(proName);
                REMSDBEntities context = new REMSDBEntities();
                // Search by name.
                List<OtherPaymentModel> model = new List<OtherPaymentModel>();
                 int fid = Convert.ToInt32(FlatId);
                if (paymentfor == "All")
                {
                   
                    var model1 = (from sale in
                                      context.SaleFlats
                                  join pay in context.PaymentOthers on sale.SaleID equals pay.SaleID
                                  where sale.FlatID==fid
                                  select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentOtherID, PaymentFor = pay.PaymentFor, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus }).AsEnumerable();
                    foreach (var v in model1)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        // Set Color
                        model.Add(new OtherPaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentOtherID = v.PaymentID, PaymentFor = v.PaymentFor, PaymentNo = v.PaymentNo, PaymentStatus = v.PaymentStatus });
                    }
                }
                else
                {
                    var model1 = (from sale in
                                      context.SaleFlats
                                  join pay in context.PaymentOthers on sale.SaleID equals pay.SaleID
                                  where pay.PaymentFor.Contains(paymentfor) && sale.FlatID==fid
                                  select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentOtherID, PaymentFor = pay.PaymentFor, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus }).AsEnumerable();
                    foreach (var v in model1)
                    {
                        string bdate = "";
                        if (v.PaymentDate != null)
                            bdate = Convert.ToDateTime(v.PaymentDate).ToString("dd/MM/yyyy");
                        // Set Color
                        model.Add(new OtherPaymentModel { PaymentDateSt = bdate, TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentOtherID = v.PaymentID, PaymentFor = v.PaymentFor, PaymentNo = v.PaymentNo, PaymentStatus = v.PaymentStatus });
                    }
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

        public string PaymentCancelSearch(string search, string FlatId, string datefrom, string dateto, string searchtext)
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
                    var md = (from pay in context.PaymentOthers join can in context.PaymentOtherCancels on pay.TransactionID equals can.TransactionID where can.CancelDate >= datef && can.CancelDate <= datet select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    // By default showing last one month sales in all properties
                }
                //else if (search == "SubType")
                //{
                //    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where sale.PropertyTypeID == ptypeid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //    foreach (var v in md)
                //    {
                //        string CDate = "", UCDate = "";
                //        if (v.cancel.CancelDate != null)
                //            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //        if (v.cancel.UnCancelDate != null)
                //            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //    }
                //    return Newtonsoft.Json.JsonConvert.SerializeObject(model);

                //    //var model = context.tblSSaleFlats.Where(p => p.SaleDate >= datef && p.SaleDate <= datet).OrderBy(o => o.SaleID);
                //    //return View(model);
                //}
                else if (search == "FlatName")
                {
                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.FlatName.Contains(searchtext) select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "Customer Name")
                {
                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.CustomerName.Contains(searchtext) select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "CancelDate")
                {

                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);

                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "PaymentDate")
                {
                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);
                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "This Month")
                {
                    DateTime dtFrom = DateTime.Now.AddMonths(-1);
                    DateTime dtTo = DateTime.Now;

                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "Last 7 Days")
                {

                    DateTime dtFrom = DateTime.Now.AddDays(-7);
                    DateTime dtTo = DateTime.Now;

                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else
                {
                    int FId = Convert.ToInt32(FlatId);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where can.SaleID == sid select new { cancel = can, pay = pay, FlatName = pay.FlatName });

                    List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                    foreach (var v in md)
                    {
                        string CDate = "", UCDate = "";
                        if (v.cancel.CancelDate != null)
                            CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                        if (v.cancel.UnCancelDate != null)
                            UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                // }
                //  else // Search by Property id
                //{
                //    if (search == "All")
                //    {
                //        var md = (from can in context.PaymentOtherCancels join pay in context.PaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.SaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "SubType")
                //    {
                //        if (ptypeid != 0) // With PropertyType
                //        {
                //            if (psize == 0) // all Sizes
                //            {
                //                var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && sale.PropertyTypeID == ptypeid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //                List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //                foreach (var v in md)
                //                {
                //                    string CDate = "", UCDate = "";
                //                    if (v.cancel.CancelDate != null)
                //                        CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //                    if (v.cancel.UnCancelDate != null)
                //                        UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //                    model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //                }
                //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //            }
                //            else
                //            {
                //                var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && sale.PropertyTypeID == ptypeid && sale.PropertySizeID == psize select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //                List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //                foreach (var v in md)
                //                {
                //                    string CDate = "", UCDate = "";
                //                    if (v.cancel.CancelDate != null)
                //                        CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //                    if (v.cancel.UnCancelDate != null)
                //                        UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //                    model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //                }
                //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //            }
                //        }
                //        else
                //        {
                //            // All Types of property
                //            if (psize == 0) // All Sizes
                //            {
                //                var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //                List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //                foreach (var v in md)
                //                {
                //                    string CDate = "", UCDate = "";
                //                    if (v.cancel.CancelDate != null)
                //                        CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //                    if (v.cancel.UnCancelDate != null)
                //                        UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //                    model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //                }
                //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //            }
                //            else
                //            {
                //                var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && sale.PropertyTypeID == ptypeid select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //                List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //                foreach (var v in md)
                //                {
                //                    string CDate = "", UCDate = "";
                //                    if (v.cancel.CancelDate != null)
                //                        CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //                    if (v.cancel.UnCancelDate != null)
                //                        UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //                    model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //                }
                //                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //            }

                //        }
                //    }
                //    else if (search == "FlatName")
                //    {
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && pay.FlatName.Contains(searchtext) select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "Customer Name")
                //    {
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && pay.CustomerName.Contains(searchtext) select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "CancelDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "PaymentDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "This Month")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
                //        }
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    }
                //    else if (search == "Last 7 Days")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddDays(-7);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from can in context.tblSPaymentOtherCancels join pay in context.tblSPaymentOthers on can.TransactionID equals pay.TransactionID join sale in context.tblSSaleFlats on can.SaleID equals sale.SaleID where sale.PropertyID == pid && can.CancelDate >= dtFrom && can.CancelDate <= dtTo select new { cancel = can, pay = pay, FlatName = pay.FlatName });
                //        List<PaymentCancelModel> model = new List<PaymentCancelModel>();
                //        foreach (var v in md)
                //        {
                //            string CDate = "", UCDate = "";
                //            if (v.cancel.CancelDate != null)
                //                CDate = v.cancel.CancelDate.Value.ToString("dd/MM/yyyy");
                //            if (v.cancel.UnCancelDate != null)
                //                UCDate = v.cancel.UnCancelDate.Value.ToString("dd/MM/yyyy");
                //            model.Add(new PaymentCancelModel { CancelDateSt = CDate, UnCancelDateSt = UCDate, PaymentCancelID = v.cancel.PaymentOtherCancelID, FlatName = v.FlatName, TransactionID = v.pay.TransactionID, SaleID = v.cancel.SaleID, Amount = v.cancel.Amount, CancelDate = v.cancel.CancelDate, UnCancelDate = v.cancel.UnCancelDate, Remarks = v.cancel.Remarks, UnCancelRemark = v.cancel.UnCancelRemark, CancelBy = v.cancel.CancelBy, UnCancelBy = v.cancel.UnCancelBy, Status = v.cancel.Status, CustomerName = v.pay.CustomerName });
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


        #region GetOPayment Services
        public string GetPaymentbyTID(string transactionid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int tid = Convert.ToInt32(transactionid);
            var model = context.PaymentOthers.Where(t => t.TransactionID == tid).FirstOrDefault();
            Mapper.CreateMap<PaymentOther, OtherPaymentModel>();
            var md = Mapper.Map<PaymentOther, OtherPaymentModel>(model);
            md.PaymentDateSt = model.PaymentDate.Value.ToString("dd/MM/yyyy");
            if (model.ChequeDate != null)
                md.ChequeDateSt = model.ChequeDate.Value.ToString("dd/MM/yyyy");
            if (model.BankClearanceDate != null)
                md.BankClearanceDateSt = model.BankClearanceDate.Value.ToString("dd/MM/yyyy");
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }
        public string GetOtherPaymentMaster()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PaymentMasters.OrderBy(p => p.PaymentMasterID).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        // Get Servitex Charged Details
        public string GetOPaymentServiceTaxCharged(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);
            var payment = context.Payments.Where(p => p.SaleID == sid && p.ServiceTaxAmount > 0).ToList();
            List<PaymentModel> md = new List<PaymentModel>();
            foreach (var pay in payment)
            {
                Mapper.CreateMap<Payment, PaymentModel>();
                var model = Mapper.Map<Payment, PaymentModel>(pay);
                if (model.PaymentDate != null)
                    model.PaymentDateSt = Convert.ToDateTime(model.PaymentDate).ToString("dd/MM/yyyy");
                if (model.ChequeDate != null)
                    model.ChequeDateSt = Convert.ToDateTime(model.ChequeDate).ToString("dd/MM/yyyy");
                if (model.BankClearanceDate != null)
                    model.BankClearanceDateSt = Convert.ToDateTime(model.BankClearanceDate).ToString("dd/MM/yyyy");
                md.Add(model);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }
        // Get Servitex Charged Details
        public string GetOPaymentClearanceCharged(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);
            var payment = context.Payments.Where(p => p.SaleID == sid && p.ClearanceCharge > 0).ToList();
            List<PaymentModel> md = new List<PaymentModel>();
            foreach (var pay in payment)
            {
                Mapper.CreateMap<Payment, PaymentModel>();
                var model = Mapper.Map<Payment, PaymentModel>(pay);
                if (model.PaymentDate != null)
                    model.PaymentDateSt = Convert.ToDateTime(model.PaymentDate).ToString("dd/MM/yyyy");
                if (model.ChequeDate != null)
                    model.ChequeDateSt = Convert.ToDateTime(model.ChequeDate).ToString("dd/MM/yyyy");
                if (model.BankClearanceDate != null)
                    model.BankClearanceDateSt = Convert.ToDateTime(model.BankClearanceDate).ToString("dd/MM/yyyy");
                md.Add(model);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        public string GetOPaymentLatePaymentCharged(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);

            var payment = context.LatePayments.Where(p => p.SaleID == sid).ToList();
            List<LatePaymentModel> md = new List<LatePaymentModel>();
            foreach (var pay in payment)
            {
                Mapper.CreateMap<LatePayment, LatePaymentModel>();
                var model = Mapper.Map<LatePayment, LatePaymentModel>(pay);
                if (model.DueDate != null)
                    model.DueDateSt = Convert.ToDateTime(model.DueDate).ToString("dd/MM/yyyy");
                if (model.CrDate != null)
                    model.CrDateSt = Convert.ToDateTime(model.CrDate).ToString("dd/MM/yyyy");
                md.Add(model);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        /// <summary>
        /// Property Transfer details from one cusotmer to another customer
        /// </summary>
        /// <param name="saleid"></param>
        /// <returns>json stirng </returns>
        public string GetOPaymentPropertyTransferCharged(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);

            var payment = context.PropertyTransfers.Where(p => p.SaleID == sid).ToList();
            List<PropertyTransferModel> md = new List<PropertyTransferModel>();
            foreach (var pay in payment)
            {
                Mapper.CreateMap<PropertyTransfer, PropertyTransferModel>();
                var model = Mapper.Map<PropertyTransfer, PropertyTransferModel>(pay);
                if (model.TransferDate != null)
                    model.TransferDateSt = Convert.ToDateTime(model.TransferDate).ToString("dd/MM/yyyy");
                md.Add(model);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        public string GetOtherPaymentBySaleIDPaymentFor(string saleid, string paymentfor)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);
            var model = context.PaymentOthers.Where(t => t.SaleID == sid && t.PaymentFor == paymentfor).ToList();
            List<OtherPaymentModel> md = new List<OtherPaymentModel>();
            foreach (var pay in model)
            {
                Mapper.CreateMap<PaymentOther, OtherPaymentModel>();
                var md2 = Mapper.Map<PaymentOther, OtherPaymentModel>(pay);
                md2.PaymentDateSt = md2.PaymentDate.Value.ToString("dd/MM/yyyy");
                if (md2.ChequeDate != null)
                    md2.ChequeDateSt = md2.ChequeDate.Value.ToString("dd/MM/yyyy");
                if (md2.BankClearanceDate != null)
                    md2.BankClearanceDateSt = md2.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                md.Add(md2);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(md);
        }

        public string GetOtherPaymentBySP(string flatID)
        {
                    Hashtable ht = new Hashtable();
                    ht.Add("FlatID", flatID);
                    ht.Add("SaleID", 0);
                    DataSet ds = obj.GetDataSETFromProcedure("getOtherpayment", ht);
                  string jst = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                  return jst;
        }

        #endregion

      
        #region Backup Receipt
        public string SearchBackupReceipt(string propertyName, string search, string FlatId, string datefrom, string dateto, string searchtext, string paymenttype)
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

                //if (paymenttype == "? undefined:undefined ?" || paymenttype == "All" || paymenttype == "") paymenttype = "All";
                //if (propertyid == "? undefined:undefined ?" || propertyid == "All" || propertyid == "") propertyid = "0";
                //if (propertySubTypeID == "? undefined:undefined ?" || propertySubTypeID == "All" || propertySubTypeID == "") { propertySubTypeID = "0"; Session["propertyName"] = "Property Name"; }
                //else { Session["propertyName"] = propertyName; }
                //if (proSize == "" || proSize == "? undefined:undefined ?" || proSize == "All") proSize = "0";
                //int pid = Convert.ToInt32(propertyid);
                //int ptypeid = Convert.ToInt32(propertySubTypeID);
                //int psize = Convert.ToInt32(proSize);
                REMSDBEntities context = new REMSDBEntities();
                //if (paymenttype == "All")
                //{
                //    if (propertyid == "0") // All Properties
                //    {
                if (search == "All")
                {
                    DateTime dtFrom = DateTime.Now.AddDays(-7);
                    DateTime dtTo = DateTime.Now;
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });
                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    // By default showing last 7th days in all properties
                }
                //else if (search == "SubType")
                //{
                //    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where sale.PropertyTypeID == ptypeid && sale.PropertySizeID == psize && pay.PaymentNo != "0" select new { pay = pay });

                //    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                //    foreach (var v in md)
                //    {
                //        string bc = "", cd = "";
                //        if (v.pay.BankClearanceDate == null) bc = "";
                //        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                //        if (v.pay.ChequeDate == null) cd = "";
                //        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                //    }
                //    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //    //var model = context.tblSSaleFlats.Where(p => p.SaleDate >= datef && p.SaleDate <= datet).OrderBy(o => o.SaleID);
                //    //return View(model);
                //}
                else if (search == "FlatName")
                {
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.FlatName.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "ReceiptNo")
                {
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentNo.Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });
                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "Customer Name")
                {
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where (pay.CustomerName).Contains(searchtext) && pay.PaymentNo != "0" select new { pay = pay });

                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "PaymentDate")
                {

                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });

                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }

                else if (search == "Last 7 Days")
                {

                    DateTime dtFrom = DateTime.Now.AddDays(-7);
                    DateTime dtTo = DateTime.Now;

                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.PaymentDate >= dtFrom && pay.PaymentDate <= dtTo && pay.PaymentNo != "0" select new { pay = pay });

                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else
                {
                    int FId = Convert.ToInt32(FlatId);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.SaleID == sid && pay.PaymentNo != "0" select new { pay = pay });
                    List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                    foreach (var v in md)
                    {
                        string bc = "", cd = "";
                        if (v.pay.BankClearanceDate == null) bc = "";
                        else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                        if (v.pay.ChequeDate == null) cd = "";
                        else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                        model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
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
                    dt = obj.GetDataTableFromProcedure("Get_PaymentOther", ht);
                    if (dt.Rows.Count > 0)
                    {
                        stStr += @"<tr><td>" + Convert.ToDateTime(dt.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td><td>" + dt.Rows[0]["Amount"] + "</td><td>" + dt.Rows[0]["CustomerName"] + "</td><td>" + dt.Rows[0]["FlatName"] + "</td><td>" + dt.Rows[0]["PaymentNo"] + "</td><td>" + dt.Rows[0]["PaymentMode"] + "</td><td>" + dt.Rows[0]["ChequeNo"] + "</td><td>" + dt.Rows[0]["BankName"] + "</td><td>" + dt.Rows[0]["ChequeDate"] + "</td><td>" + dt.Rows[0]["PaymentStatus"] + "</td></tr>";
                        // stStr += @"     " + Convert.ToDateTime(dt.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy") + "        " + dt.Rows[0]["Amount"] + "        " + dt.Rows[0]["CustomerName"] + "      " + dt.Rows[0]["FlatName"] + "      " + dt.Rows[0]["PaymentNo"] + "     " + dt.Rows[0]["PaymentMode"] + "       " + dt.Rows[0]["ChequeNo"] + "      " + dt.Rows[0]["BankName"] + "      " + dt.Rows[0]["ChequeDate"] + "        " + dt.Rows[0]["PaymentStatus"] + "     \n";
                    }
                }

                string htmlText = GridBody2();
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
                sm.BackupReceiptMail("Payment Receipt from SBP Groups", "", emailid, tfiles);
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
                string[] tids = transids.Split(',');
                REMSDBEntities context = new REMSDBEntities();

                List<PaymentPayDateModel> model = new List<PaymentPayDateModel>();
                foreach (string s in tids)
                {
                    int tid = Convert.ToInt32(s);
                    var pay = context.PaymentOthers.Where(p => p.TransactionID == tid).FirstOrDefault();
                    var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                    int? pid = context.SaleFlats.Where(ss => ss.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                    var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();

                    string bc = "", cd = "";
                    if (pay.BankClearanceDate == null) bc = "";
                    else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    if (pay.ChequeDate == null) cd = "";
                    else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                     //PropertyAddress = Property.Address,PropertyName = Property.PName, PropertyLocation = Property.Location
                    model.Add(new PaymentPayDateModel { Activity = pay.Activity, Amount = pay.Amount, AmtRcvdinWords = pay.AmtRcvdinWords, BankBranch = pay.BankBranch, BankCharges = pay.BankCharges, BankClearanceDate = bc, BankName = pay.BankName, ChequeDate = cd, ChequeNo = pay.ChequeNo, ClearanceCharge = pay.ClearanceCharge, CrDate = pay.CrDate, CreatedBy = pay.CreatedBy, CustomerName = pay.CustomerName, DueAmount = 0, FlatName = pay.FlatName, InstallmentNo = pay.PaymentFor, InterestAmount = 0, IsBounce = pay.IsBounce, IsReceipt = pay.IsReceipt, isrefund = pay.isrefund, ModifyBy = pay.ModifyBy, ModifyDate = pay.ModifyDate, PaymentDate = pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = pay.PaymentOtherID, PaymentMode = pay.PaymentMode, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus, RecordStatus = pay.RecordStatus, RefundRemark = pay.RefundRemark, SaleID = pay.SaleID, ServiceTaxAmount = 0, TotalAmount = pay.Amount, TransactionID = pay.TransactionID, TransferDate = pay.TransferDate, CustomerAddress = cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode, CoCustomerName = cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName, CoCustomerAddress = cust.CoAddress1 + " " + cust.CoAddress2 + " " + cust.CoCity + " " + cust.CoState + " " + cust.CoCountry + " " + cust.CoPinCode, Remarks = pay.Remarks, PropertyAddress = Property.Address, PropertyName = Property.PName, PropertyLocation = Property.Location });
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
                var pay = context.PaymentOthers.Where(p => p.TransactionID == tids).FirstOrDefault();
                //var pop = (from sale in context.tblSSaleFlats join p in context.tblSProperties on sale.PropertyID equals p.PID where sale.SaleID == pay.SaleID select new { PropertyName=p.PName, PID=p.PID, PropertyAddress=p.Address+ " "+p.Village+" "+p.Tehsil }).AsEnumerable();
                var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();

                string bc = "", cd = "";
                if (pay.BankClearanceDate == null) bc = "";
                else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                if (pay.ChequeDate == null) cd = "";
                else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //PropertyAddress = Property.Address,PropertyName = Property.PName, PropertyLocation = Property.Location
                model.Add(new PaymentPayDateModel { Activity = pay.Activity, Amount = pay.Amount, AmtRcvdinWords = pay.AmtRcvdinWords, BankBranch = pay.BankBranch, BankCharges = pay.BankCharges, BankClearanceDate = bc, BankName = pay.BankName, ChequeDate = cd, ChequeNo = pay.ChequeNo, ClearanceCharge = pay.ClearanceCharge, CrDate = pay.CrDate, CreatedBy = pay.CreatedBy, CustomerName = pay.CustomerName, DueAmount = 0, FlatName = pay.FlatName, InstallmentNo = pay.PaymentFor, InterestAmount = 0, IsBounce = pay.IsBounce, IsReceipt = pay.IsReceipt, isrefund = pay.isrefund, ModifyBy = pay.ModifyBy, ModifyDate = pay.ModifyDate, PaymentDate = pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = pay.PaymentOtherID, PaymentMode = pay.PaymentMode, PaymentNo = pay.PaymentNo, PaymentStatus = pay.PaymentStatus, RecordStatus = pay.RecordStatus, RefundRemark = pay.RefundRemark, SaleID = pay.SaleID, ServiceTaxAmount = 0, TotalAmount = pay.Amount, TransactionID = pay.TransactionID, TransferDate = pay.TransferDate, CustomerAddress = cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode, CoCustomerName = cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName, CoCustomerAddress = cust.CoAddress1 + " " + cust.CoAddress2 + " " + cust.CoCity + " " + cust.CoState + " " + cust.CoCountry + " " + cust.CoPinCode, Remarks = pay.Remarks, PropertyAddress = Property.Address, PropertyName = Property.PName, PropertyLocation = Property.Location });
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogExceptionNo(ex, "IME&M", User.Identity.Name);
                return "No";
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
                var md = (from pay in context.PaymentOthers join sale in context.SaleFlats on pay.SaleID equals sale.SaleID where pay.TransactionID == tid select new { pay = pay });
                foreach (var v in md)
                {
                    string bc = "", cd = "";
                    if (v.pay.BankClearanceDate == null) bc = "";
                    else bc = v.pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                    if (v.pay.ChequeDate == null) cd = "";
                    else cd = v.pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                    model.Add(new PaymentPayDateModel { Activity = v.pay.Activity, Amount = v.pay.Amount, AmtRcvdinWords = v.pay.AmtRcvdinWords, BankBranch = v.pay.BankBranch, BankCharges = v.pay.BankCharges, BankClearanceDate = bc, BankName = v.pay.BankName, ChequeDate = cd, ChequeNo = v.pay.ChequeNo, ClearanceCharge = v.pay.ClearanceCharge, CrDate = v.pay.CrDate, CreatedBy = v.pay.CreatedBy, CustomerName = v.pay.CustomerName, FlatName = v.pay.FlatName, InstallmentNo = v.pay.PaymentFor, IsBounce = v.pay.IsBounce, IsReceipt = v.pay.IsReceipt, isrefund = v.pay.isrefund, ModifyBy = v.pay.ModifyBy, ModifyDate = v.pay.ModifyDate, PaymentDate = v.pay.PaymentDate.Value.ToString("dd/MM/yyyy"), PaymentID = v.pay.PaymentOtherID, PaymentMode = v.pay.PaymentMode, PaymentNo = v.pay.PaymentNo, PaymentStatus = v.pay.PaymentStatus, RecordStatus = v.pay.RecordStatus, RefundRemark = v.pay.RefundRemark, SaleID = v.pay.SaleID, TransactionID = v.pay.TransactionID, TransferDate = v.pay.TransferDate });
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
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
                dt = obj.GetDataTableFromProcedure("Get_PaymentOther", ht);
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
        private string GridBody()
        {
            String htmlText = @"     <br />
                <div style='text-align: center'>
<h2>SBP GROUPS</h2>
<p><b>Kharar, Mohali</b></p>
<p> <b>New Delhi</b></p>
<p><b>Ph. 011-43111111</b></p>
<p><b>Fax. 011-43111111</b></p><br/>
<h3>RECEIPT</h3>
                    <br />
                </div>
           <table>
<tr><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>
 <%Data%> 
</table>
<br/>
               
";
            return htmlText;
        }
        private string GridBody2()
        {
            String htmlText = @"     <br />
              
                                 SBP GROUPS
                              Kharar, Mohali   
                                New Delhi   
                             Ph. 011-43111111  
                             Fax. 011-43111111
                             
                                 RECEIPT
                 
  <table>
<tr><td><b>PaymentDate</b></td><td><b>Amount </b></td><td><b>CustomerName</b></td><td><b>Property Name</b></td><td><b>ReceiptNo</b></td><td><b>PaymentMode</b></td><td><b>ChequeNo</b></td><td><b>BankName</b></td><td><b>ChequeDate</b></td><td><b>Status</b></td></tr>
 <%Data%> 
</table>

               
";
            return htmlText;
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
                var pay = context.PaymentOthers.Where(p => p.TransactionID == tids).FirstOrDefault();
                //var pop = (from sale in context.tblSSaleFlats join p in context.tblSProperties on sale.PropertyID equals p.PID where sale.SaleID == pay.SaleID select new { PropertyName=p.PName, PID=p.PID, PropertyAddress=p.Address+ " "+p.Village+" "+p.Tehsil }).AsEnumerable();
                var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
                int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
                var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();
                string bc = "", cd = "";
                if (pay.BankClearanceDate == null) bc = "";
                else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
                if (pay.ChequeDate == null) cd = "";
                else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");
                //Replace("<%PropertyAddress%>", Property.Address).Replace("<%PropertyName%>", Property.PName).Replace("<%PropertyLocation%>", Property.Location
                string htmlText = ReceiptBody();
                htmlText = htmlText.Replace("<%FName%>", pay.CustomerName).Replace("<%FAddress%>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode).Replace("<%CName%>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName).Replace("<%PropertyUnitAddress%>", pay.FlatName).Replace("<%PaymentDetails%>", pay.PaymentMode + " " + pay.ChequeNo + " " + pay.BankName + " " + pay.BankBranch + " " + pay.ChequeDate).Replace("<%InstallmentNo%>", pay.PaymentFor).Replace("<%Amount%>", pay.Amount.Value.ToString()).Replace("<%AmountWord%>", pay.AmtRcvdinWords).Replace("<%PaymentNo%>", pay.PaymentNo).Replace("<%PropertyAddress%>", Property.Address).Replace("<%PropertyName%>", Property.PName).Replace("<%PropertyLocation%>", Property.Location);
                stStr += htmlText;
                REMS.Web.App_Helpers.HtmlToPdfBuilder h2p = new REMS.Web.App_Helpers.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
               // TestPDF.HtmlToPdfBuilder h2p = new TestPDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
                string filename = "Receipt(" + Property.ReceiptPrefix + "_" + pay.FlatName + ")";
               // string filename = "Receipt(" + pay.FlatName + ")";
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
 <p><b>for SBP GROUPS. </b></p>
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
                    Sunita.d
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

        #endregion
    }
}