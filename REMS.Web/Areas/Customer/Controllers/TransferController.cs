using REMS.Web.Areas.Admin.Models;
using REMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Data.Entity;
using System.Collections;

namespace REMS.Web.Areas.Customer.Controllers
{
    public class TransferController : Controller
    {
        //
        // GET: /Customer/Transfer/
        public ActionResult Index(int? Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        public ActionResult TransferCustomer(int? Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }
        public ActionResult TransferProperty(int? Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        public ActionResult RefundProperty(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        public string GetSaleIDByPName(string PID)
        {
            REMSDBEntities context = new REMSDBEntities();

            int proid = Convert.ToInt32(PID);
            int sid = (from s in context.SaleFlats join f in context.Flats on s.FlatID equals f.FlatID where f.FlatID == proid select s.SaleID).FirstOrDefault();
            return sid.ToString();

            //var model = context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();
            //Mapper.CreateMap<Flat, FlatModel>();
            //var md = Mapper.Map<Flat, FlatModel>(model);
            //return md;
        }
        public string GetPropertyInfoByFlatID(string fid, string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int id = Convert.ToInt32(fid);
            int selid = Convert.ToInt32(saleid);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var fname = context.Flats.Where(f => f.FlatID == id).FirstOrDefault();
            var fmodel = context.SaleFlats.Where(s => s.FlatID == fname.FlatID).FirstOrDefault();

            //int pid = (int)fmodel.PropertyID;
            //int tid = (int)fmodel.PropertyTypeID;
            //int sid = (int)fmodel.PropertySizeID;
            //int plnid = (int)fmodel.PlanID;
            //dic.Add("PlanID", fmodel.PlanID.Value.ToString());
            //string planname = context.PlanTypes.Where(p => p.PlanID == plnid).FirstOrDefault().PlanTypeName;
            dic.Add("SaleID", Convert.ToString(fmodel.SaleID));
            dic.Add("SaleDate", Convert.ToString(fmodel.SaleDate));
            dic.Add("TotalAmount", Convert.ToString(fmodel.TotalAmount));
            dic.Add("FlatNo", fname.FlatNo);
            dic.Add("FlatType", fname.FlatType);
            dic.Add("FlatName", fname.FlatName);
            dic.Add("FlatSize", Convert.ToString(fname.FlatSize));
            // string ij = fname.FlatInstallmentDetails.FirstOrDefault().PlanInstallment.Plan.PlanName;//.PlanInstallmentID.Value;
            dic.Add("FlatSizeUnit", fname.FlatSizeUnit);
            dic.Add("SalePrice", Convert.ToString(fmodel.TotalAmount));
            //string pname = context.tblSProperties.Where(p => p.PID == pid).FirstOrDefault().PName;
            //dic.Add("PropertyName", pname);
            //dic.Add("PID", pid.ToString());

            //string ptype = context.tblSPropertyTypes.Where(p => p.PropertyTypeID == tid).FirstOrDefault().PType;
            //dic.Add("PropertyType", ptype);
            //var psize = context.tblSPropertySizes.Where(p => p.PropertySizeID == sid).FirstOrDefault();
            //string size = psize.Size.Value.ToString() + " " + psize.Unit;
            //dic.Add("PropertySize", size);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dic);
        }

        public string RefundPropertySave(string FlatID, string SaleID, string Amount, string Date, string PayMode, string ChequeNo, string ChequeDate, string BankName, string Branch, string Remarks, string FlatName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(SaleID);
                DateTime dt = new DateTime();
                DateTime cdt;
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.DateSeparator = "/";
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                if (Date != null && Date != "")
                    dt = Convert.ToDateTime(Date, dtinfo);
                int j = 0;
                if (ChequeDate != null && ChequeDate != "")
                {
                    cdt = Convert.ToDateTime(ChequeDate, dtinfo);
                    j = 1;
                }

                else cdt = new DateTime();
                RefundProperty pro = new RefundProperty();
                pro.SaleID = sid;
                pro.RefundDate = dt;
                pro.PaymentMode = PayMode;
                pro.RefundAmount = Convert.ToDecimal(Amount);
                pro.BankName = BankName;
                pro.BranchName = Branch;
                pro.FlatName = FlatName;
                if (j == 0)
                    pro.ChequeDate = null;
                else
                    pro.ChequeDate = cdt;

                pro.ChequeNo = ChequeNo;

                context.RefundProperties.Add(pro);
                int i = context.SaveChanges();
                if (i > 0)
                {
                    int fid = Convert.ToInt32(FlatID);
                    // Update status =0;
                    var flat = context.Flats.Where(f => f.FlatID == fid).FirstOrDefault();
                    flat.Status = "Available";
                    context.Entry(flat).State = EntityState.Modified;
                    //context.Flats.Add(flat);
                    i = context.SaveChanges();

                    // Update Status=0;
                    var sale = context.SaleFlats.Where(s => s.SaleID == sid).FirstOrDefault();
                    sale.Status = "Refunded";
                    sale.FlatID = null;
                    context.Entry(sale).State = EntityState.Modified;
                    //context.SaleFlats.Add(sale);
                    i = context.SaveChanges();
                    if (i > 0)
                    {
                        return "Yes";
                    }
                    else
                        return "No";
                }
                else
                    return "No";
            }
        }
        public string CheckRefundProperty(string SaleID)
        {
            try
            {

                REMSDBEntities context = new REMSDBEntities();
                int sid = Convert.ToInt32(SaleID);
                var model = context.SaleFlats.Where(p => p.SaleID == sid).FirstOrDefault().Status;
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(null);
            }
        }
        public string TranserPropertySave(int curFlatID, int newFlatID,int saleID,string transferType)
        {
            DataFunctions dbContext = new DataFunctions();
            Hashtable HT = new Hashtable();
            HT.Add("CurFlatID", curFlatID);
            HT.Add("NewFlatID", newFlatID);
            HT.Add("SaleID", saleID);
            HT.Add("UserName", User.Identity.Name);
            HT.Add("TransferType", transferType);
            bool bl = dbContext.ExecuteProcedure("spTransferProperty", HT);
            return Newtonsoft.Json.JsonConvert.SerializeObject(bl);
        }
    }
}