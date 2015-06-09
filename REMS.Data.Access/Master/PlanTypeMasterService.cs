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
    public interface IPlanTypeMasterService
    {
        int AddPlanTypeMaster(PlanTypeMaster model);
        int EditPlanTypeMaster(PlanTypeMaster model);
        int DeletePlanTypeMaster(PlanTypeMaster model);
        PlanTypeMasterModel GetPlanTypeMasterByID(int planTypeMasterID);
        PlanTypeMasterModel GetPlanTypeMasterByParams(string PlanName, string FType, decimal? Size);
        List<PlanTypeMasterModel> GetPlanTypeMasterList();
        List<PlanTypeMasterModel> GetPlanTypeDistModel();
    }
    public class PlanTypeMasterService : IPlanTypeMasterService
    {
        public int AddPlanTypeMaster(PlanTypeMaster model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.PlanTypeMasters.Add(model);
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
        public int EditPlanTypeMaster(PlanTypeMaster model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.PlanTypeMasters.Add(model);
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
        public int DeletePlanTypeMaster(PlanTypeMaster model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.PlanTypeMasters.Add(model);
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
        public PlanTypeMasterModel GetPlanTypeMasterByID(int planTypeMasterID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanTypeMasters.Where(fl => fl.PlanTypeMasterID == planTypeMasterID).FirstOrDefault();
            Mapper.CreateMap<PlanTypeMaster, PlanTypeMasterModel>();
            var mdl = Mapper.Map<PlanTypeMaster, PlanTypeMasterModel>(model);
            mdl.FType = model.FlatTypeSize.FlatType.FType;
            mdl.Size = model.FlatTypeSize.Size;
            return mdl;
        }
        public PlanTypeMasterModel GetPlanTypeMasterByParams(string PlanName, string FType, decimal? Size)
        {
            REMSDBEntities context = new REMSDBEntities();
            FlatTypeSizeService ftservice = new FlatTypeSizeService();
            var sizemodel = ftservice.GetFlatTypeSizeParam(FType, Size);
            var model = context.PlanTypeMasters.Where(fl => fl.FlatTypeSizeID == sizemodel.FlatTypeSizeID && fl.PlanName == PlanName).FirstOrDefault();
            if (model == null)
            {
                return null;
            }
            else
            {
                Mapper.CreateMap<PlanTypeMaster, PlanTypeMasterModel>();
                var mdl = Mapper.Map<PlanTypeMaster, PlanTypeMasterModel>(model);
                mdl.FType = model.FlatTypeSize.FlatType.FType;
                mdl.Size = model.FlatTypeSize.Size;
                return mdl;
            }
        }
        public List<PlanTypeMasterModel> GetPlanTypeMasterList()
        {
            List<PlanTypeMasterModel> fmodel = new List<PlanTypeMasterModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanTypeMasters.ToList();
            foreach (PlanTypeMaster fl in model)
            {
                Mapper.CreateMap<PlanTypeMaster, PlanTypeMasterModel>().ForMember(dest => dest.FType, sec => sec.MapFrom(fs => fs.FlatTypeSize.FlatType.FType)).ForMember(dest => dest.Size, sec => sec.MapFrom(fs => fs.FlatTypeSize.Size));
                var mdl = Mapper.Map<PlanTypeMaster, PlanTypeMasterModel>(fl);
                mdl.PlanSection = fl.Plan.PlanName;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
        public List<PlanTypeMasterModel> GetPlanTypeDistModel()
        {
            List<PlanTypeMasterModel> fmodel = new List<PlanTypeMasterModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.PlanTypeMasters.GroupBy(gb=>gb.PlanName).Select(se=>se.FirstOrDefault()).ToList();
            foreach (PlanTypeMaster fl in model)
            {
                Mapper.CreateMap<PlanTypeMaster, PlanTypeMasterModel>().ForMember(dest => dest.FType, sec => sec.MapFrom(fs => fs.FlatTypeSize.FlatType.FType)).ForMember(dest => dest.Size, sec => sec.MapFrom(fs => fs.FlatTypeSize.Size));
                var mdl = Mapper.Map<PlanTypeMaster, PlanTypeMasterModel>(fl);
                mdl.PlanSection = fl.Plan.PlanName;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }

}