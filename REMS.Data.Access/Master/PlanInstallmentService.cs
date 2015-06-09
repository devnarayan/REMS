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
    public interface IPlanInstallmentService
    {
        int AddPlanInstallment(PlanInstallmentModel model);
        int EditPlanInstallment(PlanInstallmentModel model);
        int DeletePlanInstallment(PlanInstallment model);
        PlanInstallmentModel GetPlanInstallmentByID(int PlanInstallmentID);
        List<PlanInstallmentModel> GetPlanInstallmentByPlanID(int planID);
        List<PlanInstallmentModel> GetPlanInstallmentList();
    }
    public class PlanInstallmentService : IPlanInstallmentService
    {
        public int AddPlanInstallment(PlanInstallmentModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<PlanInstallmentModel, PlanInstallment>();
                   var mdl= Mapper.Map<PlanInstallmentModel, PlanInstallment>(model);
                    context.PlanInstallments.Add(mdl);
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
        public int EditPlanInstallment(PlanInstallmentModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<PlanInstallmentModel, PlanInstallment>();
                    var mdl = Mapper.Map<PlanInstallmentModel, PlanInstallment>(model);
                    context.PlanInstallments.Add(mdl);
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
        public int DeletePlanInstallment(PlanInstallment model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.PlanInstallments.Add(model);
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
        public PlanInstallmentModel GetPlanInstallmentByID(int PlanInstallmentID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanInstallments.Where(fl => fl.PlanInstallmentID == PlanInstallmentID).FirstOrDefault();
            Mapper.CreateMap<PlanInstallment, PlanInstallmentModel>();
            var mdl = Mapper.Map<PlanInstallment, PlanInstallmentModel>(model);
            return mdl;
        }
        public List<PlanInstallmentModel> GetPlanInstallmentByPlanID(int planID)
        {
            List<PlanInstallmentModel> fmodel = new List<PlanInstallmentModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanInstallments.Where(ds=>ds.PlanID==planID).ToList();
            string dm = model[1].Plan.PlanName;
            foreach (PlanInstallment fl in model)
            {
                Mapper.CreateMap<PlanInstallment, PlanInstallmentModel>();
                var mdl = Mapper.Map<PlanInstallment, PlanInstallmentModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
        public List<PlanInstallmentModel> GetPlanInstallmentList()
        {
            List<PlanInstallmentModel> fmodel = new List<PlanInstallmentModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanInstallments.ToList();
            foreach (PlanInstallment fl in model)
            {
                Mapper.CreateMap<PlanInstallment, PlanInstallmentModel>();
                var mdl = Mapper.Map<PlanInstallment, PlanInstallmentModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}