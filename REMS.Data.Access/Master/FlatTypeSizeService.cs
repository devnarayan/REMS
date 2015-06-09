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
    public interface IFlatTypeSizeService
    {
        int AddFlatTypeSize(FlatTypeSize model);
        int EditFlatTypeSize(FlatTypeSize model);
        FlatTypeSizeModel GetFlatTypeSizeByID(int FlatTypeSizeID);
        FlatTypeSizeModel GetFlatTypeSizeParam(string  FlatType,decimal? size);
        List<FlatTypeSizeModel> GetFlatTypeSizeListByFlattypeid(int flattypeid);
        List<FlatTypeSizeModel> GetFlatTypeSizeListByFlattypeName(string ftype);
    }
    public class FlatTypeSizeService : IFlatTypeSizeService
    {
        public int AddFlatTypeSize(FlatTypeSize model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypeSizes.Add(model);
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
        public int EditFlatTypeSize(FlatTypeSize model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypeSizes.Add(model);
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
        public int DeleteFlatTypeSize(FlatTypeSize model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypeSizes.Add(model);
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
        public FlatTypeSizeModel GetFlatTypeSizeByID(int FlatTypeSizeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypeSizes.Where(fl => fl.FlatTypeSizeID == FlatTypeSizeID).FirstOrDefault();
            Mapper.CreateMap<FlatTypeSize, FlatTypeSizeModel>();
            var mdl = Mapper.Map<FlatTypeSize, FlatTypeSizeModel>(model);
            mdl.FType = model.FlatType.FType;
            return mdl;
        }
       public FlatTypeSizeModel GetFlatTypeSizeParam(string FType, decimal? size)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypeSizes.Where(fl => fl.FlatType.FType == FType && fl.Size==size).FirstOrDefault();
            Mapper.CreateMap<FlatTypeSize, FlatTypeSizeModel>();
            var mdl = Mapper.Map<FlatTypeSize, FlatTypeSizeModel>(model);
            mdl.FType = model.FlatType.FType;
            return mdl;
        }
        public List<FlatTypeSizeModel> GetFlatTypeSizeListByFlattypeid(int flattypeid)
        {
            List<FlatTypeSizeModel> fmodel = new List<FlatTypeSizeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypeSizes.Where(fl=>fl.FlatTypeID==flattypeid).ToList();
            foreach (FlatTypeSize fl in model)
            {
                Mapper.CreateMap<FlatTypeSize, FlatTypeSizeModel>();
                var mdl = Mapper.Map<FlatTypeSize, FlatTypeSizeModel>(fl);
                mdl.FType = fl.FlatType.FType;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
        public List<FlatTypeSizeModel> GetFlatTypeSizeListByFlattypeName(string ftype)
        {
            List<FlatTypeSizeModel> fmodel = new List<FlatTypeSizeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypeSizes.Where(fl => fl.FlatType.FType == ftype).ToList();
            foreach (FlatTypeSize fl in model)
            {
                Mapper.CreateMap<FlatTypeSize, FlatTypeSizeModel>();
                var mdl = Mapper.Map<FlatTypeSize, FlatTypeSizeModel>(fl);
                mdl.FType = fl.FlatType.FType;
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }

}
