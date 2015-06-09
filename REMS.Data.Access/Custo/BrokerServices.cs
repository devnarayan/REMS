using AutoMapper;
using REMS.Data.Broker;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
    public interface IBrokerServices
    {
        string SaveBroker(BrokerModel broker);
        List<BrokerModel> SearchBroker(string search, string query);
        IQueryable<BrokerMaster> GetAllBroker();
        List<BrokerToPropertyModel> GetBrokerBySaleID(int saleid);
        List<BrokerToPropertyModel> GetBrokerByBrokerToPropertyID(int pid);
        BrokerMaster GetBrokerByBrokerID(int bid);
        List<BrokerToPropertyModel> GetBrokersProperties(int brokerid);
        List<BrokerToPropertyModel> GetBrokersPropertiesSearch(int brokerid, string Status);
        List<BrokerPaymentModel> GetPaidAmountToBrokerByBrokerID(int brokerID);
        List<BrokerPaymentModel> GetPaidAmountToBrokerSearch(int brokerID, string Status);
        string AttachBrokerToProperty(string brokerid, string amount, string date, string remarks,int FlatID, string SaleID);
        string AttachBrokerToPropertyUpdate(int brokerToPropertyID, string brokerid, string amount, string date, string remarks, int PropertyID, int FlatID, string SaleID);
        string DeleteBrokerToPropertyID(int pid);
        string UpdatePropertyStatus(int saleid, string status);
        string UpdatePropertyStatusByID(int id, string status);

    }
    public  class BrokerServices : IBrokerServices
    {
        public string SaveBroker(BrokerModel broker)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                Mapper.CreateMap<BrokerModel, BrokerMaster>();
                var model = Mapper.Map<BrokerModel, BrokerMaster>(broker);
                context.BrokerMasters.Add(model);
                model.RecordStatus = 1;
                model.CreateDate = DateTime.Now;
                int i = context.SaveChanges();
                if (i > 0)
                {
                    return "Yes";
                }
                else
                    return "No";
            }
        }
        public int UpdateBroker(BrokerModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<BrokerModel, BrokerMaster>();
                    var mdl = Mapper.Map<BrokerModel, BrokerMaster>(model);
                    context.BrokerMasters.Add(mdl);                   
                    context.Entry(mdl).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public List<BrokerModel> SearchBroker(string search, string query)
        {
            if (search == "? undefined:undefined ?" || search == "All") search = "All";
            if (query == "? undefined:undefined ?" || search == "All") query = "";

            REMSDBEntities context = new REMSDBEntities();
            List<BrokerModel> md = new List<BrokerModel>();
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            if (search == "All")
            {
                var model = context.BrokerMasters.AsEnumerable();
                foreach (var m in model)
                {
                    Mapper.CreateMap<BrokerMaster, BrokerModel>();
                    var mdl = Mapper.Map<BrokerMaster, BrokerModel>(m);
                    if (m.CreateDate != null)
                        mdl.CreateDateSt = m.CreateDate.Value.ToString("dd/MM/yyyy");
                    md.Add(mdl);
                }
            }
            else if (search == "Name")
            {
                var model = context.BrokerMasters.Where(br => br.BrokerName.Contains(query)).AsEnumerable();
                foreach (var m in model)
                {
                    Mapper.CreateMap<BrokerMaster, BrokerModel>();
                    var mdl = Mapper.Map<BrokerMaster, BrokerModel>(m);
                    if (m.CreateDate != null)
                        mdl.CreateDateSt = m.CreateDate.Value.ToString("dd/MM/yyyy");
                    md.Add(mdl);
                }
            }
            else if (search == "MobileNo")
            {
                var model = context.BrokerMasters.Where(br => br.MobileNo.Contains(query)).AsEnumerable();
                foreach (var m in model)
                {
                    Mapper.CreateMap<BrokerMaster, BrokerModel>();
                    var mdl = Mapper.Map<BrokerMaster, BrokerModel>(m);
                    if (m.CreateDate != null)
                        mdl.CreateDateSt = m.CreateDate.Value.ToString("dd/MM/yyyy");
                    md.Add(mdl);
                }
            }
            else if (search == "PanNo")
            {
                var model = context.BrokerMasters.Where(br => br.PanNo.Contains(query)).AsEnumerable();
                foreach (var m in model)
                {
                    Mapper.CreateMap<BrokerMaster, BrokerModel>();
                    var mdl = Mapper.Map<BrokerMaster, BrokerModel>(m);
                    if (m.CreateDate != null)
                        mdl.CreateDateSt = m.CreateDate.Value.ToString("dd/MM/yyyy");
                    md.Add(mdl);
                }
            }
            return md;
        }
        public IQueryable<BrokerMaster> GetAllBroker()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.BrokerMasters.Where(br => br.RecordStatus == 1);
            return model;
        }
       public int? GetBrokerIdBysaleId(int saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int? BrokerId = context.BrokerToProperties.Where(br => br.SaleID == saleid).FirstOrDefault().BrokerID;
            return BrokerId;

        }
        public List<BrokerToPropertyModel> GetBrokerBySaleID(int saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID where bp.SaleID == saleid select new { Broker = bro, BP = bp });
            List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
            foreach (var mdl in md)
            {
                string bdate = "";
                if (mdl.BP.Date != null)
                    bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID });
            }
            return model;
        }
        public List<BrokerToPropertyModel> GetBrokerByFlatID(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID where bp.FlatID == flatid select new { Broker = bro, BP = bp });
            List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
            foreach (var mdl in md)
            {
                string bdate = "";
                if (mdl.BP.Date != null)
                    bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID });
            }
            return model;
        }
        public List<BrokerToPropertyModel> GetBrokerByBrokerToPropertyID(int pid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID where bp.BrokerToPropertyID == pid select new { Broker = bro, BP = bp });
            List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
            foreach (var mdl in md)
            {
                string bdate = "";
                if (mdl.BP.Date != null)
                    bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                model.Add(new BrokerToPropertyModel { ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID });
            }
            return model;
        }
        public BrokerMaster GetBrokerByBrokerID(int bid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.BrokerMasters.Where(br => br.RecordStatus == 1 && br.BrokerID == bid).FirstOrDefault();
            return model;
        }
        public List<BrokerToPropertyModel> GetBrokersProperties(int brokerid)
        {
            REMSDBEntities context = new REMSDBEntities();
            if (brokerid == 0)
            {
                var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID select new { Broker = bro, BP = bp, Pro = flat });
                List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
                foreach (var mdl in md)
                {
                    string bdate = "";
                    if (mdl.BP.Date != null)
                        bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                    model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                }
                return model;
            }
            else
            {
                var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID where bro.BrokerID == brokerid select new { Broker = bro, BP = bp, Pro = flat });
                List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
                foreach (var mdl in md)
                {
                    string bdate = "";
                    if (mdl.BP.Date != null)
                        bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                    model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                }
                return model;
            }

        }
        public List<BrokerToPropertyModel> GetBrokersPropertiesSearch(int brokerid, string Status)
        {
            List<BrokerToPropertyModel> model = new List<BrokerToPropertyModel>();
            REMSDBEntities context = new REMSDBEntities();
            if (brokerid != 0)
            {
                if (Status == "All")
                {
                    var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID where bro.BrokerID == brokerid select new { Broker = bro, BP = bp, Pro = flat });
                    foreach (var mdl in md)
                    {
                        string bdate = "";
                        if (mdl.BP.Date != null)
                            bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                        model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                    }

                }
                else
                {
                    var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID where bro.BrokerID == brokerid && bp.Status == Status select new { Broker = bro, BP = bp, Pro = flat });
                    foreach (var mdl in md)
                    {
                        string bdate = "";
                        if (mdl.BP.Date != null)
                            bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                        model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                    }
                }
                return model;
            }
            else
            {

                if (Status == "All")
                {
                    var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID select new { Broker = bro, BP = bp, Pro = flat });
                    foreach (var mdl in md)
                    {
                        string bdate = "";
                        if (mdl.BP.Date != null)
                            bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                        model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                    }

                }
                else
                {
                    var md = (from bro in context.BrokerMasters join bp in context.BrokerToProperties on bro.BrokerID equals bp.BrokerID join flat in context.Flats on bp.FlatID equals flat.FlatID where bp.Status == Status select new { Broker = bro, BP = bp, Pro = flat });
                    foreach (var mdl in md)
                    {
                        string bdate = "";
                        if (mdl.BP.Date != null)
                            bdate = mdl.BP.Date.Value.ToString("dd/MM/yyyy");
                        model.Add(new BrokerToPropertyModel { Status = mdl.BP.Status, ApproveStatus = mdl.BP.ApproveStatus, BrokerAmount = mdl.BP.BrokerAmount, BrokerID = mdl.BP.BrokerID, BrokerName = mdl.Broker.BrokerName, BrokerToPropertyID = mdl.BP.BrokerToPropertyID, Createdate = mdl.BP.Createdate, Date = mdl.BP.Date, DateSt = bdate, FlatID = mdl.BP.FlatID, ModifyDate = mdl.BP.ModifyDate, PID = mdl.BP.PID, RecordStatus = mdl.BP.RecordStatus, Remarks = mdl.BP.Remarks, SaleID = mdl.BP.SaleID, PropertyName = mdl.Pro.FlatName });
                    }
                }
                return model;
            }
        }
        public List<BrokerPaymentModel> GetPaidAmountToBrokerByBrokerID(int brokerID)
        {
            if (brokerID != 0)
            {
                REMSDBEntities context = new REMSDBEntities();
                var model = context.BrokerPayments.Where(br => br.BrokerID == brokerID).ToList();
                List<BrokerPaymentModel> bmodel = new List<BrokerPaymentModel>();
                foreach (var md in model)
                {
                    var pro = context.Flats.Where(f => f.FlatID == md.FlatID).FirstOrDefault();

                    Mapper.CreateMap<BrokerPayment, BrokerPaymentModel>();
                    var mdl = Mapper.Map<BrokerPayment, BrokerPaymentModel>(md);
                    mdl.PropertyName = "";
                    if (md.PaidDate != null)
                        mdl.PaymentDateSt = md.PaidDate.Value.ToString("dd/MM/yyyy");
                    if (md.ChequeDate != null)
                        mdl.ChequeDateSt = md.ChequeDate.Value.ToString("dd/MM/yyyy");
                    bmodel.Add(mdl);
                }
                return bmodel;
            }
            else
            {
                REMSDBEntities context = new REMSDBEntities();
                var model = context.BrokerPayments.ToList();
                List<BrokerPaymentModel> bmodel = new List<BrokerPaymentModel>();
                foreach (var md in model)
                {
                    var pro = context.Flats.Where(f => f.FlatID == md.FlatID).FirstOrDefault();

                    Mapper.CreateMap<BrokerPayment, BrokerPaymentModel>();
                    var mdl = Mapper.Map<BrokerPayment, BrokerPaymentModel>(md);
                    mdl.PropertyName = "";
                    if (md.PaidDate != null)
                        mdl.PaymentDateSt = md.PaidDate.Value.ToString("dd/MM/yyyy");
                    if (md.ChequeDate != null)
                        mdl.ChequeDateSt = md.ChequeDate.Value.ToString("dd/MM/yyyy");
                    bmodel.Add(mdl);
                }
                return bmodel;
            }
        }
        public List<BrokerPaymentModel> GetPaidAmountToBrokerSearch(int brokerID, string Status)
        {
            REMSDBEntities context = new REMSDBEntities();
            List<BrokerPaymentModel> bmodel = new List<BrokerPaymentModel>();
            if (Status == "All")
            {
                var model = context.BrokerPayments.Where(br => br.BrokerID == brokerID).ToList();
                foreach (var md in model)
                {
                    var pro = context.Flats.Where(f => f.FlatID == md.FlatID).FirstOrDefault();
                    Mapper.CreateMap<BrokerPayment, BrokerPaymentModel>();
                    var mdl = Mapper.Map<BrokerPayment, BrokerPaymentModel>(md);
                    mdl.PropertyName = pro.FlatName;
                    if (md.PaidDate != null)
                        mdl.PaymentDateSt = md.PaidDate.Value.ToString("dd/MM/yyyy");
                    if (md.ChequeDate != null)
                        mdl.ChequeDateSt = md.ChequeDate.Value.ToString("dd/MM/yyyy");
                    bmodel.Add(mdl);
                }
            }
            else
            {
                var model = context.BrokerPayments.Where(br => br.BrokerID == brokerID && br.Status == Status).ToList();
                foreach (var md in model)
                {
                    var pro = context.Flats.Where(f => f.FlatID == md.FlatID).FirstOrDefault();
                    Mapper.CreateMap<BrokerPayment, BrokerPaymentModel>();
                    var mdl = Mapper.Map<BrokerPayment, BrokerPaymentModel>(md);
                    mdl.PropertyName = pro.FlatName;
                    if (md.PaidDate != null)
                        mdl.PaymentDateSt = md.PaidDate.Value.ToString("dd/MM/yyyy");
                    if (md.ChequeDate != null)
                        mdl.ChequeDateSt = md.ChequeDate.Value.ToString("dd/MM/yyyy");
                    bmodel.Add(mdl);
                }
            }

            return bmodel;
        }
        public string AttachBrokerToProperty(string brokerid, string amount, string date, string remarks, int FlatID, string SaleID)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                int bid = Convert.ToInt32(brokerid);
                int sid = Convert.ToInt32(SaleID);
                BrokerToProperty bp = new BrokerToProperty();
                bp.BrokerID = bid;
                bp.ApproveStatus = 0;
                bp.BrokerAmount = Convert.ToDecimal(amount);
                bp.Createdate = DateTime.Now;
                bp.Date = Convert.ToDateTime(date, dtinfo);
                bp.SaleID = sid;
                bp.Remarks = remarks;
                bp.FlatID = FlatID;
                //bp.PID = PropertyID;
                bp.Status = "Pending";
                context.BrokerToProperties.Add(bp);
                int i = context.SaveChanges();
                if (i > 0)
                    return "Yes";
                else
                    return "No";
            }
        }
        public string AttachBrokerToPropertyUpdate(int brokerToPropertyID, string brokerid, string amount, string date, string remarks, int PropertyID, int FlatID, string SaleID)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                BrokerToProperty bp = context.BrokerToProperties.Where(b => b.BrokerToPropertyID == brokerToPropertyID).FirstOrDefault();

                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                int bid = Convert.ToInt32(brokerid);
                int sid = Convert.ToInt32(SaleID);
                bp.BrokerID = bid;
                bp.ApproveStatus = 0;
                bp.BrokerAmount = Convert.ToDecimal(amount);
                bp.Createdate = DateTime.Now;
                bp.Date = Convert.ToDateTime(date, dtinfo);
                bp.SaleID = sid;
                bp.Remarks = remarks;
                bp.FlatID = FlatID;
                bp.PID = PropertyID;

                context.BrokerToProperties.Add(bp);
                context.Entry(bp).State = EntityState.Modified;
                int i = context.SaveChanges();
                if (i > 0)
                    return "Yes";
                else
                    return "No";
            }
        }
        public string DeleteBrokerToPropertyID(int pid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                BrokerToProperty bp = context.BrokerToProperties.Where(b => b.BrokerToPropertyID == pid).FirstOrDefault();

                context.BrokerToProperties.Add(bp);
                context.Entry(bp).State = EntityState.Deleted;
                int i = context.SaveChanges();
                if (i > 0)
                    return "Yes";
                else
                    return "No";
            }
        }
        public string PaymentBrokerToProperty(int FlatID, int SaleID, int BrokerID, string PaidDate, string AmountPiad, int PID, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BranchName, string Remarks)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                BrokerPayment bp = new BrokerPayment();
                bp.FlatID = FlatID;
                bp.SaleID = SaleID;
                bp.BrokerID = BrokerID;
                bp.PaidDate = Convert.ToDateTime(PaidDate, dtinfo);
                bp.AmountPaid = Convert.ToDecimal(AmountPiad);
                bp.PID = PID;
                bp.PaymentMode = PaymentMode;
                bp.ChequeNo = ChequeNo;
                if (ChequeDate != "")
                    bp.ChequeDate = Convert.ToDateTime(ChequeDate, dtinfo);
                bp.BankName = BankName;
                bp.BankBranch = BranchName;
                bp.Remarks = Remarks;
                bp.CreateDate = DateTime.Now;
                bp.RecordStatus = 0;
                bp.Status = "Pending";

                context.BrokerPayments.Add(bp);
                int i = context.SaveChanges();
                if (i > 0)
                    return "Yes";
                else
                    return "No";
            }
        }
        public string UpdatePropertyStatus(int saleid, string status)
        {
            try
            {
                using (var ctx = new REMSDBEntities())
                {
                    var stud = (from s in ctx.BrokerToProperties where s.SaleID == saleid select s).FirstOrDefault();

                    stud.Status = status;
                    ctx.Entry(stud).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return "Success";

                }


            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }
        public string UpdatePropertyStatusByID(int id, string status)
        {
            try
            {
                using (var ctx = new REMSDBEntities())
                {
                    var stud = (from s in ctx.BrokerToProperties where s.BrokerToPropertyID == id select s).FirstOrDefault();

                    stud.Status = status;
                    ctx.Entry(stud).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return "Success";

                }


            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }
   
    }
}
