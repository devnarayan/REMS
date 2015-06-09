using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class FlatChargeModel
    {
        public int FlatChargeID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public Nullable<int> AdditionalChargeID { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string ChargeName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string ChargeType { get; set; }

    
    }
}
