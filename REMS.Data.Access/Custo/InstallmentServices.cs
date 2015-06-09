using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
    interface IInstallmentServices
    {
        Array GetFlatList(string flatName, int pId);
        List<Payment> GetPayment(int saleId);
        decimal GetTotalPayment(int saleId);
        Array GetFlatSale(string flatid);
        List<EventModel> GetInstallment(string saleId);
        string GetInstallmentByInstallId(int installmentID, string modifyDate);
        string GetInstallmetnDetailsByFlatId(int saleId);
    }
    class InstallmentServices
    {
        // Search flat 
        public Array GetFlatList(string flatName, int pId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                List<Flat> lstPropertyDetails = new List<Flat>();
                var flt = from ft in context.Flats
                          join fr in context.Floors on ft.FloorID equals fr.FloorID
                          join tr in context.Towers on fr.TowerID equals tr.TowerID
                          where (tr.ProjectID == pId && ft.FlatName == flatName)
                          select new { ft.BadRooms, ft.Balconies };

                return flt.ToArray();
            }
        }
        public List<Payment> GetPayment(int saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                return context.Payments.Where(p => p.SaleID == saleId).ToList();
            }
        }
        public decimal GetTotalPayment(int saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                return context.FlatInstallmentDetails.Where(p => p.SaleID == saleId).Sum(p => p.TotalAmount).Value;
            }
        }
        public Array GetFlatSale(string flatid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                //--  plane type is pending in below conversion query  var payment = obj.GetDataTable("Select s.SaleID,PlanType.PlanTypeName,s.FlatID,s.Aggrement,s.SaleDate,s.SaleRate,s.ServiceTaxPer,s.ServiceTaxAmount,s.BookingAmount,s.CustomerID,s.PropertyID,s.PropertyTypeID,s.PropertySizeID,s.PlanID,s.PaymentFor, c.* from tblssaleflat s inner join Customer c on s.CustomerID=c.CustomerID  INNER JOIN PlanType  ON s.PlanID = PlanType.PlanID where s.FlatID='" + flatid + "'");
                var payment = from sale in context.SaleFlats
                              join
                                  cr in context.Customers on sale.SaleID equals cr.SaleID

                              select new { sale, cr };
                return payment.ToArray();
            }
        }
        public List<EventModel> GetInstallment(string saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
                var installment = (from il in context.FlatInstallmentDetails
                                   join et in context.EventMasters on il.PlanInstallmentID equals et.EventID

                                   select new { il }).DefaultIfEmpty().ToList();
                var v = installment.AsEnumerable().ToList();
                List<EventModel> em = new List<EventModel>();
                foreach (var s in v)
                {
                    DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    dtinfo.DateSeparator = "/";
                    string ddate = "";
                    //-- here DueDate,dueamount,Installmentno not found in flatinstallment detail table
                    if (s.il.DueDate != null)
                    {
                        string dt = s.il.DueDate.ToString();
                        if (dt == "") ddate = dt;
                        else
                            ddate = Convert.ToDateTime(s.il.DueDate.ToString()).ToString("dd/MM/yyyy");
                    }
                    string mdate = "";
                    if (s.il.ModifyDate != null)
                    {
                        string dt = s.il.ModifyDate.ToString();
                        if (dt == "") mdate = dt;
                        else
                            mdate = Convert.ToDateTime(s.il.ModifyDate.ToString()).ToString("dd/MM/yyyy");
                    }
                    em.Add(new EventModel { InstallmentID = Convert.ToInt32(s.il.InstallmentID), DueAmount = Convert.ToDecimal(s.il.TotalAmount), TotalAmount = Convert.ToDecimal(s.il.TotalAmount), InstallmentNo = s.il.InstallmentID.ToString(), EventName = s.il.PlanInstallment.Installment.ToString(), DueDate = ddate, ModifyDate = mdate });
                }
                return em;
            }
        }
        public string GetInstallmentByInstallId(int installmentID, string modifyDate)
        {
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            DateTime dt = new DateTime();
            if (modifyDate != null && modifyDate != "")
            {
                using (REMSDBEntities context = new REMSDBEntities())
                {

                    var fl = context.FlatInstallmentDetails.Where(s => s.InstallmentID == installmentID).FirstOrDefault<FlatInstallmentDetail>();
                    fl.DueDate = dt;
                    context.SaveChanges();
                }
                return "Event Completion date updated.";
            }
            else
                return "Invalid Date format.";
        }
        public string GetInstallmetnDetailsByFlatId(int saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                StringBuilder l_StringBuilder = new StringBuilder();
                var list = context.FlatInstallmentDetails.Where(p => p.SaleID == saleId).ToList();
                var downplayPlanEevnts = "";
                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                l_StringBuilder.Append("<thead><tr><th>InstallmentNo</th><th>Due Date</th><th>ModifiedDate</th><th>Due Amount</th></tr></thead><tbody>");
                if (list.Count > 0)
                {
                    decimal dueAmount;
                    string dueDate = String.Empty;
                    decimal TotalAmount = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        dueAmount = decimal.Parse(list[i].TotalAmount.ToString());//-- due amount not found
                        downplayPlanEevnts = Convert.ToString(list[i].InstallmentID.ToString());//-- InstallmentNo not found
                        if (!String.IsNullOrEmpty(Convert.ToString(list[i].DueDate))) //-- DueDate not found
                        {
                            l_StringBuilder.Append("<tr><td>" + downplayPlanEevnts.ToString() + "</td></td><td>" + Convert.ToDateTime(list[i].CreateDate).ToString("dd/MM/yyyy") + "</td><td><input type='text' name='txtDueDate' value='" + "" + "' ></td><td>" + dueAmount + "</td></td><tr>");
                        }
                        else
                        {
                            l_StringBuilder.Append("<tr><td>" + downplayPlanEevnts.ToString() + "</td></td><td>" + dueDate + "</td><td>" + dueAmount + "</td><tr>");
                        }

                        TotalAmount = TotalAmount + dueAmount;
                    }
                    l_StringBuilder.Append("<tr><td></td><td></td><td>Total</td><td><b>" + TotalAmount + "</b></td><tr>");
                }
                l_StringBuilder.Append("</tbody>");
                return l_StringBuilder.ToString();
            }
        }
    }
}