#region Using

using REMS.Data;
using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using REMS.Data.Access;
using REMS.Web.App_Helpers;

#endregion

namespace REMS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        // GET: home/index
        public ActionResult Index()
        {
           // if(DateTime.Now.Day>=25 && DateTime.Now.Month>=1 && DateTime.Now.Year >= 2016)
           // {
                // Expiry
               // DataFunctions db = new DataFunctions();
              //  db.ExecuteNonQuery("drop procedure spGetDemandLetter");
               // db.ExecuteNonQuery("drop procedure spSaveDemandLetter");
               // db.ExecuteNonQuery("drop procedure Insert_Payment");
              //  return RedirectToAction("Login", "Account", new { area = "" });
           // }
            return View();
        }

        // GET: home/inbox
        public ActionResult AccessDenied()
        {
            return View();
        }

        // GET: home/calendar
        public ActionResult Calendar()
        {
            return View();
        }

        // GET: home/google-map
        public ActionResult GoogleMap()
        {
            return View();
        }

        // GET: home/widgets
        public ActionResult Widgets()
        {
            //[TEST] to initialize the theme setter
            //could be called via jQuery or somewhere...
            Settings.SetValue<string>("config:CurrentTheme", "smart-style-5");

            return View();
        }

        // GET: home/chat
        public ActionResult Chat()
        {
            return View();
        }

        //public string PropertySearch(int SaleID)
        //{
        //    try
        //    {
        //        //DateTime datef = new DateTime();
        //        //DateTime datet = new DateTime();

        //        // Date.
        //        //if (datefrom != "" && dateto != "")
        //        //{
        //        //    datef = Convert.ToDateTime(datefrom);
        //        //    datet = Convert.ToDateTime(dateto);
        //        //}
        //        //else
        //        //{
        //        //    datef = DateTime.Now.AddMonths(-1);
        //        //    datet = DateTime.Now;
        //        //}

        //        int pid = Convert.ToInt32(SaleID);
        //        REMSDBEntities context = new REMSDBEntities();

        //         var md = (from sale in context.SaleFlats
        //                   join f in context.Flats on sale.FlatID equals f.FlatID
        //                   join C in context.Customers on sale.SaleID equals C.SaleID
        //                   where sale.SaleID == pid 
        //                   select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName) });
        //        //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleID == pid && f.Status == "Sale" select new { sale = sale, FlatName = f.FlatName });
        //        List<FlatSaleModel> model = new List<FlatSaleModel>();
        //        foreach (var v in md)
        //        {
        //            string bdate = "";
        //            if (v.sale.SaleDate != null)
        //                bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
        //            model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
        //        }
        //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //        // By default showing last one month sales in all properties




        //        //else // Search by Property id
        //        //{
        //        //    if (search == "All")
        //        //    {
        //        //        var md = (from sale in context.tblSSaleFlats
        //        //                  join f in context.tblSFlats on sale.FlatID equals f.FlatID
        //        //                  join C in context.Customers on sale.CustomerID equals C.CustomerID
        //        //                  where sale.PropertyID == pid && f.Status == 1
        //        //                  select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName) });

        //        //        List<FlatSaleModel> model = new List<FlatSaleModel>();
        //        //        foreach (var v in md)
        //        //        {
        //        //            string bdate = "";
        //        //            if (v.sale.SaleDate != null)
        //        //                bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
        //        //            model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.SaleRate, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = (v.CustomerName), PropertyID = v.sale.PropertyID });
        //        //        }
        //        //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //        //    }

        //        //}
        //        return Newtonsoft.Json.JsonConvert.SerializeObject("");
        //    }
        //    catch (Exception ex)
        //    {
        //        Failure = "Invalid search query, please try again.";
        //        Helper h = new Helper();
        //        h.LogException(ex);
        //        return Newtonsoft.Json.JsonConvert.SerializeObject("");
        //    }
        //}

        public string PropertySearch(string search, string FlatId, string datefrom, string dateto, string searchtext)
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

                if (FlatId == "? undefined:undefined ?") FlatId = "0";

                REMSDBEntities context = new REMSDBEntities();

                if (search == "All")
                {
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              //where sale.SaleDate >= datef && sale.SaleDate <= datet 
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName),Status=f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleDate >= datef && sale.SaleDate <= datet && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });
                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    // By default showing last one month sales in all properties
                }

                else if (search == "Customer Name")
                {
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where (C.FName + "" + C.LName).Contains(searchtext)
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where (sale.FName + " " + sale.LName).Contains(searchtext) && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });

                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "BookingDate")
                {

                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == "Booked"
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.BookingDate >= dtFrom && sale.BookingDate <= dtTo && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });

                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "SaleDate")
                {
                    DateTime dtFrom = Convert.ToDateTime(datefrom);
                    DateTime dtTo = Convert.ToDateTime(dateto);
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == "Sale"
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });

                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });

                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "This Month")
                {
                    DateTime dtFrom = DateTime.Now.AddMonths(-1);
                    DateTime dtTo = DateTime.Now;
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == "Sale"
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });

                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else if (search == "Last 7 Days")
                {

                    DateTime dtFrom = DateTime.Now.AddDays(-7);
                    DateTime dtTo = DateTime.Now;
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == "Sale"
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleDate >= dtFrom && sale.SaleDate <= dtTo && f.Status == 1 select new { sale = sale, FlatName = f.FlatName });

                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else
                {
                    int FId = Convert.ToInt32(FlatId);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    int pid = Convert.ToInt32(sid);
                    var md = (from sale in context.SaleFlats
                              join f in context.Flats on sale.FlatID equals f.FlatID
                              join C in context.Customers on sale.SaleID equals C.SaleID
                              where sale.SaleID == pid 
                              select new { sale = sale, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName), Status = f.Status });
                    //var md = (from sale in context.SaleFlats join f in context.Flats on sale.FlatID equals f.FlatID where sale.SaleID == pid && f.Status == "Sale" select new { sale = sale, FlatName = f.FlatName });
                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    foreach (var v in md)
                    {
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.Flat.SalePrice, Statusrecord = v.Status, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = v.CustomerName });
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

    }
}