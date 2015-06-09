using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Report
{
    interface IReportServices
    {
        List<FlatSaleModel> RefundProperty();
        IQueryable<Payment> ChequeClearance();
       // List<RefundPropertyModel> RefundPropertySearch(string propertyName, string search, string propertyid, string propertySubTypeID, string proSize, string datefrom, string dateto, string searchtext);
        List<RefundPropertyModel> RefundPropertySearchByCheque(string search, string FlatId, string datefrom, string dateto, string searchtext);
        List<TransferPropertyModel> TransferPropertySearch(string search,string FlatId, string datefrom, string dateto, string searchtext);
        List<FlatSaleModel> SearchPendingInstallment(string search, string propertyid, string datefrom, string dateto, string searchtext);
        List<FlatSaleModel> SearchDemandLetter(string search, string propertyid, string datefrom);
        string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid);
        List<FlatSaleModel> DemandLettertPrintReport3(string transactionid);
        List<FlatSaleModel> DemandLettertPrintReport2(string transactionid);
        List<FlatDemandLetter> ViewSearchDemandLetter(string search, string propertyid, string datefrom);
    }
    public class ReportServices : IReportServices
    {
        public List<FlatSaleModel> RefundProperty()
        {
            REMSDBEntities context = new REMSDBEntities();
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            datef = DateTime.Now.AddMonths(-1);
            datet = DateTime.Now;
            var md = (from sale in context.SaleFlats
                      join f in context.Flats on sale.FlatID
                          equals f.FlatID
                      join cust in context.Customers on sale.SaleID equals cust.SaleID
                      join fr in context.Floors on f.FloorID equals fr.FloorID
                      join tw in context.Towers on fr.TowerID equals tw.TowerID
                      where sale.SaleDate >= datef && sale.SaleDate <= datet
                      select new { sale = sale, cust = cust, FlatName = f.FlatName, tw = tw });
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            foreach (var v in md)
            {
                model.Add(new FlatSaleModel { FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.TotalAmount, SaleDate = v.sale.SaleDate, FName = (v.cust.FName + " " + v.cust.LName), PropertyID = v.tw.ProjectID });
            }
            return model;
        }
        public IQueryable<Payment> ChequeClearance()
        {
            REMSDBEntities context = new REMSDBEntities();
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datet = datef.AddMonths(1);
            var model = context.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
            return model;
        }
        public List<RefundPropertyModel> RefundPropertySearch(int SaleID)
        {
            List<RefundPropertyModel> model = new List<RefundPropertyModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                var md = (from sale in context.RefundProperties where sale.SaleID==SaleID select new { sale = sale });
                foreach (var v in md)
                {
                    string rdate = "", cdate = "";
                    if (v.sale.RefundDate != null)
                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                    if (v.sale.ChequeDate != null)
                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                }
                return model;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public List<RefundPropertyModel> RefundPropertySearchByCheque(string search, string FlatId, string datefrom, string dateto, string searchtext)
        {
            List<RefundPropertyModel> model = new List<RefundPropertyModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                DateTime datef = new DateTime();
                DateTime datet = new DateTime();
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
                //if (propertyid == "? undefined:undefined ?" || propertyid == "All" || propertyid == "") propertyid = "0";
                //if (propertySubTypeID == "? undefined:undefined ?" || propertySubTypeID == "All" || propertySubTypeID == "") { propertySubTypeID = "0"; }

                //if (proSize == "" || proSize == "? undefined:undefined ?" || proSize == "All") proSize = "0";
                //int pid = Convert.ToInt32(propertyid);
                //int ptypeid = Convert.ToInt32(propertySubTypeID);
                //int psize = Convert.ToInt32(proSize);
                //if (propertyid == "0")
                //{
                    if (search == "All")
                    {
                        var md = (from sale in context.RefundProperties where sale.RefundDate >= datef && sale.RefundDate <= datet select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }

                        return model;

                    }
                    else if (search == "FlatName")
                    {
                        var md = (from sale in context.RefundProperties where sale.FlatName.Contains(searchtext) select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return model;
                    }
                    else if (search == "RefundDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);
                        var md = (from sale in context.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return model;
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);
                        var md = (from sale in context.RefundProperties join f in context.SaleFlats on sale.SaleID equals f.SaleID where f.SaleDate >= dtFrom && f.SaleDate <= dtTo select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return model;
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;
                        var md = (from sale in context.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return model;
                    }
                    else if (search == "Last 7 Days")
                    {

                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;
                        var md = (from sale in context.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return model;
                    }
                    else
                    {
                        int FId = Convert.ToInt32(FlatId);
                        int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                        var md = (from sale in context.RefundProperties where sale.SaleID == sid select new { sale = sale });
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }

                        return model;
                    }
               // }
               // else
                //{
                //    if (search == "All")
                //    {
                //        var md = (from sale in context.RefundProperties join f in context.SaleFlats on sale.SaleID equals f.SaleID where f.Flat.Floor.Tower.ProjectID == pid select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //    else if (search == "SubType")
                //    {
                //        if (ptypeid != 0)
                //        {
                //            if (psize == 0)
                //            {
                //                var md = (from sale in context.RefundProperties
                //                          join f in context.SaleFlats
                //                              on sale.SaleID equals f.SaleID
                //                          join ft in context.Flats on f.FlatID equals ft.FlatID
                //                          join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                          join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                          where tw.TowerID == pid
                //                          select new { sale = sale });
                //                foreach (var v in md)
                //                {
                //                    string rdate = "", cdate = "";
                //                    if (v.sale.RefundDate != null)
                //                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //                    if (v.sale.ChequeDate != null)
                //                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //                }
                //                return model;
                //            }
                //            else
                //            {
                //                var md = (from sale in context.RefundProperties
                //                          join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                          join ft in context.Flats on f.FlatID equals ft.FlatID
                //                          join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                          join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                          where tw.ProjectID == pid
                //                          select new { sale = sale });
                //                foreach (var v in md)
                //                {
                //                    string rdate = "", cdate = "";
                //                    if (v.sale.RefundDate != null)
                //                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //                    if (v.sale.ChequeDate != null)
                //                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //                }
                //                return model;
                //            }
                //        }
                //        else
                //        {

                //            if (psize == 0)
                //            {
                //                var md = (from sale in context.RefundProperties
                //                          join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                          join ft in context.Flats on f.FlatID equals ft.FlatID
                //                          join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                          join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                          where tw.ProjectID == pid
                //                          select new { sale = sale });
                //                foreach (var v in md)
                //                {
                //                    string rdate = "", cdate = "";
                //                    if (v.sale.RefundDate != null)
                //                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //                    if (v.sale.ChequeDate != null)
                //                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //                }
                //                return model;
                //            }
                //            else
                //            {
                //                var md = (from sale in context.RefundProperties
                //                          join f in context.SaleFlats on sale.SaleID
                //                              equals f.SaleID
                //                          join ft in context.Flats on f.FlatID equals ft.FlatID
                //                          join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                          join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                          where tw.ProjectID == pid
                //                          select new { sale = sale });
                //                foreach (var v in md)
                //                {
                //                    string rdate = "", cdate = "";
                //                    if (v.sale.RefundDate != null)
                //                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //                    if (v.sale.ChequeDate != null)
                //                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //                }
                //                return model;
                //            }

                //        }
                //    }
                //    else if (search == "FlatName")
                //    {
                //        var md = (from sale in context.RefundProperties
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join fr in context.Floors on f.FlatID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.FlatName.Contains(searchtext)
                //                  select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //    else if (search == "RefundDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from sale in context.RefundProperties
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo
                //                  select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //    else if (search == "SaleDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from sale in context.RefundProperties
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && f.SaleDate >= dtFrom && f.SaleDate <= dtTo
                //                  select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //    else if (search == "This Month")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from sale in context.RefundProperties
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo
                //                  select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //    else if (search == "Last 7 Days")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddDays(-7);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from sale in context.RefundProperties
                //                  join f in context.SaleFlats on sale.SaleID equals f.FlatID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo
                //                  select new { sale = sale });
                //        foreach (var v in md)
                //        {
                //            string rdate = "", cdate = "";
                //            if (v.sale.RefundDate != null)
                //                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                //            if (v.sale.ChequeDate != null)
                //                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                //            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                //        }
                //        return model;
                //    }
                //}

            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public List<TransferPropertyModel> TransferPropertySearch(string search,string FlatId,string datefrom, string dateto, string searchtext)
        {
            REMSDBEntities context = new REMSDBEntities();
            List<TransferPropertyModel> model = new List<TransferPropertyModel>();
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

                //if (propertyid == "? undefined:undefined ?" || propertyid == "All" || propertyid == "") propertyid = "0";
                //if (propertySubTypeID == "? undefined:undefined ?" || propertySubTypeID == "All" || propertySubTypeID == "") { propertySubTypeID = "0"; }
                //if (proSize == "" || proSize == "? undefined:undefined ?" || proSize == "All") proSize = "0";
                //int pid = Convert.ToInt32(propertyid);
                //int ptypeid = Convert.ToInt32(propertySubTypeID);
                //int psize = Convert.ToInt32(proSize);
                //if (propertyid == "0") // All Properties
                //{
                    if (search == "All")
                    {
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where sale.TransferDate >= datef && sale.TransferDate <= datet
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }

                        return model;
                        // By default showing last one month sales in all properties
                    }
                    else if (search == "FlatName")
                    {
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where ft.FlatName.Contains(searchtext)
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }

                        return model;
                    }
                    else if (search == "TransferDate")
                    {

                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return model;
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID
                                      equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where f.SaleDate >= dtFrom && f.SaleDate <= dtTo
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return model;
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return model;
                    }
                    else if (search == "Last 7 Days")
                    {
                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return model;
                    }
                    else
                    {
                        int proid = Convert.ToInt32(FlatId);
                          int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == proid select s.SaleID).FirstOrDefault();
                        var md = (from sale in context.PropertyTransfers
                                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                                  join ft in context.Flats on f.FlatID equals ft.FlatID
                                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                                  where sale.SaleID == sid
                                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                        foreach (var v in md)
                        {
                            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }

                        return model;

                    }
               // }
                //else // Search by Property id
                //{
                //    if (search == "All")
                //    {
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //    else if (search == "FlatName")
                //    {
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && ft.FlatName.Contains(searchtext)
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //    else if (search == "TransferDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //    else if (search == "SaleDate")
                //    {
                //        DateTime dtFrom = Convert.ToDateTime(datefrom);
                //        DateTime dtTo = Convert.ToDateTime(dateto);
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && f.SaleDate >= dtFrom && f.SaleDate <= dtTo
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //    else if (search == "This Month")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.SaleID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //    else if (search == "Last 7 Days")
                //    {
                //        DateTime dtFrom = DateTime.Now.AddDays(-7);
                //        DateTime dtTo = DateTime.Now;
                //        var md = (from sale in context.PropertyTransfers
                //                  join f in context.SaleFlats on sale.SaleID equals f.FlatID
                //                  join ft in context.Flats on f.FlatID equals ft.FlatID
                //                  join fr in context.Floors on ft.FloorID equals fr.FloorID
                //                  join tw in context.Towers on fr.TowerID equals tw.TowerID
                //                  where tw.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo
                //                  select new { sale = sale, SaleDate = f.SaleDate, PropertyID = tw.ProjectID, FlatID = f.FlatID });
                //        foreach (var v in md)
                //        {
                //            var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                //            var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                //            var flatname = context.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                //            string rdate = "", cdate = "";
                //            if (v.sale.TransferDate != null)
                //                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                //            if (v.SaleDate != null)
                //                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                //            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                //        }
                //        return model;
                //    }
                //}

            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);
                return model;
            }
            return model;
        }
        public List<FlatSaleModel> SearchPendingInstallment(string search, string FlatId, string datefrom, string dateto, string searchtext)
        {
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                //int Propertyid = Convert.ToInt32(propertyid);
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

                if (search == "PropertyName")
                {
                    var md1 = (from sale in context.SaleFlats
                               join f in context.Flats on sale.FlatID equals f.FlatID
                               join c in context.Customers on sale.SaleID equals c.SaleID
                               join fr in context.Floors on f.FloorID equals fr.FloorID
                               join tw in context.Towers on fr.TowerID equals tw.TowerID
                               where f.FlatName.Contains(searchtext) 
                               select new { saleID = sale.SaleID, Cust = c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale, tw = tw });
                    foreach (var v in md1)
                    {
                        decimal paidamount = 0;
                        var mdPaid = (from pay in context.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });
                        foreach (var MdpaidAdmount in mdPaid)
                        {
                            paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                        }
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.TotalAmount, DueDate = v.sale.SaleDate, FName = (v.Cust.FName + " " + v.Cust.LName), PropertyID = v.tw.ProjectID, PaidAmount = paidamount, DueAmount = (v.sale.TotalAmount - paidamount) });
                    }

                }
                else
                {
                    if (search == "BookingDate")
                    {
                        var VALUE1 = (from ins in context.FlatInstallmentDetails
                                      where ins.DueDate >= datef
                                      select new { sale = ins.FlatID, amount = ins.TotalAmount }).ToList();
                        List<int?> saleID = VALUE1.Select(e => e.sale).Distinct().ToList();
                        for (int K = 0; K < saleID.Count; K++)
                        {
                            decimal TotalInsAmount = 0;
                            int saleid = Convert.ToInt32(saleID[K].Value);
                            var list = VALUE1.Where(d => d.sale == saleid);

                            foreach (var amount in list)
                            {
                                TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                            }

                            var md1 = (from sale in context.SaleFlats
                                       join f in context.Flats on sale.FlatID equals f.FlatID
                                       join c in context.Customers on sale.SaleID equals c.SaleID
                                       join fr in context.Floors on f.FloorID equals fr.FloorID
                                       join tw in context.Towers on fr.TowerID equals tw.TowerID
                                       where sale.FlatID == saleid 
                                       select new { saleID = sale.SaleID, Cust = c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale, tw = tw });
                            foreach (var v in md1)
                            {

                                decimal paidamount = 0;
                                var mdPaid = (from pay in context.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });

                                foreach (var MdpaidAdmount in mdPaid)
                                {
                                    paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                                }
                                string bdate = "";
                                if (v.sale.SaleDate != null)
                                    bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                                model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.TotalAmount, DueDate = v.sale.SaleDate, FName = (v.Cust.FName + " " + v.Cust.LName), PropertyID = v.tw.ProjectID, PaidAmount = paidamount, DueAmount = (TotalInsAmount - paidamount) });
                            }
                        }
                    }
                    else
                    {
                         int proid = Convert.ToInt32(FlatId);
                          int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == proid select s.SaleID).FirstOrDefault();
                        var md1 = (from sale in context.SaleFlats
                                   join f in context.Flats on sale.FlatID equals f.FlatID
                                   join c in context.Customers on sale.SaleID equals c.SaleID
                                   join fr in context.Floors on f.FloorID equals fr.FloorID
                                   join tw in context.Towers on fr.TowerID equals tw.TowerID
                                   where sale.SaleID == sid
                                   select new { saleID = sale.SaleID, Cust = c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale, tw = tw });
                        foreach (var v in md1)
                        {
                            decimal paidamount = 0;
                            var mdPaid = (from pay in context.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });
                            foreach (var MdpaidAdmount in mdPaid)
                            {
                                paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                            }
                            string bdate = "";
                            if (v.sale.SaleDate != null)
                                bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.TotalAmount, DueDate = v.sale.SaleDate, FName = (v.Cust.FName + " " + v.Cust.LName), PropertyID = v.tw.ProjectID, PaidAmount = paidamount, DueAmount = (v.sale.TotalAmount - paidamount) });
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public string SearchPropertyRemak(string search, string propertyid, string datefrom, string dateto, string searchtext)
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "";
            }
        }
        public List<FlatSaleModel> SearchDemandLetter(string search, string propertyid, string datefrom)
        {
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                //int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);
                int DemandLetterid = 0;
                if (search == "DemandLetter1")
                {
                    DemandLetterid = 0;
                }
                else if (search == "DemandLetter2")
                {
                    DemandLetterid = 1;
                }
                else if (search == "DemandLetter3")
                {
                    DemandLetterid = 2;
                }
                else
                {
                    DemandLetterid = 0;
                }
                // DataFunctions DF = new DataFunctions();
                // DataTable ds = DF.GetDataTable("select I.SaleID, sum(I.TotalAmount) as TotalAmount from FlatInstallmentDetail as I inner join tblsSaleFlat on I.saleid=tblsSaleFlat.SaleID  where I.Duedate<='" + datef + "' and tblsSaleFlat.DemandStatus='" + DemandLetterid + "' group by I.saleid");
               // int FId = Convert.ToInt32(propertyid);
               // int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == FId select s.SaleID).FirstOrDefault();
                var q = from ft in context.FlatInstallmentDetails
                        join st in context.SaleFlats on ft.FlatID equals st.FlatID
                        where ft.DueDate <= datef && st.DemandStatus == DemandLetterid 
                        select new { st = st, ft = ft };
                foreach (var m in q)
                {
                    decimal TotalInsAmount = 0;
                    int saleid = m.st.SaleID;
                    TotalInsAmount = decimal.Parse(m.st.TotalAmount.ToString());
                    decimal paidamount = 0;
                    var mdPaid = (from pay in context.Payments where pay.SaleID == saleid select new { paidamount = pay.Amount });
                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    if ((TotalInsAmount - paidamount) > 0)
                    {
                        var md1 = (from sale in context.SaleFlats
                                   join f in context.Flats on sale.FlatID equals f.FlatID
                                   join c in context.Customers on sale.SaleID equals c.SaleID
                                   join fr in context.Floors on f.FloorID equals fr.FloorID
                                   join tw in context.Towers on fr.TowerID equals tw.TowerID
                                   where sale.SaleID == saleid && sale.DemandStatus == DemandLetterid
                                   select new { saleID = sale.SaleID, cust = c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale, tw = tw });
                        foreach (var v in md1)
                        {
                            string bdate = "";
                            if (v.sale.SaleDate != null)
                                bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.TotalAmount, DueDate = v.sale.SaleDate, FName = (v.cust.FName + " " + v.cust.LName), PropertyID = v.tw.ProjectID, PaidAmount = paidamount, DueAmount = (TotalInsAmount - paidamount) });
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);
            }
            return model;
        }
        public string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);
                string[] AllSaleID = saleid.Split(',');
                int DemandLetterid = 0;
                // Date.
                if (search == "DemandLetter1")
                {
                    DemandLetterid = 1;
                }
                else if (search == "DemandLetter2")
                {
                    DemandLetterid = 2;
                }
                else if (search == "DemandLetter3")
                {
                    DemandLetterid = 3;
                }
                for (int K = 0; K < AllSaleID.Length; K++)
                {
                    if (Convert.ToString(AllSaleID[K]) != "")
                    {
                        int SaleID = Convert.ToInt32(AllSaleID[K]);

                        int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where s.SaleID == SaleID select f.FlatID).FirstOrDefault();
                       // var Flatid = context.SaleFlats.Where(s => s.SaleID == SaleID select s.SaleID).FirstOrDefault();
                        // DataFunctions DF = new DataFunctions();
                        //  DataTable ds = DF.GetDataTable("select SaleID, sum(TotalAmount) as TotalAmount from tblSInstallmentDetail where Duedate<='" + datef + "' and  saleid='" + SaleID + "' group by saleid");
                        var q = context.FlatInstallmentDetails.Where(p => p.DueDate <= datef && p.FlatID == sid);
                        foreach (var m in q)
                        {
                            using (REMSDBEntities context1 = new REMSDBEntities())
                            {
                                decimal TotalInsAmount = 0;
                                TotalInsAmount = Convert.ToDecimal(m.TotalAmount);
                                decimal paidamount = 0;
                                var mdPaid = (from pay in context1.Payments where pay.SaleID == SaleID select new { paidamount = pay.Amount });
                                foreach (var MdpaidAdmount in mdPaid)
                                {
                                    paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                                }
                                if ((TotalInsAmount - paidamount) > 0)
                                {
                                    var stud = (from s in context1.SaleFlats
                                                where s.SaleID == SaleID
                                                select s).FirstOrDefault();
                                    stud.DemandStatus = DemandLetterid;
                                    context1.Entry(stud).State = EntityState.Modified;
                                    context1.SaveChanges();
                                    ReminderLetter _reminderletter = new ReminderLetter();
                                    _reminderletter.CreateDate = DateTime.Now;
                                    _reminderletter.LetterType = search;
                                    _reminderletter.SaleID = SaleID;
                                    _reminderletter.duedate = datef;
                                    _reminderletter.DueAmount = TotalInsAmount;
                                    context1.ReminderLetters.Add(_reminderletter);
                                    context1.SaveChanges();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
            return "";
        }
        public List<ReminderLetterModel> DemandLettertPrintReport(string transactionid)
        {
            List<ReminderLetterModel> model = new List<ReminderLetterModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                string duedate = "";
                string[] saleids = transactionid.Split(',');
                foreach (string saleid in saleids)
                {
                    if (saleid != "")
                    {
                        int sid = Convert.ToInt32(saleid);
                        var md1 = (from sale in context.SaleFlats
                                   join f in context.Flats on sale.FlatID equals f.FlatID
                                   join c in context.Customers on sale.SaleID equals c.SaleID
                                   join d in context.ReminderLetters on sale.SaleID equals d.SaleID
                                   join fr in context.Floors on f.FloorID equals fr.FloorID
                                   join tw in context.Towers on fr.TowerID equals tw.TowerID
                                   join p in context.Projects on tw.ProjectID equals p.ProjectID
                                   where d.SaleID == sid
                                   select new { saleID = sale.SaleID, FlatName = f.FlatName, CustomrName = c.AppTitle + " " + c.FName + " " + c.MName + " " + c.LName, MobileNo = c.MobileNo, DueDate = d.duedate, LetterDate = d.CreateDate, DueAmount = d.DueAmount, ProjectName = p.PName, CompanyName = p.CompanyName, PropertyAddress = p.Address });
                        foreach (var v in md1)
                        {
                            string duedt = "", Ldate1 = "", LDate2 = "", LDate3 = "";
                            if (v.DueDate != null)
                                duedt = v.DueDate.Value.ToString("dd/MM/yyyy");
                            if (v.LetterDate != null)
                                Ldate1 = v.LetterDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new ReminderLetterModel { SaleID = v.saleID, CompanyName = v.CompanyName, CustomerName = v.CustomrName, InterestRate = "18", LetterDateSt = Ldate1, LetterType = "DemandLetter1", ProjectName = v.ProjectName, PropertyAddress = v.PropertyAddress, FlatName = v.FlatName, DueDateST = duedt, DueAmount = v.DueAmount });
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public List<FlatSaleModel> DemandLettertPrintReport3(string transactionid)
        {
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                string duedate = "";
                int saleid = Convert.ToInt32(transactionid);
                var VALUE1 = (from ins in context.FlatInstallmentDetails
                              where ins.SaleID == saleid
                              select new { sale = ins.SaleID, amount = ins.TotalAmount, duedate = ins.DueDate }).ToList();
                decimal TotalInsAmount = 0;
                foreach (var amount in VALUE1)
                {
                    TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                    duedate = Convert.ToString(amount.duedate);
                }
                var md1 = (from sale in context.SaleFlats
                           join f in context.Flats on sale.FlatID equals f.FlatID
                           join c in context.Customers on sale.SaleID equals c.SaleID
                           where sale.SaleID == saleid
                           select new { saleID = sale.SaleID, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });
                foreach (var v in md1)
                {
                    decimal paidamount = 0;
                    var mdPaid = (from pay in context.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });
                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    string bdate = "";
                    if (v.sale.SaleDate != null)
                        if ((TotalInsAmount - paidamount) > 0)
                        {
                            string secondDate = "";
                            string fristDate = "";
                        }
                }

            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public List<FlatSaleModel> DemandLettertPrintReport2(string transactionid)
        {
            List<FlatSaleModel> model = new List<FlatSaleModel>();
            try
            {
                string duedate = "";
                int saleid = Convert.ToInt32(transactionid);
                REMSDBEntities context = new REMSDBEntities();
                var VALUE1 = (from ins in context.FlatInstallmentDetails
                              where ins.SaleID == saleid
                              select new { sale = ins.SaleID, amount = ins.TotalAmount, duedate = ins.DueDate }).ToList();
                decimal TotalInsAmount = 0;
                foreach (var amount in VALUE1)
                {
                    TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                    duedate = Convert.ToString(amount.duedate);
                }

                var md1 = (from sale in context.SaleFlats
                           join f in context.Flats on sale.FlatID equals f.FlatID
                           join c in context.Customers on sale.SaleID equals c.SaleID
                           where sale.SaleID == saleid
                           select new { saleID = sale.SaleID, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });

                foreach (var v in md1)
                {
                    decimal paidamount = 0;
                    var mdPaid = (from pay in context.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });

                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    string bdate = "";
                    if (v.sale.SaleDate != null)
                        if ((TotalInsAmount - paidamount) > 0)
                        {
                        }
                }

            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);

            }
            return model;
        }
        public List<FlatDemandLetter> ViewSearchDemandLetter(string search, string propertyid, string datefrom)
        {
            List<FlatDemandLetter> model = new List<FlatDemandLetter>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);
                DateTime searchDatePlusOne = datef.AddDays(-1);
                datef = datef.AddDays(1);
                var q = from st in context.SaleFlats
                        join ft in context.Flats on st.FlatID equals ft.FlatID
                        join cr in context.Customers on st.SaleID equals cr.SaleID
                        join fr in context.Floors on ft.FloorID equals fr.FloorID
                        join tr in context.Towers on fr.TowerID equals tr.TowerID
                        join rl in context.ReminderLetters on st.SaleID equals rl.SaleID
                        where rl.LetterType == search && rl.CreateDate <= datef
                        select new { st = st, ft = ft, rl = rl, cr = cr };
               
                foreach (var v in q)
                {
                    model.Add(new FlatDemandLetter { ID = v.rl.ReminderLetterID, SaleID = v.st.SaleID, DueAmount = decimal.Parse(v.rl.DueAmount.ToString()), FlatName = v.ft.FlatName, FName = v.cr.FName, MobileNo = v.cr.MobileNo, SaleRate = v.st.TotalAmount });
                }
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
            return model;
        }

    }
}