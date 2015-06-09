using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
  public class DocumentService
    {

        public int SaveDocument(Agreement ag)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                var model = context.Agreements.Where(g => g.SaleID == ag.SaleID).FirstOrDefault();
                if (model == null)
                {
                    context.Agreements.Add(ag);
                    int i = context.SaveChanges();
                    return i;
                }
                else
                {
                    context.Agreements.Add(ag);
                    context.Entry(ag).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
            }
        }
    }
}
