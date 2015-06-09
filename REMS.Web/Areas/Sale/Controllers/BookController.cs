using REMS.Data;
using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Sale/Book/
        public ActionResult Index()
        {
            return View();
        }
          public ActionResult Booking()
        {
            return View();
        }
          public string BookingSearch(string FlatId, string search)
          {
              
            
              REMSDBEntities context = new REMSDBEntities();

              DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
              dtinfo.DateSeparator = "/";
              dtinfo.ShortDatePattern = "dd/MM/yyyy";
              if (search == "All")
              {
                  var md = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID join C in context.Customers on s.SaleID equals C.SaleID where s.Status == "Booked" select new { Sale = s, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName) });
                  List<FlatSaleModel> model = new List<FlatSaleModel>();
                  foreach (var v in md)
                  {
                      string bdate = "";
                      if (v.Sale.SaleDate != null)
                          bdate = Convert.ToDateTime(v.Sale.SaleDate).ToString("dd/MM/yyyy");
                      model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, FName = v.CustomerName, SaleDate = v.Sale.SaleDate, BookingAmount = v.Sale.TotalAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
                  }
                  return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                  // By default showing last one month sales in all properties
              }
              else
              {
                   int FId = Convert.ToInt32(FlatId);
                    int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                    var md = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID join C in context.Customers on s.SaleID equals C.SaleID where s.SaleID == sid && s.Status == "Booked" select new { Sale = s, FlatName = f.FlatName, CustomerName = (C.FName + " " + C.LName) });
                  List<FlatSaleModel> model = new List<FlatSaleModel>();
                  foreach (var v in md)
                  {
                      string bdate = "";
                      if (v.Sale.SaleDate != null)
                          bdate = Convert.ToDateTime(v.Sale.SaleDate).ToString("dd/MM/yyyy");
                      model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, FName = v.CustomerName,SaleDate = v.Sale.SaleDate, BookingAmount = v.Sale.TotalAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
                  }
                  return Newtonsoft.Json.JsonConvert.SerializeObject(model);

              }
          //    if (proeprtyid == "")
          //    {
          //        // Search nothing.
          //        Failure = "No Property record found.";
          //    }
          //    else if (ptype != "" && psize == "")
          //    {
          //        // Search with type.
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        // var model = context.tblSSaleFlats.Where(p => p.PropertyTypeID == ptyp && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //  return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    else if (ptype != "" && psize != "")
          //    {
          //        //Search with property type and size.
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.PropertySizeID == psiz && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        // var model = context.tblSSaleFlats.Where(p => p.PropertyTypeID == ptyp && p.PropertySizeID == psiz && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.PropertySizeID == psiz && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    else if (ptype != "" && Search != "")
          //    {
          //        dbSBPEntities2 cont = new dbSBPEntities2();
          //        var flt = cont.tblSFlats.Where(f => f.PID == pid && f.FlatName.Contains(Search)).FirstOrDefault();
          //        if (flt != null)
          //        {
          //            // Search with only property type and proeprty name
          //            var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.FlatID == flt.FlatID && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //            List<FlatSaleModel> model = new List<FlatSaleModel>();
          //            foreach (var v in md)
          //            {
          //                string bdate = "";
          //                if (v.Sale.BookingDate != null)
          //                    bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //                model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //            }
          //            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //            // var model = context.tblSSaleFlats.Where(p => p.PropertyTypeID == ptyp && p.FlatID == flt.FlatID && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //            //  return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyTypeID == ptyp && s.FlatID == flt.FlatID && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //        }
          //        else
          //        {
          //            Failure = "Property Name not found.";
          //        }
          //    }
          //    else if (proeprtyid != "" && Search != "")
          //    {
          //        //Search With property id and property name.
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyID == pid && f.FlatName.Contains(Search) && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        //  var model = context.tblSSaleFlats.Where(p => p.PropertyID == pid && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyID == pid && f.FlatName.Contains(Search) && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    else if (proeprtyid == "" && Search != "")
          //    {
          //        //Search With property name.
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where f.FlatName.Contains(Search) && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        //  var model = context.tblSSaleFlats.Where(p => p.PropertyID == pid && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where f.FlatName.Contains(Search) && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    else if (proeprtyid != "")
          //    {
          //        //Search With property id.
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyID == pid && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        //  var model = context.tblSSaleFlats.Where(p => p.PropertyID == pid && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.PropertyID == pid && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    else
          //    {
          //        // seaerch default
          //        DateTime dtf = DateTime.Now.AddMonths(-1);
          //        DateTime dtt = DateTime.Now;
          //        var md = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.BookingDate >= dtf && s.BookingDate <= dtt && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //        List<FlatSaleModel> model = new List<FlatSaleModel>();
          //        foreach (var v in md)
          //        {
          //            string bdate = "";
          //            if (v.Sale.BookingDate != null)
          //                bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //            model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //        }
          //        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
          //        //  var model = context.tblSSaleFlats.Where(p => p.BookingDate <= dtf && p.BookingDate >= dtt && p.PaymentFor.Contains("Booking Payment")).OrderByDescending(o => o.SaleID);
          //        //return Json(new { SList = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.BookingDate >= dtf && s.BookingDate <= dtt && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName }) }, JsonRequestBehavior.AllowGet);
          //    }
          //    DateTime df = DateTime.Now.AddMonths(-1);
          //    DateTime dt = DateTime.Now;

          //    var mmd = (from s in context.tblSSaleFlats join f in context.tblSFlats on s.FlatID equals f.FlatID where s.BookingDate >= df && s.BookingDate <= dt && s.PaymentFor.Contains("Booking Payment") select new { Sale = s, FlatName = f.FlatName });
          //    List<FlatSaleModel> mdl = new List<FlatSaleModel>();
          //    foreach (var v in mmd)
          //    {
          //        string bdate = "";
          //        if (v.Sale.BookingDate != null)
          //            bdate = Convert.ToDateTime(v.Sale.BookingDate).ToString("dd/MM/yyyy");
          //        mdl.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, PropertyID = v.Sale.PropertyID, FName = v.Sale.FName, LName = v.Sale.LName, BookingDate = v.Sale.BookingDate, BookingAmount = v.Sale.BookingAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
          //    }
          //    return Newtonsoft.Json.JsonConvert.SerializeObject(mdl);
          }
	}
}