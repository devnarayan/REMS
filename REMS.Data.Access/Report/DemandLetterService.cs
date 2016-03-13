using AutoMapper;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Report
{
    public interface IDemandLetterService
    {
        DataTable SearchDemandLetter(string TowerID, string FlatID, string datefrom);
        string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid);
        string SaveDimandLetterDimand(string datefrom, string saleid);
        List<FlatSaleModel> DemandLettertPrintReport3(string transactionid);
        List<FlatSaleModel> DemandLettertPrintReport2(string transactionid);
        List<FlatDemandLetter> ViewSearchDemandLetter(string search, string propertyid, string datefrom);
        List<ReminderLetterModel2> GetDemandLetter(string searchby, DateTime datefrom, DateTime dateto);
    }
    public class DemandLetterService : IDemandLetterService
    {
        private DataFunctions dbFunction;
        private REMSDBEntities dbContext;
        public DemandLetterService()
        {
            dbFunction = new DataFunctions();
            dbContext = new REMSDBEntities();
        }
        public DataTable SearchDemandLetter(string TowerID, string FlatID, string datefrom)
        {
            try
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                DateTime dt = DateTime.Now;
                try
                {
                    dt = Convert.ToDateTime(datefrom, dtinfo);
                }
                catch (Exception ex)
                {
                    dt = DateTime.Now;
                }
                Hashtable ht = new Hashtable();
                ht.Add("TowerID", TowerID);
                ht.Add("FlatID", FlatID);
                ht.Add("DueDate", dt);
                DataTable tbl = dbFunction.spGetDatatable("spGetDemandLetter", ht);
                return tbl;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return null;
            }
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
        public string SaveDimandLetterDimand(string datefrom, string saleid)
        {
            try
            {
                string[] sids = saleid.Split(',');
                foreach (string sid in sids)
                {
                    if (!String.IsNullOrEmpty(sid))
                    {
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        dtinfo.ShortDatePattern = "dd/MM/yyyy";
                        dtinfo.DateSeparator = "/";
                        DateTime dt = DateTime.Now;
                        try
                        {
                            dt = Convert.ToDateTime(datefrom, dtinfo);
                        }
                        catch (Exception ex)
                        {
                            dt = DateTime.Now;
                        }
                        Hashtable ht = new Hashtable();
                        ht.Add("SaleID", Convert.ToInt32(sid));
                        ht.Add("DueDate", dt);
                        int tbl = dbFunction.ExecuteSP("spSaveDemandLetter", ht);
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return null;
            }
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
                                   where d.ReminderLetterID == sid
                                   select new { saleID = sale.SaleID, FlatName = f.FlatName, CustomrName = c.AppTitle + " " + c.FName + " " + c.MName + " " + c.LName, MobileNo = c.MobileNo, DueDate = d.duedate, LetterDate = d.CreateDate, DueAmount = d.DueAmount, ProjectName = p.PName, CompanyName = p.CompanyName, PropertyAddress = p.Address });
                        foreach (var v in md1)
                        {
                            string duedt = "", Ldate1 = "", LDate2 = "", LDate3 = "";
                            if (v.DueDate != null)
                                duedt = v.DueDate.Value.ToString("dd/MM/yyyy");
                            if (v.LetterDate != null)
                                Ldate1 = v.LetterDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new ReminderLetterModel { SaleID = v.saleID, CompanyName = v.CompanyName, CustomerName = v.CustomrName, FlatNo=v.FlatName, InterestRate = "18", LetterDateSt = Ldate1, LetterType = "DemandLetter1", ProjectName = v.ProjectName, PropertyAddress = v.PropertyAddress, DueDateST = duedt, DueAmount = v.DueAmount });
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
        public List<ReminderLetterModel2> GetDemandLetter(string searchby, DateTime datefrom, DateTime dateto)
        {
            try
            {
                List<ReminderLetterModel2> list = new List<ReminderLetterModel2>();
                if (searchby == "DueDate")
                {
                    var model = (from ltr in dbContext.ReminderLetters join flat in dbContext.Flats on ltr.FlatID equals flat.FlatID join cust in dbContext.Customers on ltr.CustomerID equals cust.CustomerID where ltr.duedate >= datefrom && ltr.duedate <= dateto select new { Letter = ltr, CustomerName = cust.AppTitle + " " + cust.FName + " " + cust.MName + " " + cust.LName, FlatNo = flat.FlatNo });
                    foreach (var mdl in model)
                    {
                        ReminderLetterModel2 letter = new ReminderLetterModel2();
                        Mapper.CreateMap<ReminderLetter, ReminderLetterModel2>();
                        letter = Mapper.Map<ReminderLetter, ReminderLetterModel2>(mdl.Letter);
                        if (letter.CreateDate != null)
                            letter.CrDateSt = letter.CreateDate.Value.ToString("dd/MM/yyyy");
                        else letter.CrDateSt = "";
                        if (letter.duedate != null)
                            letter.DueDateSt = letter.duedate.Value.ToString("dd/MM/yyyy");
                        else letter.DueDateSt = "";
                        letter.CustomerName = mdl.CustomerName;
                        letter.FlatNo = mdl.FlatNo;
                        list.Add(letter);
                    }
                    return list;
                }
                else
                {
                    var model = (from ltr in dbContext.ReminderLetters join flat in dbContext.Flats on ltr.FlatID equals flat.FlatID join cust in dbContext.Customers on ltr.CustomerID equals cust.CustomerID where ltr.CreateDate >= datefrom && ltr.duedate <= dateto select new { Letter = ltr, CustomerName = cust.AppTitle + " " + cust.FName + " " + cust.MName + " " + cust.LName, FlatNo = flat.FlatNo });
                    foreach (var mdl in model)
                    {
                        ReminderLetterModel2 letter = new ReminderLetterModel2();
                        Mapper.CreateMap<ReminderLetter, ReminderLetterModel2>();
                        letter = Mapper.Map<ReminderLetter, ReminderLetterModel2>(mdl.Letter);
                        if (letter.CreateDate != null)
                            letter.CrDateSt = letter.CreateDate.Value.ToString("dd/MM/yyyy");
                        else letter.CrDateSt = "";
                        if (letter.duedate != null)
                            letter.DueDateSt = letter.duedate.Value.ToString("dd/MM/yyyy");
                        else letter.DueDateSt = "";
                        letter.CustomerName = mdl.CustomerName;
                        letter.FlatNo = mdl.FlatNo;
                        list.Add(letter);
                    }
                    return list;
                }
            }
            catch(Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return null;
            }
        }
    }
}
