using AutoMapper;
using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Master
{
    public interface ISerivceTaxService
    {
        int AddSerivceTaxService(ServiceTax model);
        int EditSerivceTaxService(ServiceTax model);
        int DeleteServiceTax(ServiceTax model);
        int DeleteServiceTaxByID(int ServiceTaxID);
        ServiceTax GetServiceTaxByID(int ServiceTaxID);
        ServiceTax GetServiceTax();
        List<ServiceTaxModel> GetServiceTaxList();
        List<ServiceTax> GetAddOnChargeList();
    }
    public class SerivceTaxService : ISerivceTaxService
    {
        public int AddSerivceTaxService(ServiceTax model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.ServiceTaxes.Add(model);
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
        public int EditSerivceTaxService(ServiceTax model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.ServiceTaxes.Add(model);
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
        public int DeleteServiceTax(ServiceTax model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.ServiceTaxes.Add(model);
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
       public int DeleteServiceTaxByID(int ServiceTaxID)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    var model= context.ServiceTaxes.Where(st => st.ServiceTaxID == ServiceTaxID).FirstOrDefault();

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

        public ServiceTax GetServiceTaxByID(int ServiceTaxID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ServiceTaxes.Where(fl => fl.ServiceTaxID == ServiceTaxID).FirstOrDefault();
            return model;
        }
        public ServiceTax GetServiceTax()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ServiceTaxes.Where(st=>st.Status==true).FirstOrDefault();
            return model;
        }
        public List<ServiceTaxModel> GetServiceTaxList()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ServiceTaxes.ToList();
            Mapper.CreateMap<ServiceTax, ServiceTaxModel>().ForMember(des=>des.EndDateSt,ot=>ot.MapFrom(se=>se.EndDate.Value.ToString("dd/MM/yyyy")));
            var mdl=  Mapper.Map<List<ServiceTax>, List<ServiceTaxModel>>(model);
            return mdl;
        }
        public List<ServiceTax> GetAddOnChargeList()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ServiceTaxes.ToList();
            return model;
        }
    }
}
