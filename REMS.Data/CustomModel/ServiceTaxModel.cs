using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class ServiceTaxModel
    {
        public int ServiceTaxID { get; set; }
        public string ServiceTaxName { get; set; }
        public Nullable<decimal> ServiceTaxPer { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EndDateSt { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
