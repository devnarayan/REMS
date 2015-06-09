using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Admin
{
    public interface IFlatChargeService
    {
        int AddFlatCharge(FlatCharge model);
        int EditFlatCharge(FlatCharge model);
        int DeleteFlatCharge(FlatCharge model);
        FlatChargeModel GetFlatChargeByID(int flatChargeID);
        List<FlatChargeModel> GetFlatChargeListByFlatID(int flatID);
    }
    public class FlatChargeService : IFlatChargeService
    {
        public int AddFlatCharge(FlatCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatCharges.Add(model);
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
        public int EditFlatCharge(FlatCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatCharges.Add(model);
                    context.Entry(model).State = EntityState.Modified;
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
        public int DeleteFlatCharge(FlatCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatCharges.Add(model);
                    context.Entry(model).State = EntityState.Deleted;
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
        public FlatChargeModel GetFlatChargeByID(int flatChargeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatCharges.Where(fl => fl.FlatChargeID == flatChargeID).FirstOrDefault();
            Mapper.CreateMap<FlatCharge, FlatChargeModel>();
           var mdl= Mapper.Map<FlatCharge, FlatChargeModel>(model);
           mdl.Amount = model.AdditionalCharge.Amount;
           mdl.ChargeName = model.AdditionalCharge.Name;
           return mdl;
        }
        public List<FlatChargeModel> GetFlatChargeListByFlatID(int flatID)
        {
            List<FlatChargeModel> fmodel = new List<FlatChargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatCharges.Where(fl => fl.FlatID ==flatID).ToList();
            foreach (FlatCharge fl in model)
            {
                Mapper.CreateMap<FlatCharge, FlatChargeModel>();
                var mdl = Mapper.Map<FlatCharge, FlatChargeModel>(fl);
                mdl.Amount = fl.AdditionalCharge.Amount;
                mdl.ChargeName = fl.AdditionalCharge.Name;
                mdl.ChargeType = fl.AdditionalCharge.ChargeType;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}
