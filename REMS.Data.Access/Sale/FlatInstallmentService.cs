using AutoMapper;
using REMS.Data.Access.Admin;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Sale
{
    public interface IFlatInstallmentService
    {
        int AddFlatInstallment(FlatInstallmentDetailModel flatModel);
        int DeleteFlatInstallment(int flatid);
        List<FlatInstallmentDetailModel> GetFlatInstallment(int flatid);
        List<FlatInstallmentDetailModel> GetFlatInstallmentWithCharges(int flatid, decimal flatsize);
    }

    public class FlatInstallmentService : IFlatInstallmentService
    {
        #region Private Fields
        private readonly REMSDBEntities dbContext;
        #endregion
        public FlatInstallmentService()
        {
            dbContext = new REMSDBEntities();
        }
        public int AddFlatInstallment(FlatInstallmentDetailModel flatModel)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<FlatInstallmentDetailModel, FlatInstallmentDetail>();
                    var model = Mapper.Map<FlatInstallmentDetailModel, FlatInstallmentDetail>(flatModel);
                    context.FlatInstallmentDetails.Add(model);
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

        public int DeleteFlatInstallment(int flatid)
        {
            var model = dbContext.FlatInstallmentDetails.Where(fl => fl.FlatID == flatid).ToList();
            int i = 0;
            foreach (var md in model)
            {
                dbContext.FlatInstallmentDetails.Add(md);
                dbContext.Entry(md).State = EntityState.Deleted;
                i = dbContext.SaveChanges();
            }
            return i;
        }
        public List<FlatInstallmentDetailModel> GetFlatInstallment(int flatid)
        {
            var mdl = dbContext.FlatInstallmentDetails.Where(fl => fl.FlatID == flatid).ToList();
            Mapper.CreateMap<FlatInstallmentDetail, FlatInstallmentDetailModel>().ForMember(ds => ds.Installment, sc => sc.MapFrom(ds => ds.PlanInstallment.Installment));
            var model = Mapper.Map<List<FlatInstallmentDetail>, List<FlatInstallmentDetailModel>>(mdl);
            return model;
        }
        public List<FlatInstallmentDetailModel> GetFlatInstallmentWithCharges(int flatid, decimal flatsize)
        {
            var mdl = dbContext.FlatInstallmentDetails.Where(fl => fl.FlatID == flatid).ToList();
            FlatPLCService fps = new FlatPLCService();
            FlatChargeService fcs = new FlatChargeService();
            FlatOChargeService fos = new FlatOChargeService();
            var plcm = fps.TotalFlatPLCAmount(flatid);
            var totalplc = plcm * flatsize;

            var fcmodel = fcs.GetFlatChargeListByFlatID(flatid);
            decimal? chargetotal = 0;
            foreach (var md in fcmodel)
            {
                if (md.ChargeType == "One Time")
                {
                    chargetotal = chargetotal + md.Amount;
                }
                else if (md.ChargeType == "Sq. Ft.")
                {
                    chargetotal = chargetotal + md.Amount * flatsize;
                }
            }

            var fomodel = fos.GetFlatOChargeListByFlatID(flatid);
            decimal? totalocharge = 0;
            foreach (var md in fomodel)
            {
                if (md.ChargeType == "One Time")
                {
                    totalocharge = totalocharge + md.Amount;
                }
                else if (md.ChargeType == "Sq. Ft.")
                {
                    totalocharge = totalocharge + md.Amount * flatsize;
                }
            }
            List<FlatInstallmentDetailModel> modl = new List<FlatInstallmentDetailModel>();
            foreach (var md in mdl)
            {
                Mapper.CreateMap<FlatInstallmentDetail, FlatInstallmentDetailModel>().ForMember(ds => ds.Installment, sc => sc.MapFrom(ds => ds.PlanInstallment.Installment));
                var model = Mapper.Map<FlatInstallmentDetail, FlatInstallmentDetailModel>(md);
                model.PLCAmount = totalplc * model.PLCPer / 100;
                model.AdditionalCAmount = chargetotal * model.AdditionalPer / 100;
                model.OptionalCAmount = totalocharge * model.OptionalPer / 100;
                if (model.DueDate != null)
                    model.DueDateSt = model.DueDate.Value.ToString("dd/MM/yyyy");
                else model.DueDateSt = "";
                modl.Add(model);
            }
            return modl;
        }
    }
}
