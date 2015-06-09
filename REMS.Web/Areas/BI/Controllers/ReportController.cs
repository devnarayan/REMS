using REMS.Data;
using REMS.Data.Access;
using REMS.Data.Access.Report;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.BI.Controllers
{
    public class ReportController : Controller
    {
        ReportServices reportServices = new ReportServices();    
        public ActionResult RefundProperty()
        {
            Session["PropertyName"] = "Property Name";
            return View(reportServices.RefundProperty());
        }
        public ActionResult TransferProperty()
        {
            return View();
        }
        public ActionResult MonthlySale(int? id )
        {
            ViewBag.ID = id;
            return View();
        }
       public ActionResult CancelCheque()
        {
            return View();
        }
       public ActionResult ChequeClearance()
        {
           return View(reportServices.ChequeClearance());
        }
     public ActionResult InstallmentPayment()
        {
            return View();
        }
        
        public ActionResult OtherPayment()
        {
            return View();
        }
    
        public ActionResult CancelChequeOther()
        {
            return View();
        }
      
        public ActionResult ChequeClearanceOther()
        {
            return View();
        }
       
        public ActionResult BrokerList()
        {
            return View();
        }
        public ActionResult BrokerFlatApprove()
        {
            return View();
        }
        public ActionResult BrokerCommission()
        {
            return View();
        }
        public ActionResult AssuredReturn()
        {
            return View();
        }
        public ActionResult PendingInstallment()
        {
            return View();
        }
        public ActionResult PropertyRemark()
        {
            return View();
        }
        public ActionResult DemandLettertPrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLettert2PrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLettert3PrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLetter()
        {
            return View();
        }
        public ActionResult DemandLetterView()
        {
            return View();
        }
        public string RefundPropertySearch(int SaleID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.RefundPropertySearch(SaleID));
        }
        public string RefundPropertySearchByCheque(string search, string FlatId, string datefrom, string dateto, string searchtext)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.RefundPropertySearchByCheque(search, FlatId, datefrom, dateto, searchtext));
        }
        public string TransferPropertySearch( string search,string FlatId, string datefrom, string dateto, string searchtext)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.TransferPropertySearch( search,FlatId,datefrom,  dateto,  searchtext));
        }
        public string SearchPendingInstallment(string search, string FlatId, string datefrom, string dateto, string searchtext)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.SearchPendingInstallment(search, FlatId, datefrom, dateto, searchtext));  
        }
        public string SearchPropertyRemak(string search, string propertyid, string datefrom, string dateto, string searchtext)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.SearchPropertyRemak(search, propertyid, datefrom, dateto, searchtext));  
        }
        public string SearchDemandLetter(string search, string propertyid, string datefrom)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.SearchDemandLetter(search, propertyid,datefrom));  
        }
        public string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.GanrateDimandLetterDimand(search, propertyid, datefrom, saleid));
        }
        public string DemandLettertPrintReport(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.DemandLettertPrintReport(transactionid));
        }
        public string DemandLettertPrintReport3(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.DemandLettertPrintReport3(transactionid));
        }
        public string DemandLettertPrintReport2(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.DemandLettertPrintReport2(transactionid));
        }
        #region ViewdemandReport
        public string ViewSearchDemandLetter(string search, string propertyid, string datefrom)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(reportServices.ViewSearchDemandLetter(search, propertyid, datefrom));
        }
        #endregion
        #region Search Services
        public ActionResult PrintReport()
        {
            return View();
        }
        public string MailReport(string ReportContent, string emailid)
        {
            string filename = "ReportExport.xls";
            System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + filename), ReportContent);
            //  string tfile = ExportGrid(transids);
            SendMail sm = new SendMail();
           sm.BackupReceiptMailDataFile("Report from REMS Groups", "", emailid, filename);
            return "/PDF/Temp/" + filename;
        }
        public string ExportReport(string ReportContent)
        {
            string filename = "ReportExport.xls";
            System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + filename), ReportContent);
            return "/PDF/Temp/" + filename;
        }
        #endregion
    }
}