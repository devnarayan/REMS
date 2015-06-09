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
        SaleFlatModel GetSaleFlat(int saleid);
        SaleFlatModel GetSaleFlatByFlatID(int saleid);
    }
    public class SaleFlatService:ISaleFlatService
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

        public SaleFlatModel GetSaleFlat(int saleid)
        {
            var model = dbContext.SaleFlats.Where(sf => sf.SaleID == saleid).FirstOrDefault();
            Mapper.CreateMap<SaleFlat, SaleFlatModel>();
            return  Mapper.Map<SaleFlat, SaleFlatModel>(model);
        }
        public SaleFlatModel GetSaleFlatByFlatID(int flatid)
        {
            var model = dbContext.SaleFlats.Where(sf => sf.FlatID == flatid).FirstOrDefault();
            Mapper.CreateMap<SaleFlat, SaleFlatModel>();
            return Mapper.Map<SaleFlat, SaleFlatModel>(model);
        }
    }
}