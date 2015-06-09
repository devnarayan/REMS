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
    public interface IFlatOChargeService
    {
        int AddFlatOCharge(FlatOCharge model);
        int EditFlatOCharge(FlatOCharge model);
        int DeleteFlatOCharge(FlatOCharge model);
        FlatOChargeModel GetFlatOChargeByID(int flatOChargeID);
        List<FlatOChargeModel> GetFlatOChargeListByFlatID(int flatID);
    }
    public class FlatOChargeService : IFlatOChargeService
    {
        public int AddFlatOCharge(FlatOCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatOCharges.Add(model);
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
        public int EditFlatOCharge(FlatOCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatOCharges.Add(model);
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
        public int DeleteFlatOCharge(FlatOCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatOCharges.Add(model);
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
        public FlatOChargeModel GetFlatOChargeByID(int flatOChargeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatOCharges.Where(fl => fl.FlatOChargeID == flatOChargeID).FirstOrDefault();
            Mapper.CreateMap<FlatOCharge, FlatOChargeModel>();
            var mdl = Mapper.Map<FlatOCharge, FlatOChargeModel>(model);
            mdl.Amount = model.AddOnCharge.Amount;
            mdl.ChargeName = model.AddOnCharge.Name;
            return mdl;
        }
        public List<FlatOChargeModel> GetFlatOChargeListByFlatID(int flatID)
        {
            List<FlatOChargeModel> fmodel = new List<FlatOChargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatOCharges.Where(fl => fl.FlatID == flatID).ToList();
            foreach (FlatOCharge fl in model)
            {
                Mapper.CreateMap<FlatOCharge, FlatOChargeModel>();
                var mdl = Mapper.Map<FlatOCharge, FlatOChargeModel>(fl);
                mdl.Amount = fl.AddOnCharge.Amount;
                mdl.ChargeName = fl.AddOnCharge.Name;
                mdl.ChargeType = fl.AddOnCharge.ChargeType;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}
