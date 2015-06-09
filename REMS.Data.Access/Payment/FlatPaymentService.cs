using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access
{
    public interface IFlatPaymentService
    {
        List<Payment> Search();
        List<Payment> Search(string[] collection, ref string pName);
        List<Payment> EditPayment();
        List<Flat> GetFlatList(string flatName, int pId);

    }
    public class FlatPaymentService : IFlatPaymentService
    {
        public List<Payment> Search()
        {
            List<Payment> modal = new List<Payment>(); ;
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    DateTime dateFrom = new DateTime();
                    DateTime dateTo = new DateTime();
                    dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dateTo = dateFrom.AddMonths(1);
                    modal = context.Payments.Where(p => p.PaymentDate >= dateFrom && p.PaymentDate <= dateTo).OrderByDescending(o => o.PaymentID).ToList();

                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);

                }
            }
            return modal;
        }
        public List<Payment> Search(string[] collection, ref string pName)
        {
            List<Payment> tblSPayment = new List<Payment>();
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                string proName = collection[0];
                string proType = collection[1];
                string proSize = collection[2];
                string protypeName = collection[3];
                int pid = 0, ptype = 0, psize = 0;
                if (proName == "? undefined:undefined ?" || proName == "All")
                    proName = "All";
                else pid = Convert.ToInt32(proName);
                if (proType == "? undefined:undefined ?" || proType == "All")
                { proType = "All"; pName = "Property Name"; }
                else { ptype = Convert.ToInt32(proType); pName = protypeName; }
                if (proSize == "? undefined:undefined ?" || proSize == "All")
                    proSize = "All";
                else psize = Convert.ToInt32(proSize);
                string flat = collection[4];
                if (flat == "0")
                {
                    #region Custom Search
                    string srch = collection[5];
                    string search = collection[6];
                    string soryBy = collection[7];
                    string dateFrom = collection[8];
                    string dateTo = collection[9];
                    string searchText = collection[10];
                    DateTime dateFromSearch = new DateTime();
                    DateTime dateToSearch = new DateTime();
                    if (dateFrom != "" && dateTo != "")
                    {
                        dateFromSearch = Convert.ToDateTime(dateFrom);
                        dateToSearch = Convert.ToDateTime(dateTo);
                    }
                    if (search == "All")
                    {
                        if (soryBy == "All")
                        {
                            dateFromSearch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            dateToSearch = dateFromSearch.AddMonths(1);
                            tblSPayment = context.Payments.Where(p => p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                        else
                        {
                            tblSPayment = context.Payments.Where(p => p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                    }
                    else if (search == "FlatName")
                    {
                        if (soryBy == "All")
                        {
                            tblSPayment = context.Payments.Where(p => p.FlatName.Contains(searchText)).OrderByDescending(o => o.PaymentID).ToList();

                        }
                        else
                        {
                            tblSPayment = context.Payments.Where(p => p.FlatName.Contains(searchText) && p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();

                        }
                    }
                    else if (search == "Customer Name")
                    {
                        if (soryBy == "All")
                        {
                            tblSPayment = context.Payments.Where(p => p.CustomerName.Contains(searchText)).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                        else
                        {
                            tblSPayment = context.Payments.Where(p => p.CustomerName.Contains(searchText) && p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                    }
                    else if (search == "Cheque No")
                    {
                        if (soryBy == "All")
                        {
                            tblSPayment = context.Payments.Where(p => p.ChequeNo.Contains(searchText)).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                        else
                        {
                            tblSPayment = context.Payments.Where(p => p.ChequeNo.Contains(searchText) && p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                            return tblSPayment;
                        }
                    }
                    else if (search == "This Month")
                    {
                        dateFromSearch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dateToSearch = dateFromSearch.AddMonths(1);
                        tblSPayment = context.Payments.Where(p => p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                        return tblSPayment;
                    }
                    else if (search == "Last 7 Days")
                    {
                        dateToSearch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dateFromSearch = dateToSearch.AddDays(-7);
                        tblSPayment = context.Payments.Where(p => p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                        return tblSPayment;
                    }
                    else
                    {
                        dateFromSearch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dateToSearch = dateFromSearch.AddMonths(1);
                        tblSPayment = context.Payments.Where(p => p.PaymentDate >= dateFromSearch && p.PaymentDate <= dateToSearch).OrderByDescending(o => o.PaymentID).ToList();
                        return tblSPayment;
                    }
                    #endregion
                }
                else
                {

                    if (proName != "All" && proType != "All" && proSize != "All")
                    {
                        var model1 = (from sale in
                                          context.SaleFlats
                                      join pay in context.Payments on sale.SaleID equals pay.SaleID
                                      join flats in context.Flats on sale.FlatID equals flats.FlatID
                                      join floor in context.Floors on flats.FloorID equals floor.FloorID  
                                      join tower in context.Towers on floor.TowerID equals tower.TowerID
                                      //join tower in context.Towers on sale.FlatID equals tower.f
                                      where tower.ProjectID.Value == pid && tower.ProjectTypeID == ptype //&& sale.PropertySizeID == psize //-- PropertySizeID not found
                                      select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();

                        foreach (var v in model1)
                        {
                            //tblSPayment.Add(new tblSPayment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                        }
                        return tblSPayment;
                    }
                    else if (proName != "All" && proType != "All" && proSize == "All")
                    {
                        //var model1 = (from sale in context.SaleFlats
                        //              join pay in context.Payments on sale.SaleID equals pay.SaleID
                        //              where sale.PropertyID.Value == pid && sale.PropertyTypeID == ptype
                        //              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();
                        //foreach (var v in model1)
                        //{
                        //    tblSPayment.Add(new Payment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                        //}
                        return tblSPayment;
                    }
                    else if (proName != "All" && proType == "All" && proSize == "All")
                    {
                        //var model1 = (from sale in
                        //                  context.SaleFlats
                        //              join pay in context.Payments on sale.SaleID equals pay.SaleID
                        //              where sale.proje.Value == pid
                        //              select new { TransactionID = pay.TransactionID, FlatName = pay.FlatName, CustomerName = pay.CustomerName, PaymentDate = pay.PaymentDate, PaymentMode = pay.PaymentMode, Remarks = pay.Remarks, Saleid = pay.SaleID, Amount = pay.Amount, PaymentID = pay.PaymentID }).AsEnumerable();

                        //foreach (var v in model1)
                        //{
                        //    tblSPayment.Add(new Payment { TransactionID = v.TransactionID, FlatName = v.FlatName, CustomerName = v.CustomerName, PaymentDate = v.PaymentDate, PaymentMode = v.PaymentMode, Remarks = v.Remarks, SaleID = v.Saleid, Amount = v.Amount, PaymentID = v.PaymentID });
                        //}
                        return tblSPayment;
                    }
                }
                return tblSPayment;
            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);

            }
            return tblSPayment;
        }
        public List<Payment> EditPayment()
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dateTo = dateFrom.AddMonths(1);
            REMSDBEntities context = new REMSDBEntities();
            return context.Payments.Where(p => p.PaymentDate >= dateFrom && p.PaymentDate <= dateTo).OrderByDescending(o => o.PaymentID).ToList();          
        }
        public List<Flat> GetFlatList(string flatName, int pId)
        {
            List<Flat> lstPropertyDetails = new List<Flat>();
            REMSDBEntities context = new REMSDBEntities();
            return context.Flats.Where(p => p.FlatName == flatName).ToList(); //&& p.PID == pId).ToList();
            //var Flat = obj.GetDataTable("Select * from tblsflat where FlatName='" + flatname + "' and PID='" + pid + "' and Status=1");
            //var v = Flat.AsEnumerable().ToList();
            //return Json(new { Result = (from i in v select new { FlatID = i["FlatID"], FlatName = i["FlatName"] }) }, JsonRequestBehavior.AllowGet);
        }

    }
}
