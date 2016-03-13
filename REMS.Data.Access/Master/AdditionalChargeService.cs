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
    public interface IAdditionalChargeService
    {
        int AddAdditionalCharge(AdditionalCharge model);
        int EditAdditionalCharge(AdditionalCharge model);
        AdditionalChargeModel GetAdditionalChargeByID(int AdditionalChargeID);
        List<AdditionalChargeModel> GetAdditionalChargeList();
        List<AdditionalChargeModel> GetMandatoryAdditionalChargeList();
    }
    public class AdditionalChargeService : IAdditionalChargeService
    {
        public int AddAdditionalCharge(AdditionalCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.AdditionalCharges.Add(model);
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
        public int EditAdditionalCharge(AdditionalCharge model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.AdditionalCharges.Add(model);
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
        public int DeleteAdditionalCharge(int model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    var mdl = context.AdditionalCharges.Where(ad => ad.AdditionalChargeID == model).FirstOrDefault();
                    context.Entry(mdl).State = EntityState.Deleted;
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
        public AdditionalChargeModel GetAdditionalChargeByID(int AdditionalChargeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AdditionalCharges.Where(fl => fl.AdditionalChargeID == AdditionalChargeID).FirstOrDefault();
            Mapper.CreateMap<AdditionalCharge, AdditionalChargeModel>();
            var mdl = Mapper.Map<AdditionalCharge, AdditionalChargeModel>(model);
            return mdl;
        }
        public List<AdditionalChargeModel> GetAdditionalChargeList()
        {
            List<AdditionalChargeModel> fmodel = new List<AdditionalChargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AdditionalCharges.ToList();
            foreach (AdditionalCharge fl in model)
            {
                Mapper.CreateMap<AdditionalCharge, AdditionalChargeModel>();
                var mdl = Mapper.Map<AdditionalCharge, AdditionalChargeModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
        public List<AdditionalChargeModel> GetMandatoryAdditionalChargeList()
        {
            List<AdditionalChargeModel> fmodel = new List<AdditionalChargeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.AdditionalCharges.Where(ad=>ad.Mandatory==true).ToList();
            foreach (AdditionalCharge fl in model)
            {
                Mapper.CreateMap<AdditionalCharge, AdditionalChargeModel>();
                var mdl = Mapper.Map<AdditionalCharge, AdditionalChargeModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}
