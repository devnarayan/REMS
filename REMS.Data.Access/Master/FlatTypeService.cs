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
    public interface IFlatTypeService
    {
        int AddFlatType(FlatType model);
        int EditFlatType(FlatType model);
        int DeleteFlatType(FlatType model);
        FlatTypeModel GetFlatTypeByID(int flatTypeID);
        FlatTypeModel GetFlatTypeByFType(string FType);
        List<FlatTypeModel> GetFlatTypeList();
    }
    public class FlatTypeService : IFlatTypeService
    {
        public int AddFlatType(FlatType model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypes.Add(model);
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
        public int EditFlatType(FlatType model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypes.Add(model);
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
        public int DeleteFlatType(FlatType model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatTypes.Add(model);
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
        public FlatTypeModel GetFlatTypeByID(int flatTypeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypes.Where(fl => fl.FlatTypeID == flatTypeID).FirstOrDefault();
            Mapper.CreateMap<FlatType, FlatTypeModel>();
            var mdl = Mapper.Map<FlatType, FlatTypeModel>(model);
            return mdl;
        }
        public FlatTypeModel GetFlatTypeByFType(string FType)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypes.Where(fl => fl.FType == FType).FirstOrDefault();
            Mapper.CreateMap<FlatType, FlatTypeModel>();
            var mdl = Mapper.Map<FlatType, FlatTypeModel>(model);
            return mdl;
        }
        public List<FlatTypeModel> GetFlatTypeList()
        {
            List<FlatTypeModel> fmodel = new List<FlatTypeModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatTypes.ToList();
            foreach (FlatType fl in model)
            {
                Mapper.CreateMap<FlatType, FlatTypeModel>();
                var mdl = Mapper.Map<FlatType, FlatTypeModel>(fl);
                fmodel.Add(mdl);
            }
            return fmodel;
        }
    }
}
