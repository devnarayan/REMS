using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Sale
{
    public interface ISaleFlatService
    {
        int AddSaleFlat(SaleFlatModel model);
        int EditSaleFlat(SaleFlatModel model);
        int EditSaleFlatRegenerate(SaleFlatModel model);
        SaleFlatModel GetSaleFlat(int saleid);
        SaleFlatModel GetSaleFlatByFlatID(int saleid);
        bool UpdateSaleStatus(string username, int flatid, string status);
        bool IsSaled(int FlatID);
    }
    public class SaleFlatService : BaseActivity, ISaleFlatService
    {
        #region Private Fields
        private readonly REMSDBEntities dbContext;
        #endregion
        public SaleFlatService()
        {
            dbContext = new REMSDBEntities();
        }
        public int AddSaleFlat(SaleFlatModel model)
        {
            try
            {
                Mapper.CreateMap<SaleFlatModel, SaleFlat>();
                var mdl = Mapper.Map<SaleFlatModel, SaleFlat>(model);
                dbContext.SaleFlats.Add(mdl);
                int i = dbContext.SaveChanges();
                return mdl.SaleID;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return 0;
            }
        }
        public int EditSaleFlat(SaleFlatModel model)
        {
            try
            {
                Mapper.CreateMap<SaleFlatModel, SaleFlat>();
                var mdl = Mapper.Map<SaleFlatModel, SaleFlat>(model);
                dbContext.SaleFlats.Add(mdl);
                dbContext.Entry(mdl).State = EntityState.Modified;
                int i = dbContext.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return 0;
            }
        }
        public int EditSaleFlatRegenerate(SaleFlatModel model)
        {
            try
            {
                var smdl = dbContext.SaleFlats.Where(fl => fl.FlatID == model.FlatID).FirstOrDefault();
                smdl.UpdateBy = model.UpdateBy;
                smdl.UpdateDate = model.UpdateDate;
                smdl.Status = model.Status;
                smdl.PlanName = model.PlanName;
                smdl.TotalAmount = model.TotalAmount;
                smdl.SaleDate = model.SaleDate;

                dbContext.SaleFlats.Add(smdl);
                dbContext.Entry(smdl).State = EntityState.Modified;
                int i = dbContext.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return 0;
            }
        }

        public SaleFlatModel GetSaleFlat(int saleid)
        {
            var model = dbContext.SaleFlats.Where(sf => sf.SaleID == saleid).FirstOrDefault();
            Mapper.CreateMap<SaleFlat, SaleFlatModel>();
            return Mapper.Map<SaleFlat, SaleFlatModel>(model);
        }
        public SaleFlatModel GetSaleFlatByFlatID(int flatid)
        {
            var model = dbContext.SaleFlats.Where(sf => sf.FlatID == flatid).FirstOrDefault();
            Mapper.CreateMap<SaleFlat, SaleFlatModel>();
            return Mapper.Map<SaleFlat, SaleFlatModel>(model);
        }
        public bool UpdateSaleStatus(string username, int flatid, string status)
        {
            bool bl = UpdateFlatSaleStatus(flatid, username, status);
            return bl;
        }
        public bool IsSaled(int FlatID)
        {
            int i = dbContext.SaleFlats.Where(sl => sl.FlatID == FlatID).Count();
            if (i > 0) return true;
            else return false;
        }

    }
}