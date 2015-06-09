using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Sale
{
    public interface IPropertyServices
    {
        int AddFlatInstallment(FlatInstallmentDetailModel flatModel);
    }
    public class PropertyServices : IPropertyServices
    {
        public int AddFlatInstallment(FlatInstallmentDetailModel flatModel)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<FlatInstallmentDetailModel, FlatInstallmentDetail>();
                    var model = Mapper.Map<FlatInstallmentDetailModel, FlatInstallmentDetail>(flatModel);
                    context.FlatInstallmentDetails.Add(model);
                    int i= context.SaveChanges();
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
    }
}
