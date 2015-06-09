using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Master
{
    public interface IAOChargeService
    {
        int AddAddOnCharge(AddOnCharge model);
        int EditAddOnCharge(AddOnCharge model);
        int DeleteAddOnCharge(AddOnCharge model);
        AddOnchargeModel GetAddOnChargeByID(int AddOnChargeID);
        List<AddOnchargeModel> GetAddOnChargeList();
        List<AddOnchargeModel> GetMandatoryAddOnChargeList();
    }
    public class AOChargeService : IAOChargeService
    {
        public int AddAddOnCharge(AddOnCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.AddOnCharges.Add(model);
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
        public int EditAddOnCharge(AddOnCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.AddOnCharges.Add(model);
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
        public int DeleteAddOnCharge(AddOnCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.AddOnCharges.Add(model);
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
        public AddOnchargeModel GetAddOnChargeByID(int AddOnChargeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AddOnCharges.Where(fl => fl.AddOnChargeID == AddOnChargeID).FirstOrDefault();
            Mapper.CreateMap<AddOnCharge, AddOnchargeModel>();
            var mdl = Mapper.Map<AddOnCharge, AddOnchargeModel>(model);
            return mdl;
        }
        public List<AddOnchargeModel> GetAddOnChargeList()
        {
            List<AddOnchargeModel> fmodel = new List<AddOnchargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AddOnCharges.ToList();
            foreach (AddOnCharge fl in model)
            {
                Mapper.CreateMap<AddOnCharge, AddOnchargeModel>();
                var mdl = Mapper.Map<AddOnCharge, AddOnchargeModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
        public List<AddOnchargeModel> GetMandatoryAddOnChargeList()
        {
            List<AddOnchargeModel> fmodel = new List<AddOnchargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AddOnCharges.Where(fl=>fl.Mandatory==true).ToList();
            foreach (AddOnCharge fl in model)
            {
                Mapper.CreateMap<AddOnCharge, AddOnchargeModel>();
                var mdl = Mapper.Map<AddOnCharge, AddOnchargeModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}
