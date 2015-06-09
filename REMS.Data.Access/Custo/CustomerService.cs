using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
    public interface ICustomerService
    {
        int AddCustomer(CustomerModel model);
        string updateCustomer(CustomerModel model);
        CustomerModel GetCustomerByID(int customerid);
        CustomerModel GetCustomerByFlatID(int flatid);
        CustomerModel GetCustomerBySaleID(int saleid);
    }
    public class CustomerService : ICustomerService
    {
        private readonly REMSDBEntities dbContext;
        public CustomerService()
        {
            dbContext = new REMSDBEntities();
        }
        public int AddCustomer(CustomerModel model)
        {
            using (REMSDBEntities dbContext = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<CustomerModel, Customer>();
                    var mdl = Mapper.Map<CustomerModel, Customer>(model);
                    dbContext.Customers.Add(mdl);
                    int i = dbContext.SaveChanges();

                    return i;
                   // return mdl.CustomerID;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public string updateCustomer(CustomerModel model)
        {
            REMSDBEntities dbContext = new REMSDBEntities();
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            if (model.DateOfBirthst != null)
            {
                model.DateOfBirth = Convert.ToDateTime(model.DateOfBirthst, dtinfo);
            }
            if (model.CoDOBst != null)
            {
                model.CoDOB = Convert.ToDateTime(model.CoDOBst, dtinfo);
            }
            if (model.SecCoDOBst != null)
            {
                model.SecCoDOB = Convert.ToDateTime(model.SecCoDOBst, dtinfo);
            }
            Mapper.CreateMap<CustomerModel, Customer>();
            var cDetail = Mapper.Map<CustomerModel, Customer>(model);
            int i = dbContext.sp_Customer(cDetail.CustomerID, cDetail.AppTitle, cDetail.FName, cDetail.MName, cDetail.LName, cDetail.Title, cDetail.PName, cDetail.Address1, cDetail.Address2, cDetail.City, cDetail.State, cDetail.Country, cDetail.PAN, cDetail.MobileNo, cDetail.DateOfBirth, cDetail.CoAppTitle, cDetail.CoFName, cDetail.CoMName, cDetail.CoLName, cDetail.CoAddress1, cDetail.CoAddress2, cDetail.CoCity, cDetail.CoState, cDetail.CoCountry, cDetail.CoPAN, cDetail.CoMobileNo, cDetail.AlternateMobile, cDetail.LandLine, cDetail.EmailID, cDetail.AlternateEmail, cDetail.LoanAmount, cDetail.LienField, cDetail.BankID, cDetail.BankBranch, cDetail.SecCoAppTitle, cDetail.SecCoFName, cDetail.SecCoMName, cDetail.SecCoLName, cDetail.SecCoAddress1, cDetail.SecCoAddress2, cDetail.SecCoCity, cDetail.SecCoState, cDetail.SecCoCountry, cDetail.SecCoMobileNo, cDetail.SecCoPAN, cDetail.CoTitle, cDetail.CoPName, cDetail.SecCoTitle, cDetail.SecCoPName, cDetail.Remarks, cDetail.CoDOB, cDetail.SecCoDOB, cDetail.PinCode, cDetail.ExecutiveName, cDetail.CoPinCode, cDetail.SecCoPinCode);
            return i.ToString();
        }
        public CustomerModel GetCustomerByID(int customerid)
        {
            try
            {
                var model= dbContext.Customers.Where(cust => cust.CustomerID == customerid).FirstOrDefault();
                Mapper.CreateMap<Customer, CustomerModel>();
                var mdl= Mapper.Map<Customer, CustomerModel>(model);
                return mdl;
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return null;
            }
        }
        public CustomerModel GetCustomerByFlatID(int flatid)
        {
            try
            {
                var model = dbContext.Customers.Where(cust => cust.SaleFlat.FlatID == flatid).FirstOrDefault();
                Mapper.CreateMap<Customer, CustomerModel>();
                var mdl = Mapper.Map<Customer, CustomerModel>(model);
                return mdl;
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return null;
            }
        }
        public CustomerModel GetCustomerBySaleID(int saleid)
        {
            try
            {
                var model = dbContext.Customers.Where(cust => cust.SaleID== saleid).FirstOrDefault();
                Mapper.CreateMap<Customer, CustomerModel>();
                var mdl = Mapper.Map<Customer, CustomerModel>(model);
                return mdl;
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return null;
            }
        }

       
    }
}