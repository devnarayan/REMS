using REMS.Data.Access.Report;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.BI.Controllers
{
    public class DemandLetterController : Controller
    {
        private DemandLetterService demandLtrService;
        public DemandLetterController()
        {
            demandLtrService = new DemandLetterService();
        }
        // GET: BI/DemandLetter
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateLetter()
        {
            return View();
        }
        public string SearchDemandLetter(string TowerID, string FlatID, string datefrom)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.SearchDemandLetter(TowerID, FlatID, datefrom));
        }
        public string SaveDimandLetterDimand(string datefrom,string saleid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.SaveDimandLetterDimand(datefrom, saleid));
        }
        public string GetDemandLetter(string searchby,string datefrom,string dateto)
        {
            if (string.IsNullOrEmpty(datefrom)) return null;
            if (string.IsNullOrEmpty(dateto)) return null;
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            DateTime from = Convert.ToDateTime(datefrom, dtinfo);
            DateTime to = Convert.ToDateTime(dateto, dtinfo);
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.GetDemandLetter(searchby,from,to));
        }
        public string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.GanrateDimandLetterDimand(search, propertyid, datefrom, saleid));
        }
        public string DemandLettertPrintReport(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.DemandLettertPrintReport(transactionid));
        }
        public string DemandLettertPrintReport3(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.DemandLettertPrintReport3(transactionid));
        }
        public string DemandLettertPrintReport2(string transactionid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.DemandLettertPrintReport2(transactionid));
        }
        public string ViewSearchDemandLetter(string search, string propertyid, string datefrom)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(demandLtrService.ViewSearchDemandLetter(search, propertyid, datefrom));
        }
    }
}