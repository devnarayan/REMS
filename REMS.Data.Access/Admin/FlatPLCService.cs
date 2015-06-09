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
    
    public interface IFlatPLCService
    {
        int AddFlatPLC(FlatPLC model);
        int EditFlatPLC(FlatPLC model);
        int DeleteFlatPLC(FlatPLC model);
        FlatPLCModel GetFlatPLCByID(int flatPLCID);
        List<FlatPLCModel> GetFlatPLCListByFlatID(int flatID);
        bool IsPLCinFlat(int plcid, int flatid);
        decimal? TotalFlatPLCAmount(int flatId);
    }
    public class FlatPLCService : IFlatPLCService
    {
        public int AddFlatPLC(FlatPLC model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatPLCs.Add(model);
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
        public int EditFlatPLC(FlatPLC model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatPLCs.Add(model);
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
        public int DeleteFlatPLC(FlatPLC model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.FlatPLCs.Add(model);
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
        public FlatPLCModel GetFlatPLCByID(int flatPLCID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatPLCs.Where(fl => fl.FlatPLCID == flatPLCID).FirstOrDefault();
            Mapper.CreateMap<FlatPLC, FlatPLCModel>();
            var mdl = Mapper.Map<FlatPLC, FlatPLCModel>(model);
            mdl.AmountSqFt = model.PLC.AmountSqFt;
            mdl.PLCName = model.PLC.PLCName;
            return mdl;
        }
        public List<FlatPLCModel> GetFlatPLCListByFlatID(int flatID)
        {
            List<FlatPLCModel> fmodel = new List<FlatPLCModel>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.FlatPLCs.Where(fl => fl.FlatID == flatID).ToList();
            foreach (FlatPLC fl in model)
            {
                Mapper.CreateMap<FlatPLC, FlatPLCModel>();
                var mdl = Mapper.Map<FlatPLC, FlatPLCModel>(fl);
                mdl.AmountSqFt = fl.PLC.AmountSqFt;
                mdl.PLCName = fl.PLC.PLCName;
                fmodel.Add(mdl);
            }
            return fmodel;
        }

        public bool IsPLCinFlat(int plcid,int flatid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    FlatPLC model = context.FlatPLCs.Where(pl => pl.PLCID == plcid && pl.FlatID == flatid).FirstOrDefault();
                    if (model.FlatID == null) return false;
                    else return true;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return false;
                }
            }
        }
       public decimal? TotalFlatPLCAmount(int flatId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    decimal? model = context.FlatPLCs.Where(pl=>pl.FlatID == flatId).Sum(sm=>sm.PLC.AmountSqFt);
                    return model;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
    }
}
