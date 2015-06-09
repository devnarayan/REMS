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
    public interface IPlanService
      {
        int AddPlan(Plan model);
        int EditPlan(Plan model);
        int DeletePlan(Plan model);
        PlanModel GetPlanByID(int PlanID);
        List<PlanModel> GetPlanList();
    }
    public class PlanService : IPlanService
    {
        public int AddPlan(Plan model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Plans.Add(model);
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
        public int EditPlan(Plan model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Plans.Add(model);
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
        public int DeletePlan(Plan model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Plans.Add(model);
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
        public PlanModel GetPlanByID(int PlanID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Plans.Where(fl => fl.PlanID == PlanID).FirstOrDefault();
            Mapper.CreateMap<Plan, PlanModel>();
            var mdl = Mapper.Map<Plan, PlanModel>(model);
            return mdl;
        }
        public List<PlanModel> GetPlanList()
        {
            List<PlanModel> fmodel = new List<PlanModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Plans.ToList();
            foreach (Plan fl in model)
            {
                Mapper.CreateMap<Plan, PlanModel>();
                var mdl = Mapper.Map<Plan, PlanModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
   
}
