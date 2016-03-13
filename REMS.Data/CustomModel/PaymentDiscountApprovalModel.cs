using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class PaymentDiscountApprovalModel
    {
        public int PaymentDiscountApprovalID { get; set; }
        public int PaymentDiscountID { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Remark { get; set; }
    
    }
}
