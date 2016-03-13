using AutoMapper;
using log4net;
using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Sale
{
    public interface IPaymentDiscountService
    {
        int AddPaymentDiscount(PaymentDiscountModel model);
        int AddPaymentDiscountApprove(PaymentDiscountApprovalModel model);
        List<PaymentDiscountModel> GetPaymentDisByAdmin(string userName, bool isApproved);
        PaymentDiscountModel GetPaymentDiscount(int paymentDiscountid);
        List<PaymentDiscountApprovalModel> GetPaymentDiscountApproveList(int paymentDiscountid);
        PaymentDiscountApprovalModel GetPaymentDiscountApprove(int paymentDiscountApprovalid);
        int UpdateStatus(int paymentdiscountid, bool status, string remarks, DateTime updated);
        int ApprovePaymentDiscountApprove(int PaymentDiscountID, string Remarks, string userName);
        PaymentDiscountModel ApprovePaymentDiscount(int paymentDiscountID, string updateby);
        List<PaymentDiscountModel> GetPaymentDiscountByFlat(string flatno, bool IsApproved);
        List<PaymentDiscountModel> GetPaymentDiscounByReqDate(DateTime datefrom, DateTime dateto, bool IsApproved);
        List<PaymentDiscountModel> GetPaymentDiscountByApproveDate(DateTime datefrom, DateTime dateto, bool IsApproved);
    }
    public class PaymentDiscountService : IPaymentDiscountService
    {
        private REMSDBEntities dbContext;
        private DataFunctions dbfunction;
        ILog logger = log4net.LogManager.GetLogger(typeof(PaymentDiscountService));

        public PaymentDiscountService()
        {
            dbContext = new REMSDBEntities();
            dbfunction = new DataFunctions();
        }
        public int AddPaymentDiscount(PaymentDiscountModel model)
        {
            try
            {
                Mapper.CreateMap<PaymentDiscountModel, PaymentDiscount>();
                var pd = Mapper.Map<PaymentDiscountModel, PaymentDiscount>(model);
                pd.ReqDate = DateTime.Now;
                pd.IsApproved = false;
                pd.CreatedBy = model.CreatedBy;
                dbContext.PaymentDiscounts.Add(pd);
                int i = dbContext.SaveChanges();
                i = pd.PaymentDiscountID;
                model.PaymentDiscountID = pd.PaymentDiscountID;
                string[] users = model.UserNames.Split(',');
                var authuser = model.AuthUserNames.Split(',');
                PaymentDiscountApproval papp;

                foreach (string user in users)
                {
                    string[] row = user.Split(':');
                    foreach (string auser in authuser)
                    {
                        if (auser == row[1])
                        {
                            papp = new PaymentDiscountApproval();
                            papp.Days = Convert.ToInt32(row[0]);
                            papp.IsApproved = false;
                            papp.PaymentDiscountID = model.PaymentDiscountID;
                            papp.Remark = "";
                            papp.UpdateDate = DateTime.Now;
                            papp.UserName = row[1];
                            dbContext.PaymentDiscountApprovals.Add(papp);
                            int ii = dbContext.SaveChanges();
                            break;
                        }
                    }
                }
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("Method: AddPaymentDiscount()", ex);
                return 0;
            }
        }
        public int AddPaymentDiscountApprove(PaymentDiscountApprovalModel model)
        {
            try
            {
                Mapper.CreateMap<PaymentDiscountApprovalModel, PaymentDiscountApproval>();
                var pd = Mapper.Map<PaymentDiscountApprovalModel, PaymentDiscountApproval>(model);
                dbContext.PaymentDiscountApprovals.Add(pd);
                int i = dbContext.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("Method: AddPaymentDiscountApprove()", ex);
                return 0;
            }
        }
        public List<PaymentDiscountModel> GetPaymentDisByAdmin(string userName, bool isApproved)
        {
            List<PaymentDiscountModel> pdlist = new List<PaymentDiscountModel>();
            var payD = (from pd in dbContext.PaymentDiscounts join pda in dbContext.PaymentDiscountApprovals on pd.PaymentDiscountID equals pda.PaymentDiscountID where pda.UserName == userName && pda.IsApproved == isApproved select new { PD = pd, Approval = pda });
            foreach (var pay in payD)
            {
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                var pdis = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(pay.PD);
                DataTable dt = dbfunction.GetDataTable("select AppTitle+' '+FName+' '+MName+' '+LName as CustomerName from Customer where SaleID='" + pdis.SaleID + "'");
                DataTable dt2 = dbfunction.GetDataTable("select fl.FlatNo from Flat fl inner join SaleFlat sf on fl.FlatID = sf.FlatID where sf.SaleID='" + pdis.SaleID + "'");
                if (dt.Rows.Count > 0)
                    pdis.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                if (dt2.Rows.Count > 0)
                    pdis.FlatNo = dt2.Rows[0]["FlatNo"].ToString();
                if (pay.PD.ReqDate != null)
                    pdis.ReqDateSt = pay.PD.ReqDate.Value.ToString("dd/MM/yyyy");
                pdis.Days = pay.Approval.Days;
                pdis.AppprovalDate = pay.Approval.UpdateDate;
                pdis.ApprovalRemarks = pay.Approval.Remark;
                pdlist.Add(pdis);
            }
            return pdlist;
        }
        public PaymentDiscountModel GetPaymentDiscount(int paymentDiscountid)
        {
            try
            {
                var model = dbContext.PaymentDiscounts.Where(po => po.PaymentDiscountID == paymentDiscountid).FirstOrDefault();
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                var pd = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(model);
                return pd;
            }
            catch (Exception ex)
            {
                logger.Error("Method: GetPaymentDiscount()", ex);
                return null;
            }
        }
        public List<PaymentDiscountApprovalModel> GetPaymentDiscountApproveList(int paymentDiscountid)
        {
            try
            {
                var model = dbContext.PaymentDiscountApprovals.Where(po => po.PaymentDiscountID == paymentDiscountid).ToList();
                Mapper.CreateMap<PaymentDiscountApproval, PaymentDiscountApprovalModel>();
                var pd = Mapper.Map<List<PaymentDiscountApproval>, List<PaymentDiscountApprovalModel>>(model);
                return pd;
            }
            catch (Exception ex)
            {
                logger.Error("Method: GetPaymentDiscountApproveList()", ex);
                return null;
            }
        }
        public PaymentDiscountApprovalModel GetPaymentDiscountApprove(int paymentDiscountApprovalid)
        {
            try
            {
                var model = dbContext.PaymentDiscountApprovals.Where(po => po.PaymentDiscountApprovalID == paymentDiscountApprovalid).FirstOrDefault();
                Mapper.CreateMap<PaymentDiscountApproval, PaymentDiscountApprovalModel>();
                var pd = Mapper.Map<PaymentDiscountApproval, PaymentDiscountApprovalModel>(model);
                return pd;
            }
            catch (Exception ex)
            {
                logger.Error("Method: GetPaymentDiscountApprove()", ex);
                return null;
            }
        }
        public int UpdateStatus(int paymentdiscountAppid, bool status, string remarks, DateTime updated)
        {
            try
            {
                var model = dbContext.PaymentDiscountApprovals.Where(po => po.PaymentDiscountApprovalID == paymentdiscountAppid).FirstOrDefault();
                model.IsApproved = status;
                model.Remark = remarks;
                model.UpdateDate = updated;
                dbContext.Entry(model).State = EntityState.Modified;
                int i = dbContext.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("Method: UpdateStatus(int paymentdiscountAppid, bool status,string remarks,DateTime updated)", ex);
                return 0;
            }
        }
        public int ApprovePaymentDiscountApprove(int PaymentDiscountID, string Remarks, string userName)
        {
            try
            {
                var model = dbContext.PaymentDiscountApprovals.Where(pda => pda.PaymentDiscountID == PaymentDiscountID && pda.UserName == userName).FirstOrDefault();
                model.Remark = Remarks;
                model.IsApproved = true;
                model.UpdateDate = DateTime.Now;
                dbContext.Entry(model).State = EntityState.Modified;
                int i = dbContext.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                logger.Error("Method:ApprovePaymentDiscount()", ex);
                return 0;
            }
        }
        public PaymentDiscountModel ApprovePaymentDiscount(int paymentDiscountID, string updateby)
        {
            try
            {
                PaymentDiscountModel pdml = new PaymentDiscountModel();
                var model = dbContext.PaymentDiscounts.Where(pda => pda.PaymentDiscountID == paymentDiscountID).FirstOrDefault();
                model.UpdatedBy = updateby;
                model.IsApproved = true;
                model.UpdatedDate = DateTime.Now;
                dbContext.Entry(model).State = EntityState.Modified;
                int i = dbContext.SaveChanges();
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                pdml = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(model);
                return pdml;
            }
            catch (Exception ex)
            {
                logger.Error("Method:ApprovePaymentDiscount()", ex);
                return null;
            }
        }
        public List<PaymentDiscountModel> GetPaymentDiscountByFlat(string flatno, bool IsApproved)
        {
            List<PaymentDiscountModel> pdlist = new List<PaymentDiscountModel>();
            int flatid = Convert.ToInt32(flatno);
            var payD = (from pd in dbContext.PaymentDiscounts join sale in dbContext.SaleFlats on pd.SaleID equals sale.SaleID join flat in dbContext.Flats on sale.FlatID equals flat.FlatID  where flat.FlatID==flatid && pd.IsApproved== IsApproved select new { PD = pd, FlatNo=flat.FlatNo });
            foreach (var pay in payD)
            {
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                var pdis = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(pay.PD);
                DataTable dt = dbfunction.GetDataTable("select AppTitle+' '+FName+' '+MName+' '+LName as CustomerName from Customer where SaleID='" + pdis.SaleID + "'");
                if (dt.Rows.Count > 0)
                    pdis.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                pdis.FlatNo = pay.FlatNo;
                if (pay.PD.ReqDate != null)
                    pdis.ReqDateSt = pay.PD.ReqDate.Value.ToString("dd/MM/yyyy");
                pdlist.Add(pdis);
            }
            return pdlist;
        }
        
        public List<PaymentDiscountModel> GetPaymentDiscounByReqDate(DateTime datefrom, DateTime dateto, bool IsApproved)
        {

            List<PaymentDiscountModel> pdlist = new List<PaymentDiscountModel>();
            var model = dbContext.PaymentDiscounts.Where(st => st.ReqDate >= datefrom && st.ReqDate <= dateto && st.IsApproved==IsApproved).ToList();
            foreach (var pay in model)
            {
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                var pdis = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(pay);
                DataTable dt = dbfunction.GetDataTable("select AppTitle+' '+FName+' '+MName+' '+LName as CustomerName from Customer where SaleID='" + pdis.SaleID + "'");
                DataTable dt2 = dbfunction.GetDataTable("select fl.FlatNo from Flat fl inner join SaleFlat sf on fl.FlatID = sf.FlatID where sf.SaleID='" + pdis.SaleID + "'");
                if (dt.Rows.Count > 0)
                    pdis.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                if (dt2.Rows.Count > 0)
                    pdis.FlatNo = dt2.Rows[0]["FlatNo"].ToString();
                if (pay.ReqDate != null)
                    pdis.ReqDateSt = pay.ReqDate.Value.ToString("dd/MM/yyyy");
                pdlist.Add(pdis);
            }
            return pdlist;
        }
        public List<PaymentDiscountModel> GetPaymentDiscountByApproveDate(DateTime datefrom, DateTime dateto, bool IsApproved)
        {
            List<PaymentDiscountModel> pdlist = new List<PaymentDiscountModel>();
            var model = dbContext.PaymentDiscounts.Where(st => st.UpdatedDate >= datefrom && st.UpdatedDate <= dateto && st.IsApproved==IsApproved).ToList();
            foreach (var pay in model)
            {
                Mapper.CreateMap<PaymentDiscount, PaymentDiscountModel>();
                var pdis = Mapper.Map<PaymentDiscount, PaymentDiscountModel>(pay);
                DataTable dt = dbfunction.GetDataTable("select AppTitle+' '+FName+' '+MName+' '+LName as CustomerName from Customer where SaleID='" + pdis.SaleID + "'");
                DataTable dt2 = dbfunction.GetDataTable("select fl.FlatNo from Flat fl inner join SaleFlat sf on fl.FlatID = sf.FlatID where sf.SaleID='" + pdis.SaleID + "'");
                if (dt.Rows.Count > 0)
                    pdis.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                if (dt2.Rows.Count > 0)
                    pdis.FlatNo = dt2.Rows[0]["FlatNo"].ToString();
                if (pay.ReqDate != null)
                    pdis.ReqDateSt = pay.ReqDate.Value.ToString("dd/MM/yyyy");
                pdlist.Add(pdis);
            }
            return pdlist;
        }
    }
}