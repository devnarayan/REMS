using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class FlatInstallmentDetailModel
    {
        public int InstallmentID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public Nullable<int> PlanInstallmentID { get; set; }
        public Nullable<decimal> BSPAmount { get; set; }
        public Nullable<decimal> PLCAmount { get; set; }
        public Nullable<decimal> AdditionalCAmount { get; set; }
        public Nullable<decimal> OptionalCAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TotalAmtInWords { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public Nullable<int> InstallmentOrder { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> BSPPer { get; set; }
        public Nullable<decimal> PLCPer { get; set; }
        public Nullable<decimal> AdditionalPer { get; set; }
        public Nullable<decimal> OptionalPer { get; set; }
        public Nullable<int> InstallmentServiceTaxID { get; set; }
        public string Installment { get; set; }
        public string DueDateSt { get; set; }
    }
}