using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class PaymentDiscountModel
    {
        public int PaymentDiscountID { get; set; }
        public int SaleID { get; set; }
        public string PaymentType { get; set; }
        public Nullable<System.DateTime> ReqDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string ReqDateSt { get; set; }

        public string AuthUserNames { get; set; }
        public string UserNames { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<DateTime> AppprovalDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string FlatNo { get; set; }
        public string SaleStatus { get; set; }
        public string SaleDateSt { get; set; }
        public string PlanName { get; set; }
        public string CustomerName { get; set; }
    }
}
