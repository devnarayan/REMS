using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REMS.Data.DataModel;

namespace REMS.Data.Access.Sale
{
    public interface IPaymnetServcie
    {
        List<PaymentModel> GetPaymentListBySaleID(int saleid);
        List<BankMaster> GetBank();
    }
    public class PaymentService : IPaymnetServcie
    {
        public readonly REMSDBEntities dbContext;
        public PaymentService()
        {
            dbContext = new REMSDBEntities();
        }
        public List<PaymentModel> GetPaymentListBySaleID(int saleid)
        {
            var model = dbContext.Payments.Where(py => py.SaleID == saleid).ToList();
            Mapper.CreateMap<Payment, PaymentModel>().ForMember(dest => dest.PaymentDateSt, opts => opts.MapFrom(src => src.PaymentDate.Value.ToString("dd/MM/yyyy"))); ;
            var mdl = Mapper.Map<List<Payment>, List<PaymentModel>>(model);           
            return mdl;
        }
        public List<BankMaster> GetBank()
        {
            var bank = dbContext.BankMasters.ToList();
            return bank;
        }

    }
}
